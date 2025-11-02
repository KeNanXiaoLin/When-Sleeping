using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TilemapGrid : MonoBehaviour, IMapGenerator
{
    [Header("Tilemap引用")]
    public Tilemap obstacleTilemap;  // 障碍物瓦片地图（需在Inspector中拖入）
    public Tilemap groundTilemap;    // 地面瓦片地图（可选，用于确定网格范围）

    private Node[,] grid;  // A*网格节点数组
    private BoundsInt tilemapBounds;  // Tilemap的边界（用于确定网格大小）

    // 节点尺寸（瓦片大小，默认1x1单位，与Tilemap瓦片尺寸一致）
    public float nodeSize = 1f;

    // 初始化网格（从Tilemap提取数据）
    public Node[,] GenerateNodes()
    {
        // 获取Tilemap的边界（包含所有瓦片的最小矩形）
        // 优先用地面瓦片地图的边界，若为空则用障碍物瓦片地图
        tilemapBounds = groundTilemap != null && groundTilemap.size.x > 0
            ? groundTilemap.cellBounds
            : obstacleTilemap.cellBounds;

        // 计算网格尺寸（基于Tilemap边界的宽高）
        int gridSizeX = tilemapBounds.size.x;
        int gridSizeY = tilemapBounds.size.y;

        // 初始化节点数组
        grid = new Node[gridSizeX, gridSizeY];

        // 遍历Tilemap的每个瓦片位置，创建对应的Node
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                // 计算瓦片在Tilemap中的坐标（基于边界偏移）
                Vector3Int tilePosition = new Vector3Int(
                    tilemapBounds.xMin + x,
                    tilemapBounds.yMin + y,
                    0
                );

                // 判断该位置是否为障碍物：检查障碍物Tilemap中是否有瓦片
                bool isWalkable = !obstacleTilemap.HasTile(tilePosition);

                // 创建节点（存储瓦片坐标和通行状态）
                grid[x, y] = new Node(isWalkable, x, y, tilePosition);
            }
        }
        return grid;
    }

    // 将世界坐标转换为网格中的Node
    public Node GetNodeFromWorldPos(Vector3 worldPosition)
    {
        // 将世界坐标转换为Tilemap的瓦片坐标（整数）
        Vector3Int tilePosition = groundTilemap != null
            ? groundTilemap.WorldToCell(worldPosition)
            : obstacleTilemap.WorldToCell(worldPosition);

        // 计算在网格中的索引（基于边界偏移）
        int x = tilePosition.x - tilemapBounds.xMin;
        int y = tilePosition.y - tilemapBounds.yMin;

        // 检查是否在网格范围内
        x = Mathf.Clamp(x, 0, grid.GetLength(0) - 1);
        y = Mathf.Clamp(y, 0, grid.GetLength(1) - 1);

        return grid[x, y];
    }

    // 获取相邻节点（上下左右+斜对角）
    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();
        if (!AStarMgr.Instance.IsWalkStraight)
        {
            // 8个方向偏移
            int[] xOffsets = { -1, 0, 1, -1, 1, -1, 0, 1 };
            int[] yOffsets = { -1, -1, -1, 0, 0, 1, 1, 1 };

            for (int i = 0; i < 8; i++)
            {
                int checkX = node.gridX + xOffsets[i];
                int checkY = node.gridY + yOffsets[i];

                // 检查是否在网格范围内
                if (checkX >= 0 && checkX < grid.GetLength(0) &&
                    checkY >= 0 && checkY < grid.GetLength(1))
                {
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }
        else
        {
            // 8个方向偏移
            int[] xOffsets = { -1, 0, 1, 0 };
            int[] yOffsets = { 0, -1, 0, 1 };
            for (int i = 0; i < 4; i++)
            {
                int checkX = node.gridX + xOffsets[i];
                int checkY = node.gridY + yOffsets[i];

                // 检查是否在网格范围内
                if (checkX >= 0 && checkX < grid.GetLength(0) &&
                    checkY >= 0 && checkY < grid.GetLength(1))
                {
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbors;
    }

    // 辅助函数：将Node转换为世界坐标（瓦片中心位置）
    public Vector3 GetWorldPosFromNode(Node node)
    {
        // 从Node的瓦片坐标转换为世界坐标（Tilemap的瓦片中心）
        return groundTilemap != null
            ? groundTilemap.CellToWorld(node.tilePosition) + new Vector3(nodeSize / 2, nodeSize / 2, 0)
            : obstacleTilemap.CellToWorld(node.tilePosition) + new Vector3(nodeSize / 2, nodeSize / 2, 0);
    }

    // Gizmos绘制网格（在Scene窗口可视化）
    private void OnDrawGizmos()
    {
        if (AStarMgr.Instance.Config.isDebug)
        {
            if (grid == null) return;

            // 绘制每个节点
            foreach (Node node in grid)
            {
                // 可通行=白色，障碍物=红色
                Gizmos.color = node.isWalkable ? Color.white : Color.red;
                // 绘制节点的立方体（位置为瓦片中心）
                Vector3 worldPos = GetWorldPosFromNode(node);
                Gizmos.DrawCube(worldPos, Vector3.one * (nodeSize - 0.1f));
            }
        }

    }

    void UpdateBounds()
    {
        if (groundTilemap != null)
        {
            // 强制Tilemap重新计算边界
            groundTilemap.CompressBounds();
            // 此时再获取cellBounds就是最新的了
            BoundsInt newBounds = groundTilemap.cellBounds;
            Debug.Log("更新后的边界: " + newBounds);
        }
    }

    // 可以在编辑器中手动触发（比如挂在Tilemap对象上，通过按钮调用）
    [ContextMenu("刷新Tilemap边界")]
    void RefreshInEditor()
    {
        UpdateBounds();
    }
}