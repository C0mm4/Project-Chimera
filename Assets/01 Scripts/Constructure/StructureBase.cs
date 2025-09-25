using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureBase : MonoBehaviour
{
    [Header("Inspector 연결")]
    [SerializeField] private CharacterStats statData;

    public void Heal()
    {
        statData.data.currentHealth = statData.data.maxHealth;
    }

    private void OnEnable()
    {
        BuildEffect();
    }

    private void OnDisable()
    {
        DestroyEffect();
    }

    private void Update()
    {
        UpdateEffect();
    }

    protected virtual void BuildEffect()
    {

    }

    protected virtual void DestroyEffect()
    {

    }

    protected virtual void UpdateEffect()
    {

    }
}
