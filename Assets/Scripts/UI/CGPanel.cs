using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CGPanel : UIPanelBase
{
    public TextMeshProUGUI tmpTips;
    public Image img;
    public override void HideMe()
    {

    }

    public override void ShowMe()
    {

    }

    public void UpdateImage(Sprite sprite)
    {
        img.sprite = sprite;
    }

    public void UpdateTips(string str)
    {
        tmpTips.text = str;
    }
}
