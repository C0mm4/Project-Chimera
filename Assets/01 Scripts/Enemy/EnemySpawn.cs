using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : Singleton<EnemySpawn>
{

    [SerializeField] private EnemySO enemyDataSO;
    [SerializeField] private Transform enemyTransform;

    private BoxCollider[] boxColliders;

    private Dictionary<int, List<MonsterSpawnInfo>> stageData = new Dictionary<int, List<MonsterSpawnInfo>>();

    //오브젝트의 크기값 비교해서 넣으면됨
    public float outRangeValue = 0.5f;

    private void Awake()
    {
        boxColliders = transform.GetComponentsInChildren<BoxCollider>();

        //스테이지 생성
        /*stageData[1] = new List<MonsterSpawnInfo>
        {
            new MonsterSpawnInfo("enemy", "enemy", 5, 0, 3, 1f),
            new MonsterSpawnInfo("enemy2", "enemy2", 4, 1, 2, 1.5f)
        };

        stageData[2] = new List<MonsterSpawnInfo>
        {
            new MonsterSpawnInfo("enemy", "enemy", 6, 0, 3, 2f),
            new MonsterSpawnInfo("enemy2", "enemy2", 8, 0, 3, 5f)
        };*/

        // ScriptableObject 데이터 불러오기
        foreach (StageWave wave in enemyDataSO.stageWaves)
        {
            if (!stageData.ContainsKey(wave.stageNumber))
            {
                stageData[wave.stageNumber] = new List<MonsterSpawnInfo>();
            }

            stageData[wave.stageNumber].AddRange(wave.monsters);
        }
    }

    private void Start()
    {
        //스테이지 불러오는 부분은 다른 스크립트에서 불러와도됨
        StartStage(2);
    }

    public void SpawnMonster(string name,int enemycount,int spawnpoint,int spawncount = 1,float time = 1f)
    {
        if (enemycount <= 0 || spawnpoint < 0 || spawnpoint >= boxColliders.Length ) return;

        StartCoroutine(SpawnMonsterCoroutine(name, enemycount, spawnpoint, spawncount,time));
            
    }

    private IEnumerator SpawnMonsterCoroutine(string name, int enemycount, int spawnpoint, int spawncount, float time = 1f)
    {
        BoxCollider box = boxColliders[spawnpoint];

        for (int wave = 0; wave < spawncount; wave++)
        {
            //랜덤으로 뿌려진 위치 기록용
            List<Vector3> usedPositions = new List<Vector3>();

            for (int i = 0; i < enemycount; i++)
            {
                //오브젝트 풀에서 name키값의 오브젝트 가져옮
                //가져오면서 active ture로 변하니 따로 설정할필요 X
                GameObject enemy = ObjectPoolManager.Instance.GetPool(name);
                enemy.name = name;

                if (enemy != null)
                {
                    Vector3 spawnPos = SpawnOutRange(box, usedPositions, outRangeValue);
                    enemy.transform.position = spawnPos;
                }
            }

            yield return new WaitForSeconds(time); 
        }
    }

    private Vector3 SpawnOutRange(BoxCollider boxcoll,List<Vector3> position,float distanceMin)
    {
        //위치 안겹치는 시도 횟수
        //지금 구조로는 생성된거만 확인해서 변경 필요
        int tryValue = 30;

        //Vector3 newPosition = Vector3.zero;

        float randX = Random.Range(boxcoll.bounds.min.x, boxcoll.bounds.max.x);
        float randZ = Random.Range(boxcoll.bounds.min.z, boxcoll.bounds.max.z);

        Vector3 newPositions = new Vector3(randX, boxcoll.transform.position.y, randZ);
                       
        while (tryValue --> 0)
        {
            bool nearCheck = false;

            foreach (Vector3 pos in position)
            {
                float distanceAll = Vector3.Distance(pos, newPositions);
                float distanceZ = Mathf.Abs(pos.z - newPositions.z);

                //겹치는지 확인
                if (distanceAll < distanceMin || distanceZ < distanceMin)
                {
                    nearCheck = true;
                    break;
                }
            }

            if (!nearCheck)
            {
                position.Add(newPositions);
                return newPositions;
            }

            // 겹쳤으니 다시 새 위치 생성
            randX = Random.Range(boxcoll.bounds.min.x, boxcoll.bounds.max.x);
            randZ = Random.Range(boxcoll.bounds.min.z, boxcoll.bounds.max.z);
            newPositions = new Vector3(randX, boxcoll.transform.position.y, randZ);
        }

        // 횟수내에 겹치는곳 못찾으면 이전꺼
        return position.Count > 0 ? position[position.Count - 1] : boxcoll.bounds.center;
    }

    //스테이지 번호 찾아서 해당 몬스터 스테이지 생성
    public void StartStage(int stageNumber)
    {
        if (!stageData.ContainsKey(stageNumber))
        {
            Debug.LogWarning($"Stage {stageNumber} not found!");
            return;
        }

        foreach (var spawnInfo in stageData[stageNumber])
        {
            // 풀 생성
            ObjectPoolManager.Instance.CreatePool(spawnInfo.keyName, spawnInfo.prefabName, 1, enemyTransform);

            // 몬스터 스폰
            SpawnMonster(
                spawnInfo.keyName,
                spawnInfo.enemyCount,
                spawnInfo.spawnPoint,
                spawnInfo.spawnCount,
                spawnInfo.spawnTime
            );
        }
    }
}
