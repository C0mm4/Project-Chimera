using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharacterStats
{
    public event Action<int, GameObject> OnDeathStageHandler;
    [SerializeField] private Rigidbody body;

    protected override void Awake()
    {
        base.Awake();
        body = GetComponent<Rigidbody>();
    }

    [SerializeField] private int spawnWaveIndex;
    public void Initialize(int spawnWaveIndex)
    {
        this.spawnWaveIndex = spawnWaveIndex;
        data.currentHealth = data.maxHealth;
    }

    protected override void Death()
    {
        base.Death();
        OnDeathStageHandler?.Invoke(spawnWaveIndex, gameObject);
    }
}
