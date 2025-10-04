using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class Barrack : StructureBase
{
    [SerializeField] private int currentSpawnCount;

    [SerializeField] private BarrackData barrackData;

//    private float lastSpawnTime;
    private float spawnWaitTime;

    private List<GameObject> spawnUnits = new();
    private bool[] activateSpawnIndex;

    //유닛 위치 저장
    [SerializeField] private List<Vector3> savePosition = new List<Vector3>();

    

    public override void SetDataSO(StructureSO data)
    {
        Debug.Log("SetData");
        // 데이터 변경 전 풀링 삭제
        ObjectPoolManager.Instance.ClearPool(barrackData.spawnUnitKey, transform);

        /*
        for (int i = 0; i < spawnUnits.Count; i++)
        {
            BarrackUnitStatus unit = spawnUnits[i].GetComponent<BarrackUnitStatus>();
            spawnUnits[i].transform.position = unit.unitPosition.position;
        }*/

        base.SetDataSO(data);
        // 기존 소환된 애들 삭제 처리
        Clear();
        activateSpawnIndex = new bool[barrackData.spawnCount];

        BuildEffect();
    }

    public override void CopyStatusData(BaseStatusSO statData)
    {
        BarrackSO so = statData as BarrackSO;
        barrackData.spawnRate = so.spawnRate;
        barrackData.spawnUnitKey = so.spawnUnitKey;
        barrackData.spawnCount = so.spawnCount;
    }

    protected override void BuildEffect()
    {
        base.BuildEffect();
        SavePositions();
        // 최초 설정 시 소환 유닛 개수만큼 소환
        currentSpawnCount = 0;
        for (int i = 0; i < barrackData.spawnCount; i++)
        {
            Spawn(i);
        }
        //ObjectPoolManager.Instance.CreatePool(barrackData.spawnUnitKey,  4, transform);
    }

    protected override void DestroyEffect()
    {
        base.DestroyEffect();

        Sequence sequence = DOTween.Sequence();

        sequence.Join(transform.DOMoveY(transform.position.y - 6f, 1f).SetEase(Ease.InOutQuad));

        sequence.Join(transform.DOShakePosition(
            duration: 1f,
            strength: new Vector3(0.5f, 0f, 0f),
            vibrato: 10,
            randomness: 90f,
            fadeOut: true
            ));

    }

    protected override void UpdateEffect()
    {
        base.UpdateEffect();
        // 소환된 개수가 적으면, 주기적으로 소환
        if (currentSpawnCount < barrackData.spawnCount)
        {
            spawnWaitTime += Time.deltaTime;
            if(spawnWaitTime > barrackData.spawnRate)
            {

                int spawnIndex = -1;
                for (int i = 0; i <= barrackData.spawnCount; i++)
                {
                    if (!activateSpawnIndex[i])
                    {
                        spawnIndex = i;
                        break;
                    }
                }

                if (spawnIndex != -1)
                {
                    Spawn(spawnIndex);
                    spawnWaitTime = 0;
                }

            }
            /*
            if (Time.time - lastSpawnTime >= barrackData.spawnRate)
            {
                int spawnIndex = -1;
                for(int i = 0; i <= barrackData.spawnCount; i++)
                {
                    if (!activateSpawnIndex[i])
                    {
                        spawnIndex = i;
                        break;
                    }
                }

                if(spawnIndex != -1)
                {
                    Spawn(spawnIndex);
                    lastSpawnTime = Time.time;
                }

            }
            */
        }
    }

    private void Spawn(int index)
    {
        // 소환 시도 시 풀 생성 안되어있으면 삭제
        if (!ObjectPoolManager.Instance.ContainsPool(barrackData.spawnUnitKey,transform))
        {
            ObjectPoolManager.Instance.CreatePool(barrackData.spawnUnitKey, transform);
        }
        GameObject obj = ObjectPoolManager.Instance.GetPool(barrackData.spawnUnitKey, transform);
        if (obj != null)
        {
            spawnUnits.Add(obj);
            BarrackUnitStatus unit = obj.GetComponent<BarrackUnitStatus>();
            NavMeshAgent navmesh = obj.GetComponent<NavMeshAgent>();
            navmesh.Warp(savePosition[index]);
            unit.transform.position = savePosition[index];
            unit.spawnIndex = index;
            unit.spawnBarrack = this;
            activateSpawnIndex[index] = true;
            currentSpawnCount++;
        }
    }

    private void Clear()
    {
        foreach (var obj in spawnUnits)
        {
            ObjectPoolManager.Instance.ResivePool(barrackData.spawnUnitKey, obj);
        }

        spawnUnits.Clear();
    }

    public override void UpgradeApplyConcreteStructure()
    {
    }

    public void UnitDespawn(BarrackUnitStatus unit)
    {
        currentSpawnCount--;
        activateSpawnIndex[unit.spawnIndex] = false;
        ObjectPoolManager.Instance.ResivePool(unit.gameObject.name, unit.gameObject, transform);
    }

    protected override void Revive()
    {
        base.Revive();
        for (int i = 0; i < spawnUnits.Count; i++)
        {
            var unit = spawnUnits[i].GetComponent<BarrackUnitStatus>();
            var AI = spawnUnits[i].GetComponent<AIControllerBase>();
            AI.agent.Warp(savePosition[unit.spawnIndex]);
            AI.SetTargetNull();
            unit.OnStageEnd();
        }

    }
    private void SavePositions()
    {
        //유닛 생성
        //한줄 유닛 생성
        int unitRow = 4;
        //유닛 생성 간격
        float unitInterval = 2f;
        //유닛 줄간격
        float unitIntervalRow = 2f;

        //유닛 생성 위치
        float spawnRangeX = -3f;
        float spawnRangeY = -2f;

        //일단 위치 100까진 저장
        for (int i = 0; i < 100; i++)
        {
            //생성위치는 나중에 배럭 크기라던지 고려해서 다시 설정해야함
            savePosition.Add(transform.position + new Vector3(spawnRangeX + (i % unitRow) * unitInterval, 0, spawnRangeY + unitIntervalRow * -(i / unitRow)));
        }

    }
}

[Serializable]
public struct BarrackData
{
    public string spawnUnitKey;
    public int spawnCount;
    public float spawnRate;
}