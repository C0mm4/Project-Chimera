using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class ResourceManager : Singleton<ResourceManager>
{
    private Dictionary<string, AsyncOperationHandle> handleDict = new();

    public T Load<T>(string prefName) where T : Object
    {
        var handle = Addressables.LoadAssetAsync<T>(prefName); 
        // 저장 안된 path는 dict에 저장
        if (!handleDict.ContainsKey(prefName))
        {
            handleDict.Add(prefName, handle);
        }
        var ret = handle.WaitForCompletion();
        return ret;
    }

    public void Release(string prefName)
    {
        if (handleDict.ContainsKey(prefName))
        {
            Addressables.Release(handleDict[prefName]);
            handleDict.Remove(prefName);
            Debug.Log($"{prefName} 인스턴스가 Release 되었습니다.");
        }
    }


    public T Create<T>(string path, Transform parent = null) where T : Object
    {
        T res = Load<T>(path);
        if (res == null)
        {
            Debug.Log($"프리팹이 없습니다. : {path}");
            return null;
        }
        
        T obj = Instantiate(res, parent);
        return obj;
    }
    
    public T Create<T>(string prefKey, string path, Transform parent = null) where T : Object
    {
        string key = prefKey + path;
        return Create<T>(key, parent);
    }

    public T CreateCharacter<T>(string prefName, Transform parent = null)  where T : Object
    {
        return Create<T>(Path.Character, prefName, parent);
    }
    
    public T CreateMap<T>(string prefName, Transform parent = null)  where T : Object
    {
        return Create<T>(Path.Map, prefName, parent);
    }
    
    public T CreateUI<T>(string prefName, Transform parent = null)  where T : Object
    {
        return Create<T>(Path.UI, prefName, parent);
    }
}
