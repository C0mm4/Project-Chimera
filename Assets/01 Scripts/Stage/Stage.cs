using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public Transform StructureTrans;
    public Transform StructureSeedTrans;

    private void Awake()
    {
        StageManager.Instance.Stage = this;
    }
}
