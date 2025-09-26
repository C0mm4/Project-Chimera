using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureSO : BaseStatusSO
{
    [Header("건물 업그레이드 정보")]
    public int upgradeCost;
    public string nextLevelPrefabKey;

    public StructureSO nextLevelSO;
}
