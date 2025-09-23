using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;

    public float MaxHealth { get; private set; }
    public float CurrentHealth { get; private set; }
    public float Damage { get; private set; }
    public float MoveSpeed { get; private set; }

    public event Action<float, float> OnHealthChanged;
    public event Action OnDeath;

    private void Awake()
    {
        if(enemyData != null)
        {
            MaxHealth = enemyData.maxHealth;
            Damage = enemyData.damage;
            MoveSpeed = enemyData.moveSpeed;
        }

        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        CurrentHealth -= damageAmount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);

        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);

        if(CurrentHealth <= 0)
        {
            OnDeath?.Invoke();
        }
    }

    // 적이랑 플레이어랑 같이 쓸수있게 해놓았어요.
}
