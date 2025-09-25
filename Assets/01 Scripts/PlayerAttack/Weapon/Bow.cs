using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : BaseWeapon
{
    private RangedWeaponSO RangedData => weaponData as RangedWeaponSO;
    protected void Awake()
    {
        if (RangedData.ProjectilePrefab != null)
        {
            ObjectPoolManager.Instance.CreatePool(RangedData.ProjectilePrefab.name, RangedData.ProjectilePrefab.name, 10);
        }
    }

    protected override void PerformAttack(Transform target)
    {
        GameObject projectileObj = ObjectPoolManager.Instance.GetPool(RangedData.ProjectilePrefab.name);

        Projectile projectile = projectileObj.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.Initialize(transform, target, RangedData, InstigatorTrans);
        }
    }
}
