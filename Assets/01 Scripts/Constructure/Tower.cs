using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : StructureBase
{
    [SerializeField] private BaseWeapon currentWeapon;
    [SerializeField] private EnemyScanner scanner;

    [SerializeField] private TowerData data;

    public override void CopyStatusData(BaseStatusSO statData)
    {
        TowerSO so = statData as TowerSO;
        
        data.weaponData = DataManager.Instance.GetSOData<BaseWeaponSO>(so.weaponDataID);
        SetWeaponData(data.weaponData);
    }

    public override void SetDataSO(StructureSO statData)
    {
        // 기존 정보 파괴
        DestroyEffect();

        // 새로운 정보 설정
        base.SetDataSO(statData);
        BuildEffect();
    }

    public void SetWeaponData(BaseWeaponSO weapon)
    {
        if (currentWeapon == null)
            currentWeapon = GetComponentInChildren<BaseWeapon>();
        currentWeapon.SetWeapon(weapon, transform);

        if(scanner == null)
            scanner = GetComponentInChildren<EnemyScanner>();

        scanner.scanRange = currentWeapon.GetWeaponData().ScanRange;
        scanner.detectCollider.radius = scanner.scanRange;
    }

    protected override void BuildEffect()
    {
        base.BuildEffect();

    }

    protected override void DestroyEffect()
    {
        base.DestroyEffect();
    }

    protected override void UpdateEffect()
    {
        base.UpdateEffect();
        {
            if (scanner != null && scanner.nearestTarget != null)
            {
                if (currentWeapon != null)
                {
                    currentWeapon.Attack(scanner.nearestTarget);
                }
            }
        }
    }
}


[Serializable]
public struct TowerData
{
    public BaseWeaponSO weaponData;
}