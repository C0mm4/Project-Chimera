using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldMining : StructureBase
{
    private GoldMiningSO data;

    public override void SetDataSO(StructureSO statData)
    {
        DestroyEffect();

        base.SetDataSO(statData);
        
        data = statData as GoldMiningSO;
        BuildEffect();
    }

    protected override void BuildEffect()
    {
        base.BuildEffect();
        
        // 골드 획득량 증가 처리
    }

    protected override void DestroyEffect()
    {
        base.DestroyEffect();

        // 골드 획득량 감소 처리
    }
}
