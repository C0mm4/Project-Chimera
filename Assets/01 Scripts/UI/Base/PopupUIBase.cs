using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupUIBase : UIBase
{
    void Start()
    {
        Canvas canvas = GetComponent<Canvas>();
        UIManager.Instance.InitPopupCanvas(canvas);
    }

    
}
