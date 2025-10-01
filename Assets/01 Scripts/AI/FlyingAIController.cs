using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingAIController : AIControllerBase
{
    CharacterController characterController;
    RaycastHit groundHit = new();

    protected override void ChaseTarget()
    {
        
        Vector3 dir = Target.position - transform.position;
        dir.y = 0;

        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = rot;
        
        if (!shouldStop)
        {
            characterController.Move(dir.normalized * 3f * Time.fixedDeltaTime);

        }

        Debug.DrawRay(transform.position, dir);
    }
   

    protected override void Awake()
    {
        base.Awake();
        
        characterController = GetComponent<CharacterController>();
       
    }

    protected override void Update()
    {
        base.Update();

        //transform.position = new Vector3(transform.position.x, 3f, transform.position.z);

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        Physics.Raycast(transform.position, Vector3.down, out groundHit, 10f, LayerMask.GetMask("Ground"));
        if (groundHit.collider)
        {
            Debug.Log(groundHit.point.y);
            transform.position = new Vector3(transform.position.x, groundHit.point.y + 3f, transform.position.z);
        }
    }

    protected override bool IsAttackable()
    {
        if (Target == null) return false;

        Vector3 groundPosition = transform.position;
        groundPosition.y = 0;

        LayerMask targetLayer = LayerMask.GetMask(LayerMask.LayerToName(Target.gameObject.layer));
        int count = Physics.OverlapSphereNonAlloc(groundPosition, AttackRange, overlaps, targetLayer);
        if (count == 0) return false;

        return true;

    }


    protected override void TryAttack()
    {
        if (attackCoolDown > 0f) return;

        Vector3 groundPosition = transform.position;
        groundPosition.y = 0;

        LayerMask targetLayer = LayerMask.GetMask(LayerMask.LayerToName(Target.gameObject.layer));

        int count = Physics.OverlapSphereNonAlloc(groundPosition, AttackRange, overlaps, targetLayer);

        if (count < 1) return;

        if (weapon != null)
        {
            weapon.Attack(Target);
        }

        Debug.Log("공격");
        Debug.DrawRay(transform.position, Target.position - transform.position, Color.red, 3f);
        attackCoolDown = AttackCoolTime;
    }
}
