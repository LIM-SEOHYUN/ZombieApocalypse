using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyFSM : MonoBehaviour
{
    // 에너미 상태 상수
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }

    // 에너미 상태 변수
    EnemyState m_State;

    // 플레이어 발견 범위
    public float findDistance = 8f;

    // 플레이어 트랜스폼
    Transform player;

    // 공격 가능 범위
    public float attackDistance = 2f;

    // 캐릭터 콘트롤러 컴포넌트
    CharacterController cc;

    // 이동 속도
    public float moveSpeed = 5f;

    // 누적 시간
    float currentTime = 0;

    // 공격 딜레이 시간
    float attackDelay = 2f;

    // 에너미 공격력
    public int attackPower = 3;

    // 초기 위치 저장용 변수
    Vector3 originPos;
    Quaternion originRot;

    // 이동 가능 범위
    public float moveDistance = 20f;

    // 에너미의 체력
    public int hp = 15;

    // 에너미의 최대 체력
    int maxHp = 15;

    // 에너미 hp Slider 변수
    public Slider hpSlider;

    // 애니메이터 변수
    Animator anim;

    void Start()
    {
        m_State = EnemyState.Idle;

        player = GameObject.Find("Soldier").transform;

        cc = GetComponent<CharacterController>();
        originPos = transform.position;
        originRot = transform.rotation;
        anim = transform.GetComponentInChildren<Animator>();
    }

    void Update()
    {
        switch (m_State)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damaged:
                break;
            case EnemyState.Die:
                break;
        }

        hpSlider.value = (float)hp / (float)maxHp;
    }

    void Idle()
    {
        if (Vector3.Distance(transform.position, player.position) < findDistance)
        {
            m_State = EnemyState.Move;
            anim.SetTrigger("IdleToMove");
        }
    }

    void Move()
    {
        if (Vector3.Distance(transform.position, originPos) > moveDistance)
        {
            m_State = EnemyState.Return;
        }

        else if (Vector3.Distance(transform.position, player.position) > attackDistance)
        {
            Vector3 dir = (player.position - transform.position).normalized;
            cc.Move(dir * moveSpeed * Time.deltaTime);
            transform.forward = dir;
        }
        else
        {
            m_State = EnemyState.Attack;
            currentTime = attackDelay;
            anim.SetTrigger("MoveToAttackDelay");
        }
    }

    void Attack()
    {
        if (Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                currentTime = 0;
                anim.SetTrigger("StartAttack");
            }
        }
        else
        {
            m_State = EnemyState.Move;
            currentTime = 0;

            anim.SetTrigger("AttackToMove");
        }
    }
    public void AttackAction()
    {
        player.GetComponent<PlayerMove>().DamageAction(attackPower);
    }

    void Return()
    {
        if (Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            Vector3 dir = (originPos - transform.position).normalized;
            cc.Move(dir * moveSpeed * Time.deltaTime);

            transform.forward = dir;
        }
        else
        {
            transform.position = originPos;
            transform.rotation = originRot;
            m_State = EnemyState.Idle;

            anim.SetTrigger("MoveToIdle");
        }
    }

    public void HitEnemy(int hitPower)
    {
        if (m_State == EnemyState.Damaged || m_State == EnemyState.Die || m_State == EnemyState.Return)
        {
            return;
        }

        hp -= hitPower;

        if (hp > 0)
        {
            m_State = EnemyState.Damaged;

            anim.SetTrigger("Damaged");
            Damaged();
        }
        else
        {
            m_State = EnemyState.Die;
            anim.SetTrigger("Die");
            Die();
        }
    }

    void Damaged()
    {
        StartCoroutine(DamageProcess());
    }

    IEnumerator DamageProcess()
    {
        yield return new WaitForSeconds(1f);
        m_State = EnemyState.Move;
    }

    void Die()
    {
        StopAllCoroutines();
        StartCoroutine(DieProcess());
    }

    IEnumerator DieProcess()
    {
        cc.enabled = false;
        yield return new WaitForSeconds(2f);
        print("사망");
        Destroy(gameObject);
    }
}