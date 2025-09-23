using UnityEngine;

[CreateAssetMenu(fileName ="New Weapon Data", menuName ="WeaponSO")]
public class WeaponSO : ScriptableObject
{
    [Header("Base Data")]
    public int Id;
    public string Name;
    // 무기 설명
    // 아이콘 등 추가 가능

    [Header("Weapon Data")]
    public float Damage;
    public float ScanRange;         // 공격 범위(반지름)
    public float AttackCooldown;    // 공격 주기

    [Header("Projectile Data")]
    public GameObject ProjectilePrefab;
    public int ProjectileCount = 1;         // 발사체 개수
    public float ProjectileSpeed = 3f;      // 발사체 속도
    public float ProjectileArcHeight = 3f;  // 발사체 포물선 높이
}
