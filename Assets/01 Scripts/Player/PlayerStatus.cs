using UnityEngine;

public class PlayerStatus : CharacterStats
{
    [SerializeField] private PlayerStatusSO statusData;

    private void OnEnable()
    {
        GameManager.Instance.Player = this;
    }

    protected override void Awake()
    {
        base.Awake();

    }


}
