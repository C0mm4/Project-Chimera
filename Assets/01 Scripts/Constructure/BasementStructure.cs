using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasementStructure : StructureBase
{
    private void OnEnable()
    {
        StageManager.Instance.Basement = this;
    }
}
