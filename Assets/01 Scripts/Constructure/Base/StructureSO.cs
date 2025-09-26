using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public int upgradeCost;
    public string modelAddressableKey;
}

public class StructureSO : BaseStatusSO
{
    [Header("건물 정보")]
    public string prefabKey; // 현재 자신의 프리팹 아이디 데이터 테이블

    [Header("건물 업그레이드 정보")]
    public List<LevelData> levelProgressionData; // 성장 조건(레벨업에 필요한 코스트)
    public StructureUpgradeSO upgradeData; // 성장률 규칙(레벨업 당 늘어나는 스탯)

    public int GetMaxLevel()
    {
        // 생성된 LevelData 만큼 최대 레벨 증가
        return levelProgressionData.Count + 1;
    }
}
