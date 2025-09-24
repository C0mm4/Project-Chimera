using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructureSeed : MonoBehaviour
{
    private enum ConstructureType
    {
        GoldMining, Tower, Wall, Barrack
    }

    [SerializeField] private ConstructureType type;

    private void Build()
    {
        string keyName = "";
        switch (type)
        {
            case ConstructureType.GoldMining:
                keyName = "GoldMining";
                break;

            case ConstructureType.Tower:
                keyName = "Tower";
                break;

            case ConstructureType.Wall:
                keyName = "Wall";
                break;

            case ConstructureType.Barrack:
                keyName = "Barrack";
                break;
        }

        if(keyName != "")
        {
            var obj = ObjectPoolManager.Instance.GetPool(keyName);
            obj.transform.position = transform.position;
            obj.transform.rotation = transform.rotation;
            ObjectPoolManager.Instance.ResivePool("", gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Build();
        }
    }
}
