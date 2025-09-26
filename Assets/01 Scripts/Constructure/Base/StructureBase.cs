using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StructureBase : MonoBehaviour
{
    private BaseStatusSO originData;
    [SerializeField] private StructureData structureData;

    public void Heal()
    {
        structureData.currentHealth = structureData.maxHealth;
    }

    public virtual void SetDataSO(BaseStatusSO statData)
    {
        originData = statData;
        structureData.maxHealth = statData.maxHealth;
        structureData.currentHealth = structureData.maxHealth;
        CopyStatusData(statData);
    }

    public abstract void CopyStatusData(BaseStatusSO statData);

    private void OnEnable()
    {
        if(originData != null)
            BuildEffect();
    }

    private void OnDisable()
    {
        if (originData != null)
            DestroyEffect();
    }

    private void Update()
    {
        if (originData != null)
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
[Serializable]
public struct StructureData
{
    public float currentHealth;
    public float maxHealth;
}