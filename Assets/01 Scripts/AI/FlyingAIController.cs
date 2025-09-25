using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingAIController : AIControllerBase
{
    CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    private void FixedUpdate()
    {
        Vector3 dir = Target.position - transform.position;
        dir.y = 0;

        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = rot;
        
        if (!isStop)
        {
            characterController.Move(dir.normalized * 3f * Time.fixedDeltaTime);

        }

        Debug.DrawRay(transform.position, dir);
    }
}
