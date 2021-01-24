using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform target;
    private Rigidbody enemyRigid;
    private NavMeshAgent pathFinder;        // 경로 계산 navMeshAgent이용

    private bool hasTarget
    {
        get
        {
            if (target != null)     // 타겟이 존재하면 true
                return true;

            return false;
        }
    }
    void Awake()
    {
        pathFinder = GetComponent<NavMeshAgent>();
        enemyRigid = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hasTarget)
            pathFinder.SetDestination(target.position);
    }

    void FixedUpdate()
    {
        FreezeVelocity();
    }

    void FreezeVelocity()
    {
        enemyRigid.velocity = Vector3.zero;
        enemyRigid.angularVelocity = Vector3.zero;
    }
}
