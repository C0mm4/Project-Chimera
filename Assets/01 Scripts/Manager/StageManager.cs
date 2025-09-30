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

    public event Action OnStageFail;
    public event Action OnStageClear;

    public static GameData data;

    public StageState state = StageState.Ready;


    private void Awake()
    {
        data = new GameData();
        data.CurrentStage = 1;
        state = StageState.Ready;
        OnStageClear += ClearCallback;
        OnStageFail += FailCallback;
    }

    // For Test
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G) && state == StageState.Ready)
        {
            NextStage();
        }
    }

    public void FailStage()
    {
        Debug.Log("스테이지 실패. 3초 후 관련함수 호출");
        StartCoroutine(CallActionAfterDelay(OnStageFail, 3f));
    }

    public void StageClear()
    {
        Debug.Log("스테이지 클리어. 3초 후 관련함수 호출");
        StartCoroutine(CallActionAfterDelay(OnStageClear, 3f));
    }

    private void ClearCallback()
    {
        state = StageState.Ready;
        data.MaxClearStage = data.CurrentStage++;
    }

    private void FailCallback()
    {
        state = StageState.Ready;
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

    IEnumerator CallActionAfterDelay(Action action, float delay)
    {
        Debug.Log($"{delay}초 기다리는 중..");
        yield return new WaitForSeconds(delay);
        Debug.Log($"{delay}초 지남. 호출");
        foreach (var d in action.GetInvocationList())
        {
            Debug.Log($"구독된 메서드: {d.Method.Name}, 대상: {d.Target}");
        }

        action.Invoke();
    }
}




public enum StageState
{
    None, Ready, InPlay,
}