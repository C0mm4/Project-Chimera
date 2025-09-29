using System;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] protected BaseStatusSO originData;

    protected AIControllerBase aiController;

    [SerializeField] public StatusData data;

    public event Action<float> OnHealthChanged;
    public event Action OnDeath;


    public GaugeBarUI gaugebarUI;

    protected virtual void Awake()
    {
        aiController = GetComponent<AIControllerBase>();
        
        if(originData != null)
        {
            data.maxHealth = originData.maxHealth;

            data.currentHealth = data.maxHealth;
            data.moveSpeed = originData.moveSpeed;
        }

        if (gaugebarUI == null)
        {
            ObjectPoolManager.Instance.CreatePool("Pref_110000", transform);

            GameObject obj = ObjectPoolManager.Instance.GetPool("Pref_110000", transform);
            gaugebarUI = obj.GetComponent<GaugeBarUI>();
        }
    }



    public void TakeDamage(Transform instigator, float damageAmount)
    {

        //        Debug.Log(data);
        //        if (data == null) return;
        data.currentHealth -= damageAmount;
        data.currentHealth = Mathf.Clamp(data.currentHealth, 0, data.maxHealth);

        Debug.Log("때리는 주체 : " + instigator);
        Debug.Log(data.currentHealth);

        float percent = data.currentHealth / data.maxHealth;
        //OnHealthChanged?.Invoke(percent);
        //데미지를 받을때 여기밖에 안거치는거같음
        UpdateHealthUI(percent);
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

    private void UpdateHealthUI(float percent)
    {
        gaugebarUI.SetFillPercent(percent);
    }

}

[Serializable]
public struct StatusData
{
    public float currentHealth;
    public float maxHealth;

    public float moveSpeed;
}