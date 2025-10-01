using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public PlayerStatus Player;

    public void GameStart()
    {
       
    }

    private void Start()
    {
        UIManager.Instance.OpenUI<GameplayUI>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Time.timeScale = 5.0f;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            Time.timeScale = 1.0f;
        }
    }
}
