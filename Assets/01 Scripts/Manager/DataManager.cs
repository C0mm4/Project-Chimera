using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private Dictionary<int, ScriptableObject> SODataDict = new();

    public T GetSOData<T>(int id) where T : ScriptableObject
    {
        // 저장된 키 값이 있으면, 제너릭 확인 후 반환
        if(SODataDict.ContainsKey(id))
        {
            if (SODataDict[id] is T)
            {
                return SODataDict[id] as T;
            }
            else
            {
                Debug.LogError($"{id} instance is not {typeof(T).Name} Data");
                return null;
            }
        }
        // 데이터 로드
        var data = ResourceManager.Instance.Load<T>(id.ToString());
        // 제너릭 확인 후 저장 및 반환
        if(data is T)
        {
            SODataDict[id] = data as T;
            return data as T;
        }
        else
        {
            Debug.LogError($"{id} instance is not {typeof(T).Name} Data");
            return null;
        }
    }
}
