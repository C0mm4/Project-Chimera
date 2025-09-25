using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrack : StructureBase
{
    // 여기 내용은 SO화 해서 카드로 변경 예정
    [SerializeField] private string spawnUnitKey;
    [SerializeField] private int spawnCount;
    [SerializeField] private int currentSpawnCount;
    [SerializeField] private float spawnRate;


    private float lastSpawnTime;

    private List<GameObject> spawnUnits = new();

    public void SetSpawnData(string spawnUnitKey, int spawnCount)
    {
        // 기존 소환된 애들 삭제 처리
        foreach(var obj in spawnUnits)
        {
            ObjectPoolManager.Instance.ResivePool(spawnUnitKey, obj);
        }
        spawnUnits.Clear();

        this.spawnUnitKey = spawnUnitKey;
        this.spawnCount = spawnCount;
        currentSpawnCount = 0;
        for (int i = 0; i < spawnCount; i++)
        {
            Spawn();
        }
    }

    protected override void BuildEffect()
    {
        base.BuildEffect();
        ObjectPoolManager.Instance.CreatePool(spawnUnitKey, spawnUnitKey, 4, transform);
    }

    protected override void DestroyEffect()
    {
        base.DestroyEffect();
        foreach (var obj in spawnUnits)
        {
            ObjectPoolManager.Instance.ResivePool(spawnUnitKey, obj);
        }

        spawnUnits.Clear();
    }

    protected override void UpdateEffect()
    {
        base.UpdateEffect();
        // 소환된 개수가 적으면, 주기적으로 소환
        if (currentSpawnCount < spawnCount)
        {
            if (Time.time - lastSpawnTime >= spawnRate)
            {
                {
                    Spawn();
                    lastSpawnTime = Time.time;
                }
            }
        }
    }

    private void Spawn()
    {
        var obj = ObjectPoolManager.Instance.GetPool(spawnUnitKey);
        if (obj != null)
        {
            spawnUnits.Add(obj);
            Vector3 randomPos = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            randomPos = randomPos.normalized;
            obj.transform.position = transform.position + randomPos;
        }
        currentSpawnCount++;
    }
}
