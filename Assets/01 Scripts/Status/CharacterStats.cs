using System;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] protected BaseStatusSO originData;

    AIControllerBase aiController;

    public StatusData data;


    public event Action<float> OnHealthChanged;
    public event Action OnDeath;

    
    protected virtual void Awake()
    {
        aiController = GetComponent<AIControllerBase>();
        
        if(originData != null)
        {
            data.maxHealth = originData.maxHealth;

            data.currentHealth = data.maxHealth;
            data.moveSpeed = originData.moveSpeed;
        }
    }



    public void TakeDamage(Transform instigator, float damageAmount)
    {
//        Debug.Log(data);
//        if (data == null) return;
        data.currentHealth -= damageAmount;
        data.currentHealth = Mathf.Clamp(data.currentHealth, 0, data.maxHealth);

        float percent = data.currentHealth / data.maxHealth;
        OnHealthChanged?.Invoke(percent);
        if (aiController != null)
        {
            aiController.OnHit(instigator);
        }

        if(data.currentHealth <= 0)
        {
            OnDeath?.Invoke();
            Death();
        }
    }

    protected virtual void Death()
    {
        
    }
}


public struct StatusData
{
    public float currentHealth;
    public float maxHealth;

    public float moveSpeed;
}