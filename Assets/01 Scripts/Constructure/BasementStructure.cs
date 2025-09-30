using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasementStructure : StructureBase
{
    public override void CopyStatusData(BaseStatusSO statData)
    {

    }

    protected override void OnEnable()
    {
        base.OnEnable();

        StageManager.Instance.Basement = this;
        StageManager.Instance.OnStageFail += OnFail;

        
    }

    private void OnFail()
    {
        Debug.Log("스테이지 실패 시 해야할 작업");
    }

    public override void ConfirmUpgrade()
    {
        base.ConfirmUpgrade(); // 기본 업그레이드 로직 실행
        PlayerBaseManager.Instance.OnBaseLevelUp(); // 중앙 관리자에게 레벨업 알림
    }

    protected override void OnReturnToPool()
    {
        base.OnReturnToPool();
        // Todo: 베이스 레벨업을 해서 기존 건물을 풀에 넣기 전에 할 것
    }
    public override void UpgradeApplyConcreteStructure()
    {
    }


    protected override void Death()
    {
        base.Death();
        Debug.Log("베이스먼트 데스 함수");
        StageManager.Instance.FailStage();
        
    }
}


