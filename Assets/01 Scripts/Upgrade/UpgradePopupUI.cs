using UnityEngine;
using UnityEngine.UI;

public class UpgradePopupUI : MonoBehaviour
{
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button backgroundButton;

    void Start()
    {
        upgradeButton.onClick.AddListener(Close);
        closeButton.onClick.AddListener(Close);

        if (backgroundButton != null)
        {
            backgroundButton.onClick.AddListener(Close);
        }

        gameObject.SetActive(false);
    }

    public void Open()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;      
        Debug.Log("업그레이드 창 열림 & 게임 일시정지");
    }

    public void Close()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;        
        Debug.Log("업그레이드 창 닫힘 & 게임 재개");
    }
}