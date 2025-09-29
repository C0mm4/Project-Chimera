using UnityEngine;

public class PlayerStatus : CharacterStats
{
    [SerializeField] private PlayerStatusSO statusData;
    [SerializeField] private GaugeBarUI gaugebarUI;

    private void OnEnable()
    {
        GameManager.Instance.Player = this;
        StageManager.Instance.OnStageFail += OnFail;
    }

    private void OnFail()
    {
        Debug.Log("스테이지 실패했을 때 플레이어 처리");
    }

    protected override void Awake()
    {
        base.Awake();

    }

    public void Update()
    {
        float percent = data.currentHealth / data.maxHealth;
        gaugebarUI.SetFillPercent(percent);
    }

    protected override void Death()
    {
        base.Death();

        StageManager.Instance.FailStage();
    }
}
