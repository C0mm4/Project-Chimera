using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    // Stage 관련
    public int CurrentStage;
    public int MaxClearStage;

    public int RetryCountCurrentStage;   // 현재 스테이지 리트라이 횟수 (업적같은 거 추가하면 사용할수도?
    public int MaxCountRetryOneStage;   // 한 스테이지에서 최대 리트횟수 

    public int Gold;
    public int CollectedGolds; // 획득한 총 골드
    public int ConsumeGolds;    // 사용한 총 골드

    public int getGoldCurrentStage;
}
