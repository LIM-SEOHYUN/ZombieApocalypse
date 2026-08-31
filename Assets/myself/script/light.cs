using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class light : MonoBehaviour
{
   public Transform target;

    private void Update()
    {
        if (target != null)
        {
            transform.position = target.position + new Vector3(0, 2, 0);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}