using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasementStructure : StructureBase
{
    private void Awake()
    {
        if (statData != null)
        {
            // 원본 SO를 기반으로 '복제본'을 만들고,
            // SetDataSO를 호출하여 이 건물의 데이터로 사용합니다.
            SetDataSO(Instantiate(statData));
        }
        else
        {
            Debug.LogError("BasementStructure에 원본 StatData가 연결되지 않았습니다!", this);
        }
    }

    public override void CopyStatusData(BaseStatusSO statData)
    {

    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StageManager.Instance.Basement = this;

        ObjectPoolManager.Instance.CreatePool("GoldMining", null, 1);
        ObjectPoolManager.Instance.CreatePool("Tower", null, 1);
        ObjectPoolManager.Instance.CreatePool("Wall", null, 1);
        ObjectPoolManager.Instance.CreatePool("Barrack", null, 1);
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

    protected override void TryStartUpgrade()
    {
        // 최고 레벨 확인 로직은 베이스에도 필요하므로 그대로 둡니다.
        if (structureData.CurrentLevel >= statData.GetMaxLevel())
        {
            Debug.Log("최고 레벨입니다.");
            return;
        }

        // "베이스 레벨이 부족한지" 확인하는 로직을 "생략"합니다.
        //    (베이스는 자신을 기준으로 삼으므로, 이 검사가 필요 없습니다.)

        // 골드 확인 로직은 그대로 실행합니다.
        int requiredCost = statData.levelProgressionData[structureData.CurrentLevel - 1].upgradeCost;
        // if (GameManager.Instance.Gold < requiredCost) return;

        // 모든 조건 통과 시 업그레이드를 실행합니다.
        ConfirmUpgrade();
    }
}


