using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerAttack))]
public class PlayerController : MonoBehaviour
{
    private Vector2 TargetVelocity;
    [SerializeField] private float rotationSpeed = 10f; // 회전 속도 조절
    [SerializeField] private Transform horseTrans;
    [SerializeField] private Transform modelTrans;
    [SerializeField] private float MoveSpeed = 5f;
    private bool isDash = false;

    PlayerAttack playerAttack;

    private void Awake()
    {
        playerAttack = GetComponent<PlayerAttack>();
    }

    private void Start()
    {
        MoveSpeed = GetComponent<CharacterStats>().data.moveSpeed;
    }

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

    private void FixedUpdate()
    {
        Vector2 movDelta = TargetVelocity;

        movDelta *= (isDash ? 2 : 1) * Time.fixedDeltaTime * MoveSpeed;

        transform.position += new Vector3(movDelta.x, 0, movDelta.y);
        
        // 이동 중일때만 회전 적용
        if (TargetVelocity != Vector2.zero) 
        {
            float targetAngle = Mathf.Atan2(TargetVelocity.x, TargetVelocity.y) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);

            horseTrans.rotation = Quaternion.Lerp(horseTrans.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            if (!playerAttack.isAttacking)
            {
                modelTrans.rotation = Quaternion.Lerp(modelTrans.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            }
        }
    }
    
}
