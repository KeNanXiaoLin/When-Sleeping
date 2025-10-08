using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "MyAssets/PlayerData", order = 3)]
public class PlayerData : ScriptableObject
{
    /// <summary>
    /// 这是第一次进入场景玩家的位置数据
    /// </summary>
    public Vector3 gameStartPos;
}
