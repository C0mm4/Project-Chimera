using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : StructureBase
{
    [SerializeField] private Weapon currentWeapon;
    private TowerSO data;

    public override void SetDataSO(BaseStatusSO statData)
    {
        // 기존 정보 파괴
        DestroyEffect();

        // 새로운 정보 설정
        base.SetDataSO(statData);
        BuildEffect();
    }

    public void SetWeaponData(WeaponSO weapon)
    {
        if (currentWeapon == null)
            currentWeapon = GetComponentInChildren<Weapon>();
        currentWeapon.SetWeapon(weapon);
    }

    protected override void BuildEffect()
    {
        base.BuildEffect();
        data = statData as TowerSO;
        data.weaponData = DataManager.Instance.GetSOData<WeaponSO>(data.weaponDataID);
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
