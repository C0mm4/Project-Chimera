using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class GroundAIController : AIControllerBase
{
    NavMeshAgent agent;
    NavMeshPath path;

    Vector3 randomTargetOffset;


    protected override void Awake()
    {
        base.Awake();

        agent = GetComponent<NavMeshAgent>();
        randomTargetOffset = Random.insideUnitSphere;

    }
    protected override void OnEnable()
    {
        base.OnEnable();

    }

    protected override void Update()
    {
        base.Update();
        
    }


    protected override void ChaseTarget()
    {
        if (shouldStop)
        {
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(Target.position + randomTargetOffset);
               

            
        }
    }

    
}
