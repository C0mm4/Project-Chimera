using UnityEngine;
using UnityEngine.AI;

public class BarrackUnitAI : AIControllerBase
{
    [SerializeField] private BaseWeapon currentWeapon;

    NavMeshAgent agent;

    Vector3 randomTargetOffset;

    public WeaponTypes weaponTypes;

    private BarrackUnitStatus barrackUnitStatus;

    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        barrackUnitStatus = GetComponent<BarrackUnitStatus>();

        randomTargetOffset = Random.insideUnitSphere;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Update()
    {


        if (Target == null )
        {
            Target = searchStrategy.SearchTarget();
        }

        if (Target != null )
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

    protected override bool IsAttackable()
    {
        if (Target == null) return false;

        LayerMask targetLayer = LayerMask.GetMask(LayerMask.LayerToName(Target.gameObject.layer));
        int count = Physics.OverlapSphereNonAlloc(transform.position, AttackRange, overlaps, targetLayer);

        if (count == 0) return false;

        return true;
    }

    protected override void TryAttack()
    {
        if (attackCoolDown > 0f) return;

        LayerMask targetLayer = LayerMask.GetMask(LayerMask.LayerToName(Target.gameObject.layer));
        int count = Physics.OverlapSphereNonAlloc(transform.position, AttackRange, overlaps, targetLayer);

        if (count < 1) return;

        //타입별 공격
        switch (weaponTypes)
        {
            case WeaponTypes.Bow:
                currentWeapon.Attack(Target);
                break;
            case WeaponTypes.Sword:
                attackCoolDown = AttackCoolTime;
                break;
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
