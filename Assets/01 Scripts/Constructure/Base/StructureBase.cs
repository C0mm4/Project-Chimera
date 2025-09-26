using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureBase : MonoBehaviour
{
    [Header("Inspector 연결")]
    [SerializeField] protected StructureSO statData; // 정진규: BaseStatusSO 에서 StructureSO로 변경

    public void Heal()
    {
        statData.currentHealth = statData.maxHealth;
    }

    public virtual void SetDataSO(StructureSO statData) // 정진규: BaseStatusSO 에서 StructureSO로 변경
    {
        this.statData = statData;
        this.statData.currentHealth = statData.maxHealth;
    }

    private void OnEnable()
    {
        if(statData != null)
            BuildEffect();
    }

    private void OnDisable()
    {
        if (statData != null)
            DestroyEffect();
    }

    private void Update()
    {
        if (statData != null)
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
