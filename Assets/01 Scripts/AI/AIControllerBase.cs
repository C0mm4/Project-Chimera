using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class AIControllerBase : MonoBehaviour
{
    [field: SerializeField] public Transform Target { get; protected set; }

    public float PlayerDetectRange;
    public float StructureDetectRange;
    public float AttackRange;
    public float PlayerChaseTime;
    public float AttackCoolTime;

    protected float playerChaseStartTime;

    protected float attackCoolDown;

    protected bool shouldStop;

    public LayerMask PlayerLayerMask;
    public LayerMask StructureLayerMask;

    protected ISearchStrategy searchStrategy;

    protected Collider[] overlaps = new Collider[10];

    protected Transform closestStructure;

    protected virtual void Awake()
    {
    }

    protected virtual void OnEnable()
    {
        playerChaseStartTime = Time.time;

    }

    protected virtual void Update()
    {
        if (Time.time - playerChaseStartTime > PlayerChaseTime)
        {
            Target = null;
        }

        if (Target == null)
        {
            Target = searchStrategy.SearchTarget();
        }

        if (attackCoolDown > 0)
        {
            attackCoolDown = Mathf.Clamp(attackCoolDown - Time.deltaTime, 0, attackCoolDown);
        }

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

    protected virtual void FixedUpdate()
    {
        if (!shouldStop)
        {
            ChaseTarget();
        }
    }

    // 일반 ai : 피격 시 공격 유닛을 타겟팅, 가까운 구조물 공격, 그 외 기지 공격
    // greedy: 제일 가까운 플레이어 or 구조물 타게팅
    // 플레이어 우선순위 ai: 플레이어 감지 범위 이내에 플레이어 존재하면 플레이어 타겟팅, 아니면 일반으로
    // 기지 강제공격 ai: 무조건 기지만 공격함
    protected abstract void ChaseTarget();

    protected virtual bool IsAttackable()
    {
        if (Target == null) return false;

        LayerMask targetLayer = LayerMask.GetMask(LayerMask.LayerToName(Target.gameObject.layer));

        int count = Physics.OverlapSphereNonAlloc(transform.position, AttackRange, overlaps, targetLayer);

        if (count < 1) return false;

        return true;
        
    }

    protected virtual void TryAttack()
    {
        if (attackCoolDown > 0f) return;
        Debug.Log("공격");
        attackCoolDown = AttackCoolTime;
    }

    protected abstract void InitStrategy();
    
}
