using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewBarrackData", menuName = "Constructure Data/New Barrack Data")]
public class BarrackSO : BaseStatusSO
{
    public string spawnUnitKey;
    public int spawnCount;
    public float spawnRate;
}
