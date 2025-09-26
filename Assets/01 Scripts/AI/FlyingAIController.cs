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
    
}
