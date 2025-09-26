using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : BaseWeapon
{
    private RangedWeaponSO RangedData => weaponData as RangedWeaponSO;
    protected void Start()
    {
        if (RangedData.ProjectileID != null)
        {
            ObjectPoolManager.Instance.CreatePool(RangedData.ProjectileID,transform, 10);
        }
    }

    protected override void PerformAttack(Transform target)
    {
        GameObject projectileObj = ObjectPoolManager.Instance.GetPool(RangedData.ProjectileID);

        Projectile projectile = projectileObj.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.Initialize(transform, target, RangedData, InstigatorTrans);
        }
    }
}
