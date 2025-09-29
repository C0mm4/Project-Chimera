using UnityEngine;

public class BarrackUnitStatus : CharacterStats
{
    public Transform unitPosition;

    public Barrack spawnBarrack;
    public int spawnIndex;

/*    protected override void Awake()
    {
        unitPosition = transform;
        //테스트용
//        unitPosition.position = new Vector3(-5.5f, 0.48f, -7.63f);

    }*/
    

    private void OnEnable()
    {
        //이 부분에 초기화 넣어주면됨
        //        transform.position = unitPosition.position;
        transform.rotation = Quaternion.identity;
    }

    protected override void Death()
    {
        //사망시
        base.Death();
        spawnBarrack.UnitDespawn(this);
//        BarrackUnitSpawn.Instance.nowUnit--;
    }


}
