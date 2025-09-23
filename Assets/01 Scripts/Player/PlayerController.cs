using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Vector2 TargetVelocity { get; private set; }

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

    private void FixedUpdate()
    {
        Vector2 movDelta = TargetVelocity * (MoveSpeed * (isDash ? 2 : 1)) * Time.fixedDeltaTime;

        transform.position += new Vector3(movDelta.x, 0, movDelta.y);
        if(TargetVelocity != Vector2.zero)
            transform.eulerAngles = new Vector3(0f, Mathf.Atan2(TargetVelocity.x, TargetVelocity.y) * Mathf.Rad2Deg, 0f);
    }
}
