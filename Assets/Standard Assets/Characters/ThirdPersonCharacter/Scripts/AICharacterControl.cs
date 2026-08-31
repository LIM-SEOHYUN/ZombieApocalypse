using System;
using UnityEngine;
using UnityEngine.AI;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour
    {
        public NavMeshAgent agent { get; private set; }
        public ThirdPersonCharacter character { get; private set; }
        public Transform target;
        public float detectionRange = 10f; // 타겟 감지 범위 설정
        private bool isTargetDetected = false; // 타겟 인식 여부

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();
            agent.updateRotation = false;
            agent.updatePosition = true;
        }

        private void Update()
        {
            if (target != null)
            {
                float distance = Vector3.Distance(transform.position, target.position);

                if (distance <= detectionRange)
                {
                    isTargetDetected = true;
                }

                if (isTargetDetected)
                {
                    agent.SetDestination(target.position);

                    if (agent.remainingDistance > agent.stoppingDistance)
                    {
                        character.Move(agent.desiredVelocity, false, false);
                    }
                    else
                    {
                        character.Move(Vector3.zero, false, false);
                    }
                }
            }
            else
            {
                character.Move(Vector3.zero, false, false);
            }
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }
    }
}
