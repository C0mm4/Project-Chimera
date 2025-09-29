using System.Collections.Generic;
using UnityEngine;

public class EnemySearchStrategy : ISearchStrategy
{
    public Transform Owner;
    public float SearchRange;
    public LayerMask SearchLayerMask;


    //범위내 콜라이더 찾기
    private CapsuleCollider[] capsuleEnemy = new CapsuleCollider[10];
    private BoxCollider[] boxEnemy = new BoxCollider[10];

    //콜라이더 완전체
    [SerializeField]
    private List<Collider> colliders = new List<Collider>();

    public Transform SearchTarget()
    {
        colliders = new List<Collider>();

        int countCapsule = Physics.OverlapSphereNonAlloc(Owner.position, SearchRange, capsuleEnemy, SearchLayerMask);
        int countBox = Physics.OverlapSphereNonAlloc(Owner.position, SearchRange, boxEnemy, SearchLayerMask);

        //콜라이더 종합
        //계속 CharacterController랑 같이잡아서 화나긴함
        for (int i = 0; i < countCapsule; i++)
        {
            colliders.Add(capsuleEnemy[i]);
            if (capsuleEnemy[i].GetComponent<CharacterController>() != null) i++;
        }
        for (int i = 0; i < countBox; i++)
        {
            colliders.Add(boxEnemy[i]);
            if (boxEnemy[i].GetComponent<CharacterController>() != null) i++;
        }

        if (countCapsule < 1 && countBox < 1) return Owner.transform;

        float minDist = 44444;
        int targetIdx = -1;

        for (int i = 0; i < colliders.Count; ++i)
        {
            float dist = Vector3.Distance(Owner.position, colliders[i].transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                targetIdx = i;
            }
        }

        //가장 가까운 타겟 반환
        return colliders[targetIdx].transform;
    }

}

