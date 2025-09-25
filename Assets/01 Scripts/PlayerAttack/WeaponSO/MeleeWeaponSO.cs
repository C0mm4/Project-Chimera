using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "WeaponSO/Sword")]
public class MeleeWeaponSO : BaseWeaponSO
{
    [Header("Melee Weapon Data")]
    public float AttackRadius; // 공격 판정 범위
    public float AttackArcAngle; // 공격 부채꼴 각도  
    public float hitboxActiveDuration = 0.3f; // 히트박스를 활성화할 시간
}
