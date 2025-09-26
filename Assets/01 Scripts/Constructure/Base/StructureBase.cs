using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureBase : MonoBehaviour
{
    [Header("Inspector 연결")]
    [SerializeField] protected StructureSO statData; // 정진규: BaseStatusSO 에서 StructureSO로 변경
    [SerializeField] protected InteractionZone interactionZone; // 정진규: 건물도 업그레이드 하려면 필요

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
        if (interactionZone != null)
        {
            interactionZone.OnInteract += TryStartUpgrade;
        }

        if (statData != null)
            BuildEffect();
    }

    private void OnDisable()
    {
        if (interactionZone != null)
        {
            interactionZone.OnInteract -= TryStartUpgrade;
        }

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

    // 업그레이드 시도 메서드
    private void TryStartUpgrade()
    {
        // 더 이상 다음 레벨 건물이 없을 때
        if(statData.nextLevelSO == null)
        {
            Debug.Log("현재 최고 레벨 건물입니다.");
        }

        // 베이스 건물의 레벨이 부족할 때
        if (statData.nextLevelSO.level > PlayerBaseManager.Instance.CurrentBaseLevel)
        {
            Debug.Log($"베이스 레벨이 부족합니다. (필요 베이스 레벨: {statData.nextLevelSO.level})");
            return;
        }

        // Todo: 골드 조건 확인 로직

        // Todo: 업그레이드 UI 호출
        // 확인 버튼 클릭시 ConfirmUpgrade() 메서드 호출 필요
        // 테스트로 그냥 인터렉션존에 일정 시간 머무르면 레벨업
        ConfirmUpgrade();
    }

    // 업그레이드 수락 메서드
    public virtual void ConfirmUpgrade()
    {
        // TODO: 골드 차감 로직

        var upgradedBuilding = ObjectPoolManager.Instance.GetPool(statData.nextLevelPrefabKey);
        upgradedBuilding.transform.position = this.transform.position;
        upgradedBuilding.transform.rotation = this.transform.rotation;

        upgradedBuilding.GetComponent<StructureBase>().SetDataSO(statData.nextLevelSO);

        // 풀에 넣기 전 초기화 할 것 메서드
        OnReturnToPool();

        ObjectPoolManager.Instance.ResivePool(statData.prefabKey, this.gameObject);
    }

    protected virtual void OnReturnToPool() { }
}
