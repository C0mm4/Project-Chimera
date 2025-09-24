using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "EnemySO")]
public class StageWaveSO : ScriptableObject
{
    public List<MonsterSpawnInfo> monsters;
}

[System.Serializable]
public class MonsterSpawnInfo
{
    public string keyName; // 어드레서블 키
    public string prefabName; // 어드레서블 키와 동일
    public int enemyCount;
    public int spawnPoint;
    public int spawnCount;
    public float spawnTime;
}