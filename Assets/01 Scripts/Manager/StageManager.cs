using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    // Spawner spawner;
    // public Spawner spawner; 
    public BasementStructure Basement;
    public Stage Stage;

    [SerializeField] private int stageN;
    public static GameData data;

    public StageState state = StageState.Ready;

    public event Action OnEndStage;

    private void Awake()
    {
        data = new GameData();
        data.CurrentStage = 1;
        state = StageState.Ready;
    }

    // For Test
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G) && state == StageState.Ready)
        {
            NextStage();
        }
    }

    public void StageClear()
    {
        state = StageState.Ready;
        data.MaxClearStage = data.CurrentStage++;
        OnEndStage?.Invoke();
    }

    public void NextStage()
    {
        EnemySpawn.Instance.StartStage(data.CurrentStage);
    }

    public void GetGold(int amount)
    {
        data.Gold += amount;
        data.CollectedGolds += amount;
    }

    public bool ConsumeGold(int amount)
    {
        if (data.Gold < amount) return false;

        data.Gold -= amount;
        data.ConsumeGolds += amount;

        return true;
    }
}


public enum StageState
{
    None, Ready, InPlay,
}