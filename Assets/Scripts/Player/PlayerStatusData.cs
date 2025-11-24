using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStatusData
{
    /// <summary>
    /// 最大San值
    /// </summary>
    public int maxSan = 100;
    private int curSan;
    /// <summary>
    /// 移动速度
    /// </summary>
    public float moveSpeed = 5;
    /// <summary>
    /// 初始跳跃时的 竖直上抛速度
    /// </summary>
    public float initYSpeed = 10;
    /// <summary>
    /// 重力加速度
    /// </summary>
    public float G = 9.8f;
    /// <summary>
    /// 玩家单次跳跃的最高位置
    /// </summary>
    // public float jumpMaxH = 5;
    /// <summary>
    /// 最大跳跃次数
    /// </summary>
    public int maxJumpTimes = 2;
    /// <summary>
    /// 攻击力的大小
    /// </summary>
    public int atkSize = 10;
    /// <summary>
    /// 攻击的范围，因为进行的是盒装检测，所以实际范围会*2
    /// </summary>
    public float atkRange = 0.5f;
    /// <summary>
    /// 攻击间隔
    /// </summary>
    public float atkInterval = 1f;
    /// <summary>
    /// 最大生命值
    /// </summary>
    public int maxHp = 100;
    /// <summary>
    /// 是否开启Debug模式
    /// </summary>
    public bool isDebug = false;

    public PlayerStatusData()
    {
        curSan = maxSan;
    }

    public void ChangeSan(int value)
    {
        curSan += value;
        if (curSan > maxSan)
        {
            curSan = maxSan;
        }
        else if (curSan <= 0)
        {
            curSan = 0;
        }
        EventCenter.Instance.EventTrigger<int>(E_EventType.E_SanChange, curSan);
    }
}
