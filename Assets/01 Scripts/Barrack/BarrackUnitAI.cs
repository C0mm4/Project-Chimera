using UnityEngine;

public class BarrackUnitAI : MonoBehaviour
{
    public float SearchRange;
    public LayerMask SearchLayerMask;

    Collider[] overlaps = new Collider[10];
    public Transform SearchTarget()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, SearchRange, overlaps, SearchLayerMask);

        if (count < 1) return StageManager.Instance.Basement.transform;

        float minDist = 44444;
        int targetIdx = -1;

        for (int i = 0; i < count; ++i)
        {
            float dist = Vector3.Distance(transform.position, overlaps[i].transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                targetIdx = i;
            }
        }

        return overlaps[targetIdx].transform;
    }
}
