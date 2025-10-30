using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A*寻路的节点数据类（纯数据容器，与地图生成方式解耦）
/// </summary>
public class AStarMapNode
{
    private Node[,] grid;  // 存储生成的节点网格
    private string mapName;  // 地图名称
    private IMapGenerator mapGenerator;  // 依赖抽象接口，而非具体实现

    // 构造函数：传入地图名称和对应的生成器
    public AStarMapNode(string name, IMapGenerator generator)
    {
        mapName = name;
        mapGenerator = generator;
        // 初始化时通过生成器生成节点网格
        grid = mapGenerator.GenerateNodes();
    }

    // 对外提供节点网格
    public Node[,] GetGrid() => grid;

    // 对外提供“世界坐标转节点”的能力（委托给生成器）
    public Node GetNodeFromWorldPos(Vector3 worldPos)
    {
        return mapGenerator.GetNodeFromWorldPos(worldPos);
    }

    // 对外提供“节点转世界坐标”的能力
    public Vector3 GetWorldPosFromNode(Node node)
    {
        return mapGenerator.GetWorldPosFromNode(node);
    }

    // 对外提供“获取相邻节点”的能力
    public List<Node> GetNeighbors(Node node)
    {
        return mapGenerator.GetNeighbors(node);
    }

    // 可选：动态更新节点（如地图变化时重新生成）
    public void RefreshNodes()
    {
        grid = mapGenerator.GenerateNodes();
    }
}