using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class GroundAIController : AIControllerBase
{
    NavMeshAgent agent;

    protected override void Awake()
    {
        base.Awake();

        agent = GetComponent<NavMeshAgent>();

    }
    protected override void OnEnable()
    {
        base.OnEnable();

    }

    protected override void Update()
    {
        base.Update();
    }


    

    private void OnPathFailed()
    {
        // 타겟으로 가는 경로 못찾을 때 실행되는 함수 

    }

    protected override void ChaseTarget()
    {
        if (shouldStop)
        {
            agent.SetDestination(transform.position);
        }
        else
        {
            agent.SetDestination(Target.position);
        }
    }

    
}
