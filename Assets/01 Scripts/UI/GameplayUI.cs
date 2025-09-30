using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : UIBase
{
    [Header("UI 요소 연결")]
    [SerializeField] private Button pauseButton;
    [SerializeField] private TextMeshProUGUI goldText;

    protected override void OnOpen()
    {
        base.OnOpen();
        pauseButton.onClick.AddListener(OnPauseButtonClicked);

        StageManager.Instance.OnGoldChanged += UpdateGoldText;

        UpdateGoldText(StageManager.data.Gold); // 처음 UI가 켜길 때, 한 번 업데이트
    }

    protected override void OnClose()
    {
        base.OnClose();
        pauseButton.onClick.RemoveAllListeners();

        if (StageManager.IsCreatedInstance()) // StageManager가 파괴되었을 경우를 대비
        {
            StageManager.Instance.OnGoldChanged -= UpdateGoldText;
        }
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

    // ============ 골드 변경 이벤트

    private void UpdateGoldText(int newGoldAmount)
    {
        goldText.text = newGoldAmount.ToString();
    }
}
