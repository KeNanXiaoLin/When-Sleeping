using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_StateType
{
    /// <summary>
    /// 表示不处于任何一个状态
    /// </summary>
    None,
    /// <summary>
    /// 生活场景的待机状态
    /// </summary>
    Idle,
    /// <summary>
    /// 生活场景的移动状态
    /// </summary>
    Move,
    /// <summary>
    /// 战斗场景的待机状态
    /// </summary>
    Battle_Idle,
    /// <summary>
    /// 战斗场景的移动状态
    /// </summary>
    Battle_Move,
    /// <summary>
    /// 战斗场景的攻击状态
    /// </summary>
    Battle_Attack,
    /// <summary>
    /// 战斗场景的受伤状态
    /// </summary>
    Battle_Damage,
    /// <summary>
    /// 战斗场景的死亡状态
    /// </summary>
    Battle_Death,
}
