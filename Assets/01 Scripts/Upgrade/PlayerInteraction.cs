using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<UpgradeableObject>() != null)
        {
            Debug.Log("감지함");
            UIManager.Instance.OpenPopupUI<UpgradePopupUI>();
        }
        else
        {
            Debug.Log($"[PlayerInteraction] UpgradeableObject script NOT found on {other.name}");
        }
    }
}
