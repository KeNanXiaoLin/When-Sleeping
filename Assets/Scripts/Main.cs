using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    void Start()
    {
        UIManager.Instance.ShowPanel<GameStartUI>();
        //初始化剧情系统
        PlotSystem.Instance.Init();
        //开始界面，禁用时间流逝
        TimeSystem.Instance.PauseTime();
        // CGManager.Instance.PlayKaiTouCG();
        // CGManager.Instance.PlayChiRenCG();
        // CGManager.Instance.PlayEndCG();
        // CGManager.Instance.PlayJuqing();
    }

    
}
