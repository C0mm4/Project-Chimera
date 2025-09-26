using UnityEngine;

public class BaseWeaponSO : ScriptableObject
{
    [Header("Base Data")]
    public string Name;
    // 무기 설명
    // 아이콘 등 추가 가능

    [Header("Weapon Data")]
    public float Damage;
    public float ScanRange;         // 공격 범위(반지름)
    public float AttackCooldown;    // 공격 주기
}
