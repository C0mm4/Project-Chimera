using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class EnemySpawn : Singleton<EnemySpawn>
{

    [SerializeField] private List<StageWaveSO> stageDataSO;
    [SerializeField] private Transform enemyTransform;

    private BoxCollider[] boxColliders;

    private Dictionary<string, List<MonsterSpawnInfo>> stageData = new Dictionary<string, List<MonsterSpawnInfo>>();

    //오브젝트의 크기값 비교해서 넣으면됨
    public float outRangeValue = 0.5f;

    private void Awake()
    {
        boxColliders = transform.GetComponentsInChildren<BoxCollider>();
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

            foreach (Transform child in enemyTransform)
            {
                usedPositions.Add(child.position);
            }

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

        float randX = Random.Range(boxcoll.bounds.min.x, boxcoll.bounds.max.x);
        float randZ = Random.Range(boxcoll.bounds.min.z, boxcoll.bounds.max.z);

        Vector3 newPositions = new Vector3(randX, boxcoll.transform.position.y, randZ);
                       
        while (tryValue --> 0)
        {
            bool nearCheck = false;

            //위치 중복 체크
            foreach (Vector3 pos in position)
            {
                if (Vector3.Distance(pos, newPositions) < distanceMin)
                {
                    nearCheck = true;
                    break;
                }
            }

            if (nearCheck)
            {
                randX = Random.Range(boxcoll.bounds.min.x, boxcoll.bounds.max.x);
                randZ = Random.Range(boxcoll.bounds.min.z, boxcoll.bounds.max.z);
                newPositions = new Vector3(randX, boxcoll.transform.position.y, randZ);
                continue;
            }

            //새로 생성된거랑 위치 다시 확인
            foreach (Vector3 occupiedPos in position)
            {
                if (Vector3.Distance(occupiedPos, newPositions) < distanceMin)
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
        string key = $"stage{stageNumber:D2}";

        if (stageData.ContainsKey(key))
        {
            //키가 있으면 스테이지를 가져오지 않고 바로 몬스터 생성
            Debug.LogWarning($"Stage {stageNumber} 가 이미 있음");
        }
        else
        {
            // SO 데이터 가져와서 넣기
            var handle = ResourceManager.Instance.Load<StageWaveSO>(key);

            StageWaveSO waveSO = handle;
            stageData[key] = waveSO.monsters;
        }

        //몬스터 스폰부분
        foreach (var spawnInfo in stageData[key])
        {
            // 풀 생성
            ObjectPoolManager.Instance.CreatePool(spawnInfo.keyName, enemyTransform);

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
