using System.Collections;
using System.Collections.Generic;
using KNXL.DialogSystem;
using TMPro;
using UnityEngine;

public class DialogInfoPanel : UIPanelBase
{
    public TextMeshProUGUI tmpContent;
    public override void HideMe()
    {

    }

    public override void ShowMe()
    {

    }

    public void PlayNextDialog()
    {

    }

    public void ShowDialog(DialogData data)
    {
        this.tmpContent.text = data.dialogText;
    }
}
