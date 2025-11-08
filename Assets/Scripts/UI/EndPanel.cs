using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPanel : UIPanelBase
{
    public override void HideMe()
    {
        
    }

    public override void ShowMe()
    {

    }
    
    protected override void ClickBtn(string btnName)
    {
        switch (btnName)
        {
            case "Quit":
                Debug.Log("EndPanel退出按钮被点击");
                Application.Quit();
                break;
        }
    }
}
