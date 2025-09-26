using UnityEngine;

public class BarrackUnit : CharacterStats
{
    public Vector3 unitPosition { get; set; }
    private Vector3 unitRotation;

    protected override void Awake()
    {
        unitRotation = transform.rotation.eulerAngles;
    }

    private void OnEnable()
    {
        //이 부분에 초기화 넣어주면됨
        transform.position = unitPosition;
        transform.rotation = Quaternion.Euler(unitRotation);
    }

    protected override void Death()
    {
        //사망시
        base.Death();
        BarrackUnitSpawn.Instance.nowUnit--;
    }

}
