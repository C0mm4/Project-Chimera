using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureSO : BaseStatusSO
{
    [Header("건물 정보")]
    public int level = 1; // 이 건물의 레벨
    public string prefabKey; // 현재 자신의 프리팹 아이디 데이터 테이블

    [Header("건물 업그레이드 정보")]
    public int upgradeCost;
    public string nextLevelPrefabKey;
    public StructureSO nextLevelSO; // 없다면 최고 레벨 건물인 것
}
