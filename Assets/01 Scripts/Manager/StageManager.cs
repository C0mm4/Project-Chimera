using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    // Spawner spawner;
    // public Spawner spawner; 
    public BasementStructure Basement;

    [SerializeField] private int stageN;

    public StageState state = StageState.Ready;

    // For Test
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G) && state == StageState.Ready)
        {
            EnemySpawn.Instance.StartStage(++stageN);
        }
    }

    public void StageClear()
    {
        state = StageState.Ready;
    }
}


public enum StageState
{
    None, Ready, InPlay,
}