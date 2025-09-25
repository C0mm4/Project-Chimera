using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : StructureBase
{
    [SerializeField] private Weapon currentWeapon;

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
