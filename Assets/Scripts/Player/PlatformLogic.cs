using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 平台逻辑处理类
/// </summary>
public class PlatformLogic
{
    //对应游戏对象
    private Player obj;
    //游戏对象当前所在平台
    private Platform nowPlatform;
    //游戏中所有平台数据
    private List<Platform> platformData;

    public PlatformLogic(Player obj)
    {
        this.obj = obj;
        nowPlatform = null;
        platformData = PlatformDataMgr.Instance.platformList;
    }

    /// <summary>
    /// 用于每帧检测玩家平台变化的函数
    /// </summary>
    public void UpdateCheck()
    {
        //当玩家跳跃时 才会切换平台
        //当玩家下落时 也会切换平台
        if(obj.isJump || obj.isFall)
        {
            //让每一次遍历寻找到的平台 是这一瞬间最高的平台 而不是整个跳跃轨迹中的最高平台
            nowPlatform = null;
            for (int i = 0; i < platformData.Count; i++)
            {
                //不停的判断玩家是否处于落在某个平台的条件下
                if( platformData[i].CheckObjFallOnMe(obj.transform.position) &&
                    (nowPlatform == null || nowPlatform.Y < platformData[i].Y))
                {
                    //记录当前平台
                    nowPlatform = platformData[i];
                    //更新玩家相关的平台数据
                    obj.ChangePlatformData(nowPlatform.Y, nowPlatform.canFall);
                }
            }
        }


        //只会检测当前所在平台 是否满足了下平台的条件
        if( !obj.isJump && !obj.isFall &&
            nowPlatform != null && nowPlatform.canFall &&
            !nowPlatform.CheckObjFallOnMe(obj.transform.position))
        {
            Debug.Log($"我是否在平台之上{obj.transform.position.y >= nowPlatform.Y}");
            Debug.Log($"这一帧我的位置{obj.transform.position.y}，这一帧平台的位置{nowPlatform.Y}");
            Debug.Log($"我是否超出了平台的左边界{obj.transform.position.x <= nowPlatform.Left}");
            Debug.Log($"我是否超出了平台的右边界{obj.transform.position.x >= nowPlatform.Right}");
            obj.Fall();
        }
    }
}
