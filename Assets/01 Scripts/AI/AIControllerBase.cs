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

    protected float playerChaseElapseTime;

    protected float attackCoolDown;

    protected bool shouldStop;

    public float TargetGroundDistance;


    public LayerMask PlayerLayerMask;
    public LayerMask StructureLayerMask;

    public AISearchType SearchType;
    protected ISearchStrategy searchStrategy;

    protected Collider[] overlaps = new Collider[10];

    public BaseWeapon weapon;

    protected virtual void Awake()
    {
        InitializeStrategy();
    }

    protected virtual void OnEnable()
    {
        playerChaseElapseTime = 0f;

    }

    private void Start()
    {
        Target = searchStrategy.SearchTarget();

    }

    protected virtual void Update()
    {

        if (Target.gameObject.CompareTag("Player"))
        {
            playerChaseElapseTime += Time.deltaTime;
        }

        if (!Target.gameObject.activeInHierarchy)
        {
            Target = null;
        }

        if (playerChaseElapseTime > PlayerChaseTime)
        {
            Target = null;
        }

        if (Target == null)
        {
            Target = searchStrategy.SearchTarget();
        }

        Vector3 dir = Target.position - transform.position;
        dir.y = 0f;
        TargetGroundDistance = dir.magnitude;


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
        
        ChaseTarget();
        
    }

    public void OnHit(Transform instigator)
    {
        if (SearchType == AISearchType.BaseOnly) return;
        {
            Debug.Log($"{instigator.name} 한테 맞았음");
        } 

        if (SearchType == AISearchType.General)
        {
            Debug.Log($"{instigator.name} 한테 맞았음");
        }
        if (instigator.CompareTag("Player"))
        {
            playerChaseElapseTime = 0f;

        }

        Target = instigator;
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

        if (count == 0) return false;

        return true;
    }

    protected virtual void TryAttack()
    {
        if (attackCoolDown > 0f) return;

        LayerMask targetLayer = LayerMask.GetMask(LayerMask.LayerToName(Target.gameObject.layer));
        int count = Physics.OverlapSphereNonAlloc(transform.position, AttackRange, overlaps, targetLayer);

        if (count < 1) return;

        if(weapon != null)
        {
            weapon.Attack(Target);
        }

        Debug.Log("공격");
        Debug.DrawRay(transform.position, Target.position - transform.position, Color.red, 3f);
        attackCoolDown = AttackCoolTime;
    }

    protected void InitializeStrategy()
    {
        switch (SearchType)
        {
            case AISearchType.General:
                GeneralSearchStrategy generalStrategy = new GeneralSearchStrategy();

                generalStrategy.Owner = transform;
                generalStrategy.SearchLayerMask = StructureLayerMask;
                generalStrategy.SearchRange = StructureDetectRange;

                searchStrategy = generalStrategy;
                break;
            case AISearchType.PlayerFirst:
                PlayerAggroSearchStrategy playerAggroSearchStrategy = new PlayerAggroSearchStrategy();

                playerAggroSearchStrategy.Owner = transform;
                playerAggroSearchStrategy.PlayerSearchRange = PlayerDetectRange;
                playerAggroSearchStrategy.StructureSearchRange = StructureDetectRange;
                playerAggroSearchStrategy.PlayerLayerMask = PlayerLayerMask;
                playerAggroSearchStrategy.StructureLayerMask = StructureLayerMask;

                searchStrategy = playerAggroSearchStrategy;
                break;
            case AISearchType.DistanceFirst:
                DFSStrategy dfsStrategy = new DFSStrategy();

                dfsStrategy.Owner = transform;
                dfsStrategy.SearchLayerMask = LayerMask.GetMask("Player", "Structure");
                dfsStrategy.SearchRange = 5f;

                searchStrategy = dfsStrategy;
                break;
            case AISearchType.BaseOnly:
                BaseOnlySearchStrategy baseOnlyStrategy = new BaseOnlySearchStrategy();

                searchStrategy = baseOnlyStrategy;
                break;
            case AISearchType.Enemy:
                EnemySearchStrategy enemyStrategy = new EnemySearchStrategy();

                enemyStrategy.Owner = transform;
                enemyStrategy.SearchLayerMask = LayerMask.GetMask("Enemy");
                enemyStrategy.SearchRange = 10f;
                searchStrategy = enemyStrategy;
                break;
        }
    }
    
}
