using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : UIBase
{
    [Header("UI 요소 연결")]
    [SerializeField] private Button pauseButton;

    protected override void OnOpen()
    {
        base.OnOpen();
        pauseButton.onClick.AddListener(OnPauseButtonClicked);
    }

    protected override void OnClose()
    {
        base.OnClose();
        pauseButton.onClick.RemoveAllListeners();
    }

    private void Update()
    {
        // 팝업 UI가 열려있으면 일시정지 버튼을 숨김
        if (UIManager.Instance.PopupStackCount > 0)
        {
            if (pauseButton.gameObject.activeSelf)
            {
                pauseButton.gameObject.SetActive(false);
            }
        }
        else
        {
            if (!pauseButton.gameObject.activeSelf)
            {
                pauseButton.gameObject.SetActive(true);
            }
        }
    }

    private void OnPauseButtonClicked()
    {
        UIManager.Instance.OpenPopupUI<PauseUI>();
    }
}
