using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingAIController : AIControllerBase
{
    CharacterController characterController;

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

        transform.position = new Vector3(transform.position.x, 3f, transform.position.z);

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
}
