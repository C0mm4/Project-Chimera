using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private Dictionary<string, ScriptableObject> SODataDict = new();

    public T GetSOData<T>(int id) where T : ScriptableObject
    {
        string ID = $"SO_{id}";

        return GetSOData<T>(ID);
    }

    public T GetSOData<T>(string ID) where T : ScriptableObject
    {
        if (!ID.StartsWith("SO_"))
        {
            ID = $"SO_{ID}";
        }
        if (SODataDict.ContainsKey(ID))
        {
            if (SODataDict[ID] is T)
            {
                return SODataDict[ID] as T;
            }
            else
            {
                Debug.LogError($"{ID} instance is not {typeof(T).Name} Data");
                return null;
            }
        }
        // 데이터 로드
        var data = ResourceManager.Instance.Load<T>(ID);
        if (data == null)
        {
            Debug.LogError($"Failed Load {ID} Data");
            return null;
        }

        // 제너릭 확인 후 저장 및 반환
        if (data is T)
        {
            SODataDict[ID] = data as T;
            return data as T;
        }
        else
        {
            Debug.LogError($"{ID} instance is not {typeof(T).Name} Data");
            return null;
        }
    }

    public void ReleaseSOData(int id)
    {
        string ID = $"SO_{id}";
        ReleaseSOData(ID);
    }

    public void ReleaseSOData(string ID)
    {
        if (!ID.StartsWith("SO_"))
        {
            ID = $"SO_{ID}";
        }
        if (SODataDict.ContainsKey(ID))
        {
            ResourceManager.Instance.Release(ID);
            SODataDict.Remove(ID);
        }
    }
}
