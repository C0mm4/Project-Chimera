using UnityEngine;

public class Bow : BaseWeapon
{
    private RangedWeaponSO RangedData => weaponData as RangedWeaponSO;
    protected void Awake()
    {
        if (RangedData.ProjectilID != null)
        {
            ObjectPoolManager.Instance.CreatePool(RangedData.ProjectilID, transform, 10 );
        }
    }

    protected override void PerformAttack(Transform target)
    {
        Debug.Log("performattack : " + transform);
        GameObject projectileObj = ObjectPoolManager.Instance.GetPool(RangedData.ProjectilID,transform);

        Projectile projectile = projectileObj.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.Initialize(transform, target, RangedData, InstigatorTrans);
        }
    }
}
