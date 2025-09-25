using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : StructureBase
{
    [SerializeField] private Weapon currentWeapon;
    private TowerSO data;

    public override void SetDataSO(BaseStatusSO statData)
    {
        base.SetDataSO(statData);
        data = statData as TowerSO;

        SetWeaponData(data.weaponData);
    }

    public void SetWeaponData(WeaponSO weapon)
    {
        currentWeapon.SetWeapon(weapon);
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
