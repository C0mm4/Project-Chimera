using System.Collections.Generic;
using UnityEngine;

public class BarrackUnitSpawn : Singleton<BarrackUnitSpawn> 
{
    //test 오브젝트
    public List<BarrackUnit> testunit = new List<BarrackUnit>();

    //유닛 최대치 수, 현재 유닛 수, 키 값, 프리팹 이름
    private int maxUnit = 0;

    public int nowUnit { get; set; } = 0;

    private string keyValue;

    //스폰 시간
    public float spawnTime = 2f;
    private float nowTime;

    //유닛 위치 저장
    private List<Vector3> savePosition = new List<Vector3>();

    private void Start()
    {
        //유닛 추가 됨에 따라 해당부분 추가 필요
        ObjectPoolManager.Instance.CreatePool("SwordMan", transform);

        SavePositions();

        //테스트 생성
        SpawnUnits(8, "SwordMan");
    }

    public void Update()
    {
        if (maxUnit != nowUnit)
        {
            Debug.Log(maxUnit);
            Debug.Log(nowUnit);

            nowTime += Time.deltaTime;

            if (nowTime >= spawnTime)
            {
                SpawnUnits(1, keyValue, true);
                nowTime = 0;
            }
        }

        if (Input.GetMouseButtonDown(0)) // 0은 왼쪽 마우스 버튼
        {
            Debug.Log("???");
            testunit[4].TakeDamage(10); // 예: 10만큼 데미지 입기
        }
    }

    public void SpawnUnits(int maxunit, string keyvalue, bool oneSpawn = false)
    {
        if (!oneSpawn)
        {
            //키 값 저장
            keyValue = keyvalue;

            for (int i = 0; i < maxunit; i++)
            {
                GameObject unit = ObjectPoolManager.Instance.GetPool(keyvalue);

                BarrackUnit position = unit.GetComponent<BarrackUnit>();
                position.unitPosition = savePosition[i];
                position.transform.position = savePosition[i];

                //테스트 데이터
                testunit.Add(position);
                //
            }

            maxUnit = maxunit;
            nowUnit = maxunit;
        }
        else if (oneSpawn)
        {
            nowUnit++;
            GameObject unit = ObjectPoolManager.Instance.GetPool(keyvalue);
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
