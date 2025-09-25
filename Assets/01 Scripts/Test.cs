using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    void Start()
    {
        ObjectPoolManager.Instance.CreatePool("GoldMining");
        ObjectPoolManager.Instance.CreatePool("Tower");
        ObjectPoolManager.Instance.CreatePool("Wall");
        ObjectPoolManager.Instance.CreatePool("Barrack");
    }

}
