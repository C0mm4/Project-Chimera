using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    private float damage;
    // 공격 한 번에 다단히트로 들어가는 것을 막음
    private List<Collider> targetsHit;

    public void StartAttack(float damage)
    {
        this.damage = damage;
        // 공격 시작 시, 맞은 타겟 리스트를 초기화
        targetsHit = new List<Collider>();
    }

}
