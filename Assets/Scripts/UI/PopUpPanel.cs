using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpPanel : UIPanelBase
{
    public TextMeshProUGUI tmpDetails;
    public Image icon;
    public override void HideMe()
    {

    }

    public override void ShowMe()
    {

    }

    public void UpdateInfo(Sprite sprite, string content)
    {
        icon.sprite = sprite;
        tmpDetails.text = content;
    }
}
