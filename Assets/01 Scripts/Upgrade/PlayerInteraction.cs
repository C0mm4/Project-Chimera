using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField]
    private UpgradePopupUI upgradePanel;

    void Start()
    {
        if (upgradePanel != null)
        {
            upgradePanel.gameObject.SetActive(false);
            upgradePanel.Initialize(this);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        UpgradeableObject objectToInteract = other.GetComponent<UpgradeableObject>();

        if (objectToInteract != null && !objectToInteract.hasBeenInteractedWith)
        {
            if (upgradePanel != null && !upgradePanel.gameObject.activeInHierarchy)
            {
                upgradePanel.gameObject.SetActive(true);
                Time.timeScale = 0f;

                objectToInteract.hasBeenInteractedWith = true;
            }
        }
    }

    public void CloseUIAndResumeGame()
    {
        if (upgradePanel != null)
        {
            upgradePanel.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}