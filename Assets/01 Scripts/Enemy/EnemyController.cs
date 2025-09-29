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

    private void FixedUpdate()
    {
        if (body == null) return;
//        body.velocity *= 0.3f;
        if(body.velocity.sqrMagnitude <= 0.01f)
        {
            //kinematic body는 지원안한다고 경고뜸 확인바람
            body.velocity = Vector3.zero;
        }
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
