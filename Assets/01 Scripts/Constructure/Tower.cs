using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : StructureBase
{
    [SerializeField] private BaseWeapon currentWeapon;
    private TowerSO data;

    public override void SetDataSO(BaseStatusSO statData)
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
    }

    protected override void BuildEffect()
    {
        base.BuildEffect();
        data = statData as TowerSO;
        data.weaponData = DataManager.Instance.GetSOData<BaseWeaponSO>(data.weaponDataID);
        SetWeaponData(data.weaponData);

    }

    protected override void DestroyEffect()
    {
        base.DestroyEffect();
        data = null;
        statData = null;
    }

    protected override void UpdateEffect()
    {
        base.UpdateEffect();
        {
            if (currentWeapon != null)
            {
                currentWeapon.Attack();
            }
        }
    }
}
