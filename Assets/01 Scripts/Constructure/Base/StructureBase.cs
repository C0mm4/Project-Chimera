using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StructureBase : MonoBehaviour
{
	[Header("Inspector 연결")]
    [SerializeField] protected StructureData structureData;
    [SerializeField] protected StructureSO statData; // 정진규: BaseStatusSO 에서 StructureSO로 변경
    [SerializeField] protected InteractionZone interactionZone; // 정진규: 건물도 업그레이드 하려면 필요
    
    private GameObject currentModelInstance; // 현재 생성된 건물 오브젝트를 기억(레벨)
//    public int CurrentLevel { get; private set; }

    public void Heal()
    {
        structureData.currentHealth = structureData.maxHealth;
    }

    public virtual void SetDataSO(StructureSO statData) // 정진규: BaseStatusSO 에서 StructureSO로 변경
    {
        this.statData = statData;

        structureData.maxHealth = statData.maxHealth;
        structureData.currentHealth = structureData.maxHealth;
        structureData.CurrentLevel = 1;

        CopyStatusData(this.statData);
        UpdateModel();
    }

    public abstract void CopyStatusData(BaseStatusSO statData);

    protected virtual void OnEnable()
    {
        if (interactionZone != null)
        {
            interactionZone.OnInteract += TryStartUpgrade;
        }

        if (statData != null)
            BuildEffect();
    }

    protected virtual void OnDisable()
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
    protected virtual void TryStartUpgrade()
    {
        //if (CurrentLevel >= statData.GetMaxLevel())
        //{
        //    Debug.Log("최고 레벨입니다.");
        //    return;
        //}

        //int nextLevel = CurrentLevel + 1;

        //if (nextLeve >= PlayerBaseManager.Instance.CurrentBaseLevel)
        //{
        //    Debug.Log("베이스 레벨이 낮아 업그레이드할 수 없습니다.");
        //    return;
        //}

        //int requiredCost = statData.levelProgressionData[CurrentLevel - 1].upgradeCost;
        //// if (GameManager.Instance.Gold < requiredCost) return;

        //ConfirmUpgrade();

        // --- 디버깅 코드 추가 ---
        Debug.Log("===== 업그레이드 시도 시작 =====");

        if (statData == null)
        {
            Debug.LogError("오류 원인: statData가 null입니다!");
            return;
        }
        else
        {
            Debug.Log($"성공: statData는 '{statData.name}' 입니다.");
        }

        if (PlayerBaseManager.Instance == null)
        {
            Debug.LogError("오류 원인: PlayerBaseManager.Instance가 null입니다! 씬에 없거나, 비활성화 상태이거나, 실행 순서가 늦습니다.");
            return;
        }
        else
        {
            Debug.Log($"성공: PlayerBaseManager.Instance는 '{PlayerBaseManager.Instance.gameObject.name}' 오브젝트에 있습니다.");
        }

        Debug.Log("===== 모든 변수 확인 완료, 업그레이드 로직 실행 =====");
        // --- 디버깅 코드 끝 ---


        // 더 이상 다음 레벨 건물이 없을 때
        if (structureData.CurrentLevel >= statData.GetMaxLevel())
        {
            Debug.Log("최고 레벨입니다.");
            return;
        }

        int nextLevel = structureData.CurrentLevel + 1;

        // 베이스 건물의 레벨이 부족할 때
        if (nextLevel > PlayerBaseManager.Instance.CurrentBaseLevel)
        {
            Debug.Log($"베이스 레벨이 부족합니다. (필요 베이스 레벨: {statData.GetMaxLevel()})"); // 여기를 GetMaxLevel()로 변경하는 것이 더 명확할 수 있습니다.
            return;
        }

        int requiredCost = statData.levelProgressionData[structureData.CurrentLevel - 1].upgradeCost;
        // if (GameManager.Instance.Gold < requiredCost) return;

        ConfirmUpgrade();
    }

    // 업그레이드 수락 메서드
    public virtual void ConfirmUpgrade()
    {
        // todo : 레벨 업 테이블 SO 만들어서 그거 로드해서 사용하는걸로 수정 필요
        int requiredCost = statData.levelProgressionData[structureData.CurrentLevel - 1].upgradeCost;

        // GameManager.Instance.Gold -= requiredCost; (골드 게임매니저에 있을거면)

        var upgrade = statData.upgradeData;
        if (upgrade == null) return;

        // 자신의 레벨 상태를 올립니다.
        structureData.CurrentLevel++;

        // 복제된 SO 데이터의 스탯을 직접 수정합니다.
        structureData.maxHealth += upgrade.maxHealthIncrease;
        structureData.currentHealth = statData.maxHealth;

        Debug.Log($"{name.Replace("SO", "(Instance)")} 업그레이드! -> Lv.{structureData.CurrentLevel}");

        // 레벨에 맞는 외형으로 교체합니다.
        UpdateModel();
        UpgradeApplyConcreteStructure();
    }

    public abstract void UpgradeApplyConcreteStructure();

    // 외형 오브젝트 업그레이드
    private void UpdateModel()
    {
        if (currentModelInstance != null)
        {
            Destroy(currentModelInstance); // 오브젝트 풀로 교체 필요?
        }

        string modelKey = statData.levelProgressionData[structureData.CurrentLevel - 1].modelAddressableKey;

        //if (!string.IsNullOrEmpty(modelKey))
        //{
        //    //currentModelInstance = ResourceManager.Instance.Create<GameObject>(modelKey, transform);
        //    //currentModelInstance.transform.localPosition = Vector3.zero;
        //    //currentModelInstance.transform.localRotation = Quaternion.identity;
        //}
    }

    protected virtual void OnReturnToPool() { }
}
[Serializable]
public struct StructureData
{
    public float currentHealth;
    public float maxHealth;

    public int CurrentLevel;
}