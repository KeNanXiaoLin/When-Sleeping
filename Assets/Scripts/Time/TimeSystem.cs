using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_TimeType
{
    /// <summary>
    /// 早上
    /// </summary>
    Moring,
    /// <summary>
    /// 下午
    /// </summary>
    Afternoon,
    /// <summary>
    /// 晚上
    /// </summary>
    Night,
}

/// <summary>
/// 时间系统，定义现实中的1s为游戏中的1min
/// </summary>
public class TimeSystem : SingletonAutoMono<TimeSystem>
{
    //游戏时间和现实时间的速率比，60相当于现实1s游戏1min
    private int timeSpeed = 60;
    private int gameStartDay = 1;
    private int gameStartHour = 6;
    private int gameStartMinute = 0;
    private int gameStartSecond = 0;
    private E_TimeType gameStartTimeType = E_TimeType.Moring;

    private int curDay;
    private int curHour;
    private int curMinute;
    private int curSecond;
    private E_TimeType curTimeType;
    private float totalPassSecond;

    public E_TimeType CurTimeType { get => curTimeType; }
    public int CurDay { get => curDay; }

    #region 控制早中晚事件一天只分发一次的变量
    private bool isHandleMorningEvent = false;
    private bool isHandleAfternoonEvent = false;
    private bool isHandleNightEvent = false;
    #endregion
    private bool isPause = false;
     private int lastDay; // 新增：记录上一帧的天数，用于检测跨天

    private TimeSystem()
    {
        curDay = 0;
        curHour = 0;
        curMinute = 0;
        curSecond = 0;
        curTimeType = gameStartTimeType;
        totalPassSecond = 0;
        lastDay = curDay; 
    }

    void Update()
    {
        if (isPause) return;
        totalPassSecond += Time.deltaTime * timeSpeed;
        CalcNowTime(); 
        EventCenter.Instance.EventTrigger<string>(E_EventType.E_UpdateTime, GetRealTime());
        CheckNextDay6AM();
        curTimeType = calNowTime();
        HandleDiffTimeEvent();
        GameManager.Instance.Update();
    }

    /// <summary>
    /// 计算当前时间
    /// </summary>
    /// <returns></returns>
    public void CalcNowTime()
    {
        int totalS = (int)totalPassSecond; // 假设totalPassSecond是已定义的总秒数变量

        // 计算天数
        // 这里因为是从早上6点开始计算，所以再过18小时就会是下一天
        // 我们直接根据小时数来计算天数
        curDay = totalS / 86400;
        // // 剩余秒数（扣除天数部分）
        int remainingSeconds = totalS % 86400;
        // int remainingSeconds = totalS;
        // 计算小时
        curHour = remainingSeconds / 3600;
        // 剩余秒数（扣除小时部分）
        remainingSeconds %= 3600;

        // 计算分钟
        curMinute = remainingSeconds / 60;
        // 剩余秒数（最终为秒数）
        curSecond = remainingSeconds % 60;
    }

    public string GetRealTime()
    {
        string res = "";
        int realDay = gameStartDay + curDay + (gameStartHour + curHour) / 24;
        int realHour = (gameStartHour + curHour) % 24;
        int realMinute = gameStartMinute + curMinute;
        int realSecond = gameStartSecond + curSecond;
        return $"Day {realDay} {realHour}h {realMinute}m ";
    }

    /// <summary>
    /// 检测是否到达下一天的6点，并重置每日事件状态
    /// </summary>
    private void CheckNextDay6AM()
    {
        // 当天数增加（跨天），因为记录的是游戏开始实际过了多久，所以只用判断过天
        if (curDay > lastDay)
        {
            // 重置每日事件的触发状态（新的一天需要重新触发早中晚事件）
            isHandleMorningEvent = false;
            isHandleAfternoonEvent = false;
            isHandleNightEvent = false;

            // 更新lastDay为当前天数，避免重复触发
            lastDay = curDay;
        }
    }

    /// <summary>
    /// 加速一天
    /// </summary>
    public void SpeedUpOneDay()
    {
        totalPassSecond += 60 * 60 * 24;
    }

    /// <summary>
    /// 加速一个小时
    /// </summary>
    public void SpeedUpOneHour()
    {
        totalPassSecond += 60 * 60;
    }

    public void SpeedUpThreeHour()
    {
        totalPassSecond += 60 * 60 * 3;
    }

    /// <summary>
    /// 计算当前是上午还是晚上
    /// </summary>
    /// <returns></returns>
    private E_TimeType calNowTime()
    {
        if (curHour >= 0 && curHour < 6)
        {
            return E_TimeType.Moring;
        }
        else if (curHour >= 6 && curHour < 14)
        {
            return E_TimeType.Afternoon;
        }
        else
        {
            return E_TimeType.Night;
        }
    }

    /// <summary>
    /// 分发不同时间段的事件，每天都有早中晚，每次进入这三个时间段的时候分发一次事件
    /// </summary>
    private void HandleDiffTimeEvent()
    {
        //如果没有分发过早晨事件，分发一次，设置状态为已分发
        if (!isHandleMorningEvent && curTimeType == E_TimeType.Moring)
        {
            EventCenter.Instance.EventTrigger<int>(E_EventType.E_Morning, curDay);
            isHandleMorningEvent = true;
            Debug.Log("分发早晨事件");
        }
        else if (!isHandleAfternoonEvent && curTimeType == E_TimeType.Afternoon)
        {
            EventCenter.Instance.EventTrigger<int>(E_EventType.E_Afternoon, curDay);
            isHandleAfternoonEvent = true;
            Debug.Log("分发中午事件");
        }
        else if (!isHandleNightEvent && curTimeType == E_TimeType.Night)
        {
            EventCenter.Instance.EventTrigger<int>(E_EventType.E_Night, curDay);
            isHandleNightEvent = true;
            Debug.Log("分发晚上事件");
        }
        //如果到了第二天的6点，刷新时间分配的状态,已经在其他方法中处理
    }

    /// <summary>
    /// 对外提供：跳转到指定“实际天数”的6点（核心需求）
    /// </summary>
    /// <param name="targetRealDay">目标实际天数（如2=跳转到第2天6点）</param>
    public void JumpToTargetDay(int targetRealDay)
    {
        // 1. 校验参数合法性（不能小于初始天数）
        if (targetRealDay < gameStartDay)
        {
            Debug.LogWarning($"目标天数不能小于初始天数{gameStartDay}，请重新传入！");
            return;
        }
        totalPassSecond = targetRealDay * 86400f;
    }

    public void JumpToNextDay()
    {
        int nextDay = curDay + 1;
        JumpToTargetDay(nextDay);
    }

    /// <summary>
    /// 暂停时间，主要是玩家没有进入游戏的时候调用
    /// </summary>
    public void PauseTime()
    {
        isPause = true;
    }

    public void RecoverTime()
    {
        isPause = false;
    }
}
