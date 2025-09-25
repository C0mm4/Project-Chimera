using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIControllerBase : MonoBehaviour
{
    [field: SerializeField] public Transform Target { get; protected set; }

    public float PlayerDetectRange;
    public float StructureDetectRange;
    public float AttackRange;
    public float PlayerChaseTime;
    public float AttackCoolTime;

    protected float playerChaseStartTime;

    protected float attackCoolDown;

    protected bool isStop;

    public LayerMask PlayerLayerMask;
    public LayerMask StructureLayerMask;

    protected Collider[] overlaps = new Collider[10];

    protected virtual void OnEnable()
    {
        playerChaseStartTime = Time.time;

    }

    protected virtual void Update()
    {
        if (attackCoolDown > 0)
        {
            attackCoolDown = Mathf.Clamp(attackCoolDown - Time.deltaTime, 0, attackCoolDown);
        }

        if (IsAttackable())
        {
            isStop = true;
            TryAttack();
        }
        else
        {
            isStop = false;
        }
    }

    protected virtual bool IsAttackable()
    {
        if (Target == null) return false;

        LayerMask targetLayer = LayerMask.GetMask(LayerMask.LayerToName(Target.gameObject.layer));

        int count = Physics.OverlapSphereNonAlloc(transform.position, AttackRange, overlaps, targetLayer);

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

    protected virtual void TryAttack()
    {
        if (attackCoolDown > 0f) return;
        Debug.Log("공격");
        attackCoolDown = AttackCoolTime;
    }

}
