using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ConstructureSeed : MonoBehaviour
{
    private enum ConstructureType
    {
        GoldMining, Tower, Wall, Barrack
    }

    [SerializeField] private ConstructureType type;

    [SerializeField] InteractionZone interactionZone;
    private void Awake()
    {

        if (interactionZone != null)
        {
            interactionZone.OnInteract += Build;
        }
    }

    private void Build()
    {
        string keyName = "";
        BaseStatusSO so = null;
        switch (type)
        {
            case ConstructureType.GoldMining:
                keyName = "GoldMining";
                so = DataManager.Instance.GetSOData<GoldMiningSO>(310000);
                break;

            case ConstructureType.Tower:
                keyName = "Tower";
                so = DataManager.Instance.GetSOData<TowerSO>(320000);
                break;

            case ConstructureType.Wall:
                keyName = "Wall";
                so = DataManager.Instance.GetSOData<WallSO>(340000);
                break;

            case ConstructureType.Barrack:
                keyName = "Barrack";
                so = DataManager.Instance.GetSOData<BarrackSO>(310000);
                break;
        }

        if(keyName != "" && so != null)
        {
            var obj = ObjectPoolManager.Instance.GetPool(keyName);
            obj.transform.position = transform.position;
            obj.transform.rotation = transform.rotation;

            // SO Data로드 후 주입
            obj.GetComponent<StructureBase>().SetDataSO(so);
            ObjectPoolManager.Instance.ResivePool("", gameObject);
        }
    }
}
