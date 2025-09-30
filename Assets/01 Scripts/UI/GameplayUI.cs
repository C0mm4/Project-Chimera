using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : UIBase
{
    [Header("UI 그룹")]
    // 게임 플레이 UI 요소 전체를 담는 부모 오브젝트
    [SerializeField] private GameObject hudGroup;

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
        // 팝업 UI가 비어있을 때만 HUD 그룹을 활성화
        hudGroup.SetActive(UIManager.Instance.PopupStackCount == 0);
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
