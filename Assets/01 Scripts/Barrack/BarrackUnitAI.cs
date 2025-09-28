using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BarrackUnitAI : AIControllerBase
{
    NavMeshAgent agent;

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


        if (Target == null)
        {
            Target = searchStrategy.SearchTarget();
        }

        if (Target != null)
        {
            if (attackCoolDown > 0)
            {
                attackCoolDown = Mathf.Clamp(attackCoolDown - Time.deltaTime, 0, attackCoolDown);
            }

            Vector3 dir = Target.position - transform.position;
            dir.y = 0f;
            TargetGroundDistance = dir.magnitude;

            if (IsAttackable())
            {
                shouldStop = true;
                TryAttack();
            }
            else
            {
                shouldStop = false;
            }
        }
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
            if(Target != null)
            agent.SetDestination(Target.position + randomTargetOffset);
        }
    }

}
