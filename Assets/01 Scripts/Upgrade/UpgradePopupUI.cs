using UnityEngine;
using UnityEngine.UI;

public class UpgradePopupUI : MonoBehaviour
{
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button closeButton;

    private PlayerInteraction playerInteraction;

    public void Initialize(PlayerInteraction owner)
    {
        playerInteraction = owner;
    }

    void Start()
    {
        upgradeButton.onClick.AddListener(CloseUI);
        closeButton.onClick.AddListener(CloseUI);
    }

    void CloseUI()
    {
        if (playerInteraction != null)
        {
            playerInteraction.CloseUIAndResumeGame();
        }
    }
}