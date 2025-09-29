using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldMining : StructureBase
{
    //   private GoldMiningSO data;
    [SerializeField] private GoldMiningData data;

    public override void CopyStatusData(BaseStatusSO statData)
    {
        GoldMiningSO so = statData as GoldMiningSO;
        data.AddGoldDropRate = so.AddGoldDropRate;
        data.AddGoldGetRate = so.AddGoldGetRate;
    }

    public override void SetDataSO(StructureSO statData)
    {
        DestroyEffect();

        base.SetDataSO(statData);
        
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
    public override void UpgradeApplyConcreteStructure()
    {
    }
}

[Serializable]
public struct GoldMiningData
{
    public float AddGoldDropRate;
    public float AddGoldGetRate;
}