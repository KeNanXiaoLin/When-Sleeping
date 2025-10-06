using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RealCGPanel : UIPanelBase
{
    public Image image;
    public override void HideMe()
    {

    }

    public override void ShowMe()
    {

    }

    public void UpdateInfo(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
