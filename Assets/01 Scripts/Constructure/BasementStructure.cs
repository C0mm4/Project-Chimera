using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasementStructure : StructureBase
{

    public override void CopyStatusData(BaseStatusSO statData)
    {

        
    }

    private void OnEnable()
    {
        StageManager.Instance.Basement = this;
    }
}

