using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public Transform StructureTrans;
    public Transform StructureSeedTrans;
    public Transform ObjDropTrans;
    private void Awake()
    {
        StageManager.Instance.Stage = this;

        ObjectPoolManager.Instance.CreatePool("GoldMining", StructureTrans);
        ObjectPoolManager.Instance.CreatePool("Tower", StructureTrans);
        ObjectPoolManager.Instance.CreatePool("Wall", StructureTrans);
        ObjectPoolManager.Instance.CreatePool("Barrack", StructureTrans);
    }
}
