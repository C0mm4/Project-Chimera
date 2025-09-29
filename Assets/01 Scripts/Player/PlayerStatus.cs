using UnityEngine;

public class PlayerStatus : CharacterStats
{
    [SerializeField] private PlayerStatusSO statusData;
    [SerializeField] private GaugeBarUI gaugebarUI;

    private void OnEnable()
    {
        GameManager.Instance.Player = this;
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

}
