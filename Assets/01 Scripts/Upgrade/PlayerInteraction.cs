using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private UpgradePopupUI upgradePanel;

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<UpgradeableObject>() != null)
        {
            Debug.Log("업그레이드 오브젝트 감지!");

            if (upgradePanel != null)
            {
                upgradePanel.Open();
            }
            else
            {
                Debug.LogError("PlayerInteraction 스크립트에 UpgradePanel이 연결되지 않았습니다!");
            }
        }
    }
}