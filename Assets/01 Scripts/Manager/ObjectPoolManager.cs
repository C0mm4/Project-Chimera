using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    private class SavePool
    {
        public string prefabPath;
        public Transform defaultTransform;
    }

    private Dictionary<(string,Transform), Stack<GameObject>> pool = new();
    private Dictionary<string, SavePool> poolOther = new();

    //풀 생성
    public void CreatePool(string _addressableName, Transform _defaultTransform , int _count = 1)
    {

        var key = (_addressableName, _defaultTransform);

        if (pool.ContainsKey(key))
        {
            //이미 키값으로 생성된게 있음
            return;
        }

        Stack<GameObject> queue = new Stack<GameObject>();

        for (int i = 0; i < _count; i++)
        {
            //로드+생성 후 오브젝트 끄기
            GameObject obj = ResourceManager.Instance.Create<GameObject>(_addressableName, _defaultTransform);
            obj.SetActive(false);
            obj.name = _addressableName;
            
            //queue에 겜오브젝트 추가
            queue.Push(obj);
        }

        pool.Add(key, queue);

        poolOther[_addressableName] = new SavePool
        {
            prefabPath = _addressableName,
            defaultTransform = _defaultTransform
        };

    }

    //풀에서 오브젝트 가져오기
    public GameObject GetPool(string keyValue, Transform _defaultTransform)
    {

        var key = (keyValue, _defaultTransform);

        if (!pool.ContainsKey(key))
        {
            //키값 없음
            return null;
        }

        Stack<GameObject> queue = pool[key];

        if (queue.Count == 0)
        {
            //비어있으면 재생성, 저장된 키값에 정보확인
            if (!poolOther.ContainsKey(keyValue))
            {
                return null;
            }

            //SavePool 클래스에 저장된 부모위치, 프리팹Path불러온거 적용
            SavePool info = poolOther[keyValue];
            GameObject obj = ResourceManager.Instance.Create<GameObject>(info.prefabPath, info.defaultTransform);
            obj.SetActive(false);
            obj.name = keyValue;

            //부모 위치 있는지 확인
            //if (info.defaultTransform != null)
            //    obj.transform.SetParent(info.defaultTransform, false);

            //queue 에 생성한 오브젝트 집어넣기
            queue.Push(obj);
        }

        //오브젝트 활성화 및 반환 (새로 생성했으면 바로 반환)
        GameObject otherObj = queue.Pop();
        otherObj.SetActive(true);
        return otherObj;
    }

    //풀에다가 오브젝트 다시 집어넣기
    public void ResivePool(string keyValue, GameObject obj, Transform _defaultTransform)
    {
        var key = (keyValue, _defaultTransform);

        if (!pool.ContainsKey(key))
        {
            //키값 없음 , 오브젝트 파괴
            Destroy(obj);
            return;
        }

        //오브젝트 비활성화 및 다시 키값에 반납
        obj.SetActive(false);
        obj.transform.SetParent(poolOther[keyValue].defaultTransform, false);
        pool[key].Push(obj);
    }

    //풀 삭제
    public void ClearPool(string keyValue, Transform _defaultTransform)
    {
        var key = (keyValue, _defaultTransform);

        if (!pool.ContainsKey(key))
        {
            //키값 없음 구?지?
            return;
        }

        //오브젝트 삭제
        foreach (GameObject obj in pool[key])
        {
            Destroy(obj);
        }

        //풀안의 키값 삭제
        pool.Remove(key);
    }

    public bool ContainsPool(string keyValue, Transform _defaultTransform) 
    {
        var key = (keyValue, _defaultTransform);

        return pool.ContainsKey(key);
    }
}
