using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePopupUI : PopupUIBase
{

    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button backgroundButton;

    private void Start()
    {

        upgradeButton.onClick.AddListener(OnClickUpgradeButton);
        closeButton.onClick.AddListener(OnClickCloseButton);


        if (backgroundButton != null)
        {
            backgroundButton.onClick.AddListener(OnClickCloseButton);
        }
    }


    public void OnClickUpgradeButton()
    {
        Debug.Log("실행");
        UIManager.Instance.ClosePopupUI();
    }

    public void OnClickCloseButton()
    {
        Debug.Log("닫음");
        UIManager.Instance.ClosePopupUI();
    }
}
