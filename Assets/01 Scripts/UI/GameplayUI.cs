using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : UIBase
{
    [Header("HUD 그룹")]
    [SerializeField] private GameObject HUDGroup;

    [Header("UI 요소 연결")]
    [SerializeField] private Button pauseButton;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private Button nextStageBtn;
    [SerializeField] private TMP_Text nextStageText;

    protected override void OnOpen()
    {
        base.OnOpen();
        pauseButton.onClick.AddListener(OnPauseButtonClicked);
        OnStageEnd();

        StageManager.Instance.OnGoldChanged += UpdateGoldText;
        StageManager.Instance.OnStageClear += OnStageEnd;
        StageManager.Instance.OnStageFail += OnStageEnd;

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
            if (HUDGroup.gameObject.activeSelf)
            {
                HUDGroup.gameObject.SetActive(false);
            }
        }
        else
        {
            if (!HUDGroup.gameObject.activeSelf)
            {
                HUDGroup.gameObject.SetActive(true);
            }
        }
    }

    private void OnPauseButtonClicked()
    {
        UIManager.Instance.OpenPopupUI<PauseUI>();
    }

    private void OnNextStageButtonClicked()
    {
        StageManager.Instance.NextStage();
        nextStageBtn.gameObject.SetActive(false);
        nextStageText.text = "NextStage";
        nextStageBtn.onClick.RemoveAllListeners();
    }

    private void OnStageEnd()
    {
        nextStageBtn.gameObject.SetActive(true);
        nextStageBtn.onClick.AddListener(OnNextStageButtonClicked);
        nextStageText.text = "NextStage";
    }

    // ============ 골드 변경 이벤트

    private void UpdateGoldText(int newGoldAmount)
    {
        goldText.text = newGoldAmount.ToString();
    }
}
