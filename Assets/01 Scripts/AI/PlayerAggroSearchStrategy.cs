using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAggroSearchStrategy : ISearchStrategy
{
    public Transform owner;

    public float PlayerSearchRange;
    public float StructureSearchRange;

    public LayerMask PlayerLayerMask;
    public LayerMask StructureLayerMask;

    Collider[] overlaps = new Collider[10];

    public Transform SearchTarget()
    {
        if (Physics.CheckSphere(owner.position, PlayerSearchRange, PlayerLayerMask))
        {
            return GameManager.Instance.Player.transform;
        }
       
        int count = Physics.OverlapSphereNonAlloc(owner.position, StructureSearchRange, overlaps, StructureLayerMask);

        if (count < 1) return StageManager.Instance.Basement.transform;

        float minDist = 12345;
        int targetIdx = -1;

        for (int i = 0; i < count; ++i)
        {
            float dist = Vector3.Distance(owner.position, overlaps[i].transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                targetIdx = i;
            }
        }

        return overlaps[targetIdx].transform;
    }

}
