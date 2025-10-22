using System.Collections;
using System.Collections.Generic;
using KNXL.DialogSystem;
using UnityEngine;

public class PlotSystem : SingletonAutoMono<PlotSystem>
{
    void Awake()
    {
        // gameStartDialogData = Resources.Load<RoleDialogData>("PlotData/GameStartDialog");
    }
    private void OnEnable()
    {

    }

    void OnDisable()
    {

    }

    public void PlayGameStartDialog()
    {
        DialogSystemMgr.Instance.StartPlayPlotDialog(10001);
    }
}
