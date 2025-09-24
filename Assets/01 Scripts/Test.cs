using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    void Start()
    {
        ObjectPoolManager.Instance.CreatePool("GoldMining", "GoldMining", 1);
        ObjectPoolManager.Instance.CreatePool("Tower", "Tower", 1);
        ObjectPoolManager.Instance.CreatePool("Wall", "Wall", 1);
        ObjectPoolManager.Instance.CreatePool("Barrack", "Barrack", 1);
    }

}
