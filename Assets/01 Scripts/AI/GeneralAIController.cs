using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class GeneralAIController : AIControllerBase
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


    public void OnHit(Transform instigator)
    {

        // Debug.Log($"에너미 주위 반지름 {PlayerDetectRange} 이내에 플레이어 있는지 감지");

        // bool checkPlayer = Physics.CheckSphere(transform.position, PlayerDetectRange, PlayerLayerMask);

        // if (checkPlayer)
        // {
        // Debug.Log("플레이어 찾음");
        //playerChaseStartTime = Time.time;
        //Target = GameManager.Instance.Player.transform;
        // }
        Debug.Log($"{instigator.name}");
        if (instigator.CompareTag("Player"))
        {
            playerChaseElapseTime = 0f;

        }

        Target = instigator;
    }

    private void OnPathFailed()
    {
        // 타겟으로 가는 경로 못찾을 때 실행되는 함수 

    }

    protected override void ChaseTarget()
    {
        agent.SetDestination(Target.position);
    }

    
}
