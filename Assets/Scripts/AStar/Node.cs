using UnityEngine;

public class Node
{
    public int gridX;  // 网格中的X索引
    public int gridY;  // 网格中的Y索引
    public Vector3Int tilePosition;  // Tilemap中的瓦片坐标（关键）

    public float gCost;  // 起点到当前节点的成本
    public float hCost;  // 到终点的预估成本
    public float fCost => gCost + hCost;

    public Node parent;
    public bool isWalkable;  // 是否可通行
    public int movementPenalty = 1;

    // 构造函数：增加tilePosition参数
    public Node(bool walkable, int x, int y, Vector3Int tilePos)
    {
        isWalkable = walkable;
        gridX = x;
        gridY = y;
        tilePosition = tilePos;
        movementPenalty = 1;
    }

    /// <summary>
    /// 传入走过花费的构造
    /// </summary>
    /// <param name="moveCost"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="tilePos"></param>
    public Node(int moveCost, int x, int y, Vector3Int tilePos)
    {
        movementPenalty = moveCost;
        gridX = x;
        gridY = y;
        tilePosition = tilePos;
        isWalkable = true;
    }
}