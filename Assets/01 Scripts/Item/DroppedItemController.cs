using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItemController : MonoBehaviour
{
    private Rigidbody rb;
    private bool hasLanded = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(hasLanded)
        {
            return;
        }

        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            rb.isKinematic = true;

            hasLanded = true;
        }
    }
}
