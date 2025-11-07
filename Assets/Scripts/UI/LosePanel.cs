using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LosePanel : UIPanelBase
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
                Application.Quit();
                break;
        }
    }
}
