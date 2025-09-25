using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public GameObject interactionUIPanel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.OpenUI<InteractionPanelUI>();
            Time.timeScale = 0f; // 게임 일시정지

            UIManager.Instance.SetActiveInteraction(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.CloseUI<InteractionPanelUI>();
            Time.timeScale = 1f; // 게임 재개
            UIManager.Instance.SetActiveInteraction(null);
        }
    }

    public void OnUpgradeButtonClick()
    {
        Debug.Log("눌러");

        UIManager.Instance.CloseUI<InteractionPanelUI>();
        Time.timeScale = 1f; // 게임 재개
    }
}
