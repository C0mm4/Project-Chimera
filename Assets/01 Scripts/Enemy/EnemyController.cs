using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharacterStats
{
    public event Action<int, GameObject> OnDeathStageHandler;
    [SerializeField] private Rigidbody body;
    [SerializeField] private Transform weaponTrans;
    private EnemyData enemyData;
    [SerializeField] AIControllerBase aiController;
    BaseWeapon weapon;

    protected override void Awake()
    {
        base.Awake();
        body = GetComponent<Rigidbody>();
        body.freezeRotation = true;
        body.isKinematic = true;
    }

    [SerializeField] private int spawnWaveIndex;
    public void Initialize(int spawnWaveIndex)
    {
        this.spawnWaveIndex = spawnWaveIndex;
        data.currentHealth = data.maxHealth;
        enemyData = originData as EnemyData;
        ObjectPoolManager.Instance.CreatePool(enemyData.WeaponID, weaponTrans, 1);
        var obj = ObjectPoolManager.Instance.GetPool(enemyData.WeaponID, weaponTrans);
        if(obj != null)
        {
            weapon = obj.GetComponent<BaseWeapon>();
            weapon.InstigatorTrans = transform;
        }

        aiController = GetComponent<AIControllerBase>();
        if (aiController != null && obj != null)
        {
            aiController.weapon = weapon;
        }
    }

    protected override void Death()
    {
        base.Death();

        // 사망 시 무기 풀링 초기화
        var poolName = weapon.gameObject.name;
        ObjectPoolManager.Instance.ResivePool(poolName, weapon.gameObject, weaponTrans);
        ObjectPoolManager.Instance.ClearPool(poolName, weaponTrans);

        OnDeathStageHandler?.Invoke(spawnWaveIndex, gameObject);
    }
}
