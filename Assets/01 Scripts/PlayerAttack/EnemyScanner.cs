using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 주변 적을 탐색하는 기능
/// </summary>
public class EnemyScanner : MonoBehaviour
{
    public float scanRange;         // 무기에서 받아올 탐지 범위(원의 반지름)
    public LayerMask targetLayer;   // 타겟 레이어
    public Collider[] targets;    // 탐지 범위에 들어온 타겟
    public Transform nearestTarget; // 가장 가까운 적

    private void FixedUpdate()
    {
        targets = Physics.OverlapSphere(transform.position, scanRange, targetLayer);
        nearestTarget = GetNearestEnemy();
    }

    // 가장 가까운 적을 찾는 메서드
    private Transform GetNearestEnemy()
    {
        if (targets.Length == 0) return null; // 범위 내에 적이 없으면 return

        Transform result = null;
        float minDistance = Mathf.Infinity; // 처음에 가장 가까운 적을 찾기 위해

        foreach(Collider target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 enemyPos = target.transform.position;

            float curDistance = Vector3.SqrMagnitude(myPos - enemyPos); // 루트 연산 X

            if(curDistance < minDistance)
            {
                minDistance = curDistance;
                result = target.transform;
            }
        }

        return result;
    }

    // 에디터에서 탐지 범위를 시각적으로 확인하기 위한 메서드
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, scanRange);
    }
}
