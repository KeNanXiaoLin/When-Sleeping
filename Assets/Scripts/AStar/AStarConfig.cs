using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 写一些A*算法的规范，方便在其他的项目中复用
/// </summary>
[CreateAssetMenu(fileName = "AStarConfig", menuName = "AStar/AStarConfig")]
public class AStarConfig : ScriptableObject
{
    [Header("必填参数")]
    /// <summary>
    /// 是否只能直着走，不能走斜线，在A*里的表现就是只能走上下左右四个方向
    /// </summary>
    [Tooltip("是否只能直着走，不能走斜线，在A*里的表现就是只能走上下左右四个方向")]
    public bool isWalkStraight = true;
    /// <summary>
    /// 每个场景用于A*寻路的Tilemap父对象的名字，场景中的名字必须和其保持一致
    /// </summary>
    [Tooltip("每个场景用于A*寻路的Tilemap父对象的名字，场景中的名字必须和其保持一致")]
    public string tilemapName = "AStarTilemap";
    /// <summary>
    /// 地面层名字，场景中的名字必须和其保持一致
    /// </summary>
    [Tooltip("地面层名字，场景中的名字必须和其保持一致")]
    public string groundTileName = "Ground";
    /// <summary>
    /// 每个场景用于A*寻路的Tilemap父对象的名字，场景中的名字必须和其保持一致
    /// </summary>
    [Tooltip("障碍层名字，场景中的名字必须和其保持一致")]
    public string obstacleTileName = "Obstacle";
    [Tooltip("是否开启debug模式，如果开启，会绘制出A*找到的路线，起点和终点等内容")]
    public bool isDebug = false;

    [Header("可选参数,可以自行添加任意层")]
    [Tooltip("水层名字，场景中的名字必须和其保持一致")]
    public string waterTileName = "Water";
    [Tooltip("从这里走过的花费")]
    public int waterCost = 10;
}
