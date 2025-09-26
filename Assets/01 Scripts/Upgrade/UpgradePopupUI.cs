using UnityEngine;
using UnityEngine.UI;

public class UpgradePopupUI : PopupUIBase
{
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button closeButton;

    private UpgradeableObject currentObject;

    public void Initialize(UpgradeableObject targetObject)
    {
        currentObject = targetObject;
    }

    void Start()
    {
        upgradeButton.onClick.AddListener(OnInteractionFinished);
        closeButton.onClick.AddListener(OnInteractionFinished);
    }

    void OnInteractionFinished()
    {
        if (currentObject != null)
        {
            currentObject.hasBeenInteractedWith = true;
        }

        UIManager.Instance.ClosePopupUI();
    }
}