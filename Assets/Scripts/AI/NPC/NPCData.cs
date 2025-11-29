using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCData
{
    /// <summary>
    /// NPC的类型
    /// </summary>
    public E_NPCType npcType;
    /// <summary>
    /// NPC的位置
    /// </summary>
    public MyVector3 pos;
    /// <summary>
    /// 是否跟随玩家，因为在游戏里，NPC只会跟随玩家
    /// </summary>
    public bool isFollowPlayer;
    /// <summary>
    /// NPC的家的位置
    /// </summary>
    public MyVector3 homePos;
    /// <summary>
    /// NPC的移动速度
    /// </summary>
    public float moveSpeed = 3f;
    /// <summary>
    /// 跟随保持的距离
    /// </summary>
    public float followDis = 1f;
    /// <summary>
    /// 是否处于移动状态
    /// </summary>
    public bool isMoving = false;
}
