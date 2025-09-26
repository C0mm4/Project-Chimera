using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrack : StructureBase
{
    // 여기 내용은 SO화 해서 카드로 변경 예정
    [SerializeField] private int currentSpawnCount;

    private BarrackSO barrackData;

    private float lastSpawnTime;

    private List<GameObject> spawnUnits = new();

    public override void SetDataSO(StructureSO data)
    {
        Debug.Log("SetData");
        base.SetDataSO(data);
        barrackData = statData as BarrackSO;

        if (barrackData == null) return;

        // 기존 소환된 애들 삭제 처리
        Clear();

        // 최초 설정 시 소환 유닛 개수만큼 소환
        currentSpawnCount = 0;
        for (int i = 0; i < barrackData.spawnCount; i++)
        {
            Spawn();
        }
    }

    protected override void BuildEffect()
    {
        base.BuildEffect();
        //ObjectPoolManager.Instance.CreatePool(barrackData.spawnUnitKey,  4, transform);
    }

    protected override void DestroyEffect()
    {
        base.DestroyEffect();
    }

    protected override void UpdateEffect()
    {
        base.UpdateEffect();
        // 소환된 개수가 적으면, 주기적으로 소환
        if (currentSpawnCount < barrackData.spawnCount)
        {
            if (Time.time - lastSpawnTime >= barrackData.spawnRate)
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
        // 소환 시도 시 풀 생성 안되어있으면 삭제
        if (!ObjectPoolManager.Instance.ContainsPool(barrackData.spawnUnitKey))
        {
            ObjectPoolManager.Instance.CreatePool(barrackData.spawnUnitKey, 4, transform);
        }
        
        var obj = ObjectPoolManager.Instance.GetPool(barrackData.spawnUnitKey);
        if (obj != null)
        {
            spawnUnits.Add(obj);
            Vector3 randomPos = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            randomPos = randomPos.normalized;
            obj.transform.position = transform.position + randomPos;
        }
        currentSpawnCount++;
    }

    private void Clear()
    {
        foreach (var obj in spawnUnits)
        {
            ObjectPoolManager.Instance.ResivePool(barrackData.spawnUnitKey, obj);
        }

        spawnUnits.Clear();
    }
}
