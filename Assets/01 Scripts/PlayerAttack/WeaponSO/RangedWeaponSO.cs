using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "WeaponSO/Bow")]
public class RangedWeaponSO : BaseWeaponSO
{
    [Header("Projectile Data")]
    public string ProjectileID; // 어드레서블 ID
    public int ProjectileCount = 1;         // 발사체 개수
    public float ProjectileSpeed = 3f;      // 발사체 속도
    public float ProjectileArcHeight = 3f;  // 발사체 포물선
}
