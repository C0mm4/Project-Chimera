using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 임시 공격 호출 스크립트
/// </summary>
public class PlayerAttack : MonoBehaviour
{
    // 인스펙터 창에서 현재 장착 중인 무기를 연결해줄 변수
    [SerializeField] private Weapon currentWeapon;

    private void Update()
    {
        // 무기가 연결되어 있는지 확인하고, 연결되어 있다면 공격 명령
        if (currentWeapon != null)
        {
            currentWeapon.Attack();
        }
    }

    public void SetWeapon(Weapon newWeapon)
    {
        currentWeapon = newWeapon;
    }

}
