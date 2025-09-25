using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private BaseStatusSO originData;

    NormalAIController aiController;

    private BaseStatusSO data;


    public event Action<float, float> OnHealthChanged;
    public event Action OnDeath;

    private void Awake()
    {
        aiController = GetComponent<NormalAIController>();
        data = Instantiate(originData);

        data.currentHealth = data.maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        if (data == null) return;
        data.currentHealth -= damageAmount;
        data.currentHealth = Mathf.Clamp(data.currentHealth, 0, data.maxHealth);

        OnHealthChanged?.Invoke(data.currentHealth, data.maxHealth);
        aiController?.OnHit();

        if(data.currentHealth <= 0)
        {
            OnDeath?.Invoke();
            Death();
        }
    }

    protected virtual void Death()
    {
        ObjectPoolManager.Instance.ResivePool(gameObject.name, gameObject);
    }

    // 적이랑 플레이어랑 같이 쓸수있게 해놓았어요. 
}
