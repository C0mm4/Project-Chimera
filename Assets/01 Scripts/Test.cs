using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    void Start()
    {
        ObjectPoolManager.Instance.CreatePool("GoldMining", 1);
        ObjectPoolManager.Instance.CreatePool("Tower", 1);
        ObjectPoolManager.Instance.CreatePool("Wall", 1);
        ObjectPoolManager.Instance.CreatePool("Barrack", 1);
    }

}
