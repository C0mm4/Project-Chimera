using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    [field: SerializeField] public Transform Target { get; private set; }

    NavMeshAgent agent;

    public float PlayerDetectRange;
    public float StructureDetectRange;
    public float AttackRange;

    public float TargetDist;

    // Player player;
    public LayerMask PlayerLayerMask;
    public LayerMask StructureLayerMask;

    private Collider[] overlaps = new Collider[10];

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

    }
    private void OnEnable()
    {
        Debug.Log("베이스로 목표 설정");
        // Target = StageManager.Instance.Basement.transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            // Target = GameManager.Instance.Player.transform;
        }

        
    }

    private void FixedUpdate()
    {
        // 주변 건물 찾아서 제일 가까운 건물로 타겟 설정
        int count = Physics.OverlapSphereNonAlloc(transform.position, StructureDetectRange, overlaps, StructureLayerMask);
        if (count > 0)
        {
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
        }

        if (Target != null)
        {
            TargetDist = Vector3.Distance(Target.position, transform.position);
            Debug.Log($"{Target.name}을 추적");

            if (TargetDist < AttackRange)
            {
                Debug.Log("공격 범위 내로 들어옴. 공격하기");
                agent.destination = transform.position;
            }
            else
            {
                agent.destination = Target.position;

            }

        }

    }

    private void OnHit()
    {
        Debug.Log("피격 받음 (에너미 캐릭터의 HitReaction에서 이 함수 호출하기)");
        Debug.Log($"에너미 주위 반지름 {PlayerDetectRange} 이내에 플레이어 있는지 감지");

        bool checkPlayer = Physics.CheckSphere(transform.position, PlayerDetectRange, PlayerLayerMask);

        if (checkPlayer)
        {
            // Target = GameManager.Instance.Player;
        }
    }

    private void OnPathFailed()
    {
        // 타겟으로 가는 경로 못찾을 때 실행되는 함수 

    }
}
