using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "EnemySO")]
public class EnemySO : ScriptableObject
{
    public List<StageWave> stageWaves;
}

[System.Serializable]
public class StageWave
{
    public int stageNumber;
    public List<MonsterSpawnInfo> monsters;
}

[System.Serializable]
public class MonsterSpawnInfo
{
    public string keyName;
    public string prefabName; // 어드레서블 키
    public int enemyCount;
    public int spawnPoint;
    public int spawnCount;
    public float spawnTime;
}