using System;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private BaseStatusSO originData;

    AIController aiController;

    public BaseStatusSO data;


    public event Action<float, float> OnHealthChanged;
    public event Action OnDeath;

    /*
    private void Awake()
    {
        aiController = GetComponent<AIController>();
        data = Instantiate(originData);

        data.currentHealth = data.maxHealth;
    }
    */

    private void OnEnable()
    {
        // 이게 먼저 실행되도록 해야 안전함
        if (data == null)
        {
            data = Instantiate(originData);
            data.currentHealth = data.maxHealth;
        }

        if (aiController == null)
            aiController = GetComponent<AIController>();
    }


    public void TakeDamage(float damageAmount)
    {
        Debug.Log(data);
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
