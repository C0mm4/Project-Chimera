using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : Singleton<EnemySpawn>
{
    [SerializeField] private Transform enemyTransform;

    private BoxCollider[] boxColliders;

    //오브젝트의 크기값 비교해서 넣으면됨
    public float outRangeValue = 0.5f;

    private void Awake()
    {
        boxColliders = transform.GetComponentsInChildren<BoxCollider>();
    }

    private void Start()
    {
        //테스트
        /*
        ObjectPoolManager.Instance.CreatePool("enemy", "enemy", 1, enemyTransform);
        ObjectPoolManager.Instance.CreatePool("enemy2", "enemy2", 1, enemyTransform);

        SpawnMonster("enemy", 5, 0, 3);
        SpawnMonster("enemy2", 4, 0, 3,1.3f);
        */
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
        int tryValue = 30;

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
}
