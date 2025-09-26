using System;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private BaseStatusSO originData;

    GroundAIController aiController;

    public BaseStatusSO data;


    public event Action<float, float> OnHealthChanged;
    public event Action OnDeath;

    
    protected virtual void Awake()
    {
        aiController = GetComponent<GroundAIController>();
        data = Instantiate(originData);

        data.currentHealth = data.maxHealth;
    }



    public void TakeDamage(Transform instigator, float damageAmount)
    {
        Debug.Log(data);
        if (data == null) return;
        data.currentHealth -= damageAmount;
        data.currentHealth = Mathf.Clamp(data.currentHealth, 0, data.maxHealth);

        OnHealthChanged?.Invoke(data.currentHealth, data.maxHealth);
        aiController?.OnHit(instigator);

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
