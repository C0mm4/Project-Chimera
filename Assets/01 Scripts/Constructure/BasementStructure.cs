using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasementStructure : StructureBase
{
    public override void CopyStatusData(BaseStatusSO statData)
    {

    }

    private void OnEnable()
    {
        StageManager.Instance.Basement = this;

        ObjectPoolManager.Instance.CreatePool("GoldMining", null,1);
        ObjectPoolManager.Instance.CreatePool("Tower", null,1);
        ObjectPoolManager.Instance.CreatePool("Wall", null,1);
        ObjectPoolManager.Instance.CreatePool("Barrack", null,1);
    }

    public override void ConfirmUpgrade()
    {
        base.ConfirmUpgrade(); // 기본 업그레이드 로직 실행
        PlayerBaseManager.Instance.OnBaseLevelUp(); // 중앙 관리자에게 레벨업 알림
    }

    protected override void OnReturnToPool()
    {
        base.OnReturnToPool();
        // Todo: 베이스 레벨업을 해서 기존 건물을 풀에 넣기 전에 할 것
    }
    public override void UpgradeApplyConcreteStructure()
    {
    }
}


