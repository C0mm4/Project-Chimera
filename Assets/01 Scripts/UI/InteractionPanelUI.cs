using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InteractionPanelUI : UIBase
{
    [SerializeField] private Button upgradeButton;

    private void Start()
    {
        upgradeButton.onClick.AddListener(UIManager.Instance.OnInteractionUpgradeButton);
    }

}

