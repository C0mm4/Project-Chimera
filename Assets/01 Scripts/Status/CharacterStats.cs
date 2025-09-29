using System;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private BaseStatusSO originData;

    AIControllerBase aiController;

    public StatusData data;


    public event Action<float, float> OnHealthChanged;
    public event Action OnDeath;

    
    protected virtual void Awake()
    {
        aiController = GetComponent<AIControllerBase>();
        
        data.maxHealth = originData.maxHealth;

        data.currentHealth = data.maxHealth;
        data.moveSpeed = originData.moveSpeed;
    }



    public void TakeDamage(Transform instigator, float damageAmount)
    {
//        Debug.Log(data);
//        if (data == null) return;
        data.currentHealth -= damageAmount;
        data.currentHealth = Mathf.Clamp(data.currentHealth, 0, data.maxHealth);

        OnHealthChanged?.Invoke(data.currentHealth, data.maxHealth);
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
        ObjectPoolManager.Instance.ResivePool(gameObject.name, gameObject);
    }

    // 적이랑 플레이어랑 같이 쓸수있게 해놓았어요.
}


public struct StatusData
{
    public float currentHealth;
    public float maxHealth;

    public float moveSpeed;
}