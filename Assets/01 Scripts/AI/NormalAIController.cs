using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NormalAIController : AIControllerBase
{
    NavMeshAgent agent;
    NavMeshPath path;

    public float TargetDist;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();

    }
    protected override void OnEnable()
    {
        base.OnEnable();
        Target = StageManager.Instance.Basement.transform;
        playerChaseStartTime = Time.time;
    }

    private void FixedUpdate()
    {
        if (Time.time - playerChaseStartTime > PlayerChaseTime)
        {
            Target = StageManager.Instance.Basement.transform;
        }

        //if (!FindNearestTargetInRange(StructureDetectRange, StructureLayerMask))
        //{
        //    Target = StageManager.Instance.Basement.transform;
        //}

        if (Target != null)
        {
            TargetDist = Vector3.Distance(Target.position, transform.position);

            LayerMask layer = LayerMask.GetMask(LayerMask.LayerToName(Target.gameObject.layer));
            if (FindNearestTargetInRange(AttackRange, layer))
            {
                agent.destination = transform.position;
            }
            else
            {
                agent.SetDestination(Target.position);
            }

        }

    }

    private bool FindNearestTargetInRange(float range, LayerMask layerMask)
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, range, overlaps, layerMask);

        if (count < 1) return false;

        float minDist = 12345;
        int targetIdx = -1;

        for (int i = 0; i < count; ++i)
        {
            float dist = Vector3.Distance(transform.position, overlaps[i].transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                targetIdx = i;
            }
        }

        Target = overlaps[targetIdx].transform;

        return true;
    }

    public void OnHit()
    {

        // Debug.Log($"에너미 주위 반지름 {PlayerDetectRange} 이내에 플레이어 있는지 감지");

        // bool checkPlayer = Physics.CheckSphere(transform.position, PlayerDetectRange, PlayerLayerMask);

        // if (checkPlayer)
        // {
            // Debug.Log("플레이어 찾음");
            //playerChaseStartTime = Time.time;
            //Target = GameManager.Instance.Player.transform;
        // }
        playerChaseStartTime = Time.time;
        Target = GameManager.Instance.Player.transform;
    }

    private void OnPathFailed()
    {
        // 타겟으로 가는 경로 못찾을 때 실행되는 함수 

    }
}
