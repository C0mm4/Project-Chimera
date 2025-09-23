using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 TargetVelocity;
    [SerializeField] private float rotationSpeed = 10f; // 회전 속도 조절

    [SerializeField] private float MoveSpeed = 5f;
    private bool isDash = false;

    public void OnMove(InputAction.CallbackContext context)
    {
        TargetVelocity = context.ReadValue<Vector2>();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started || context.phase == InputActionPhase.Performed)
        {
            isDash = true;
        }
        else if(context.phase == InputActionPhase.Canceled || context.phase == InputActionPhase.Disabled)
        {
            isDash = false;
        }
    }

    [SerializeField] private bool Test1 = false;

    private void FixedUpdate()
    {
        Vector2 movDelta = TargetVelocity;

        Vector3 forward = transform.forward;
        Vector2 rotationDir = new Vector2(forward.x, forward.z);

        movDelta *= (isDash ? 2 : 1) * Time.fixedDeltaTime * MoveSpeed;

        // Test Code 바라보는 방향에 맞춰서 이동하는 로직
        if(Test1)
            movDelta = Vector2.Dot(movDelta, rotationDir) * rotationDir;

        transform.position += new Vector3(movDelta.x, 0, movDelta.y);
        
        // 이동 중일때만 회전 적용
        if (TargetVelocity != Vector2.zero) 
        {
            float targetAngle = Mathf.Atan2(TargetVelocity.x, TargetVelocity.y) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }
}
