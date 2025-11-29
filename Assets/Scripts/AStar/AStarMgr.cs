using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarMgr : BaseManager<AStarMgr>
{
    private AStarConfig aStarConfig;
    public bool IsDebug => aStarConfig.isDebug;
    public bool IsWalkStraight => aStarConfig.isWalkStraight;
    public AStarConfig Config => aStarConfig;
    public string currentMap => GameManager.Instance.currentSceneName;
    /// <summary>
    /// 当前地图的MapNode
    /// </summary>
    public AStarMapNode currentMapNode;

    /// <summary>
    /// 用来存储每张地图的A*寻路的地图信息
    /// </summary>
    // private Dictionary<string, AStarMapNode> allMapNodes = new();
    //由于Unity过场景会重新创建所有场景中的物品，所以使用字典来存储是不合适的，我们只需要记录当前地图的mapNode即可
    private AStarMgr()
    {
        aStarConfig = Resources.Load<AStarConfig>("AStar/AStarConfig");
    }


    // 寻路入口：从世界坐标到世界坐标
    public List<Vector3> FindPath(Vector3 startWorldPos, Vector3 targetWorldPos)
    {
        if (currentMapNode == null)
            return null;
        Node startNode = currentMapNode.GetNodeFromWorldPos(startWorldPos);
        Node targetNode = currentMapNode.GetNodeFromWorldPos(targetWorldPos);

        // 开放列表（待探索节点）和关闭列表（已探索节点）
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            // 从开放列表中找fCost最小的节点
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost ||
                    (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            // 找到终点，回溯路径
            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            // 遍历相邻节点
            foreach (Node neighbor in currentMapNode.GetNeighbors(currentNode))
            {
                // 跳过不可通行或已探索的节点
                if (!neighbor.isWalkable || closedSet.Contains(neighbor))
                {
                    continue;
                }

                // 计算到相邻节点的成本（直线10，斜线14，与前面Python版本一致）
                float newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                // 如果是更优路径，更新节点信息
                if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);  // 启发函数（曼哈顿距离）
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }
        // 开放列表为空，无路径
        return null;
    }

    // 回溯路径：从终点通过parent反向找到起点，转换为世界坐标
    private List<Vector3> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();  // 反转路径，从起点到终点

        // 转换为世界坐标（每个节点的中心位置）
        List<Vector3> waypoints = new List<Vector3>();
        foreach (Node node in path)
        {
            waypoints.Add(currentMapNode.GetWorldPosFromNode(node));
        }
        return waypoints;
    }

    // 计算两个节点的距离（曼哈顿距离，确保启发函数可采纳性）
    private float GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        // 曼哈顿距离公式：|x1-x2| + |y1-y2|，乘以10统一成本单位
        return 10 * (dstX + dstY);
    }

    /// <summary>
    /// 清楚当前地图的A*寻路地图信息，在退出游戏时调用
    /// </summary>
    public void Clear()
    {
        currentMapNode = null;
    }

}
