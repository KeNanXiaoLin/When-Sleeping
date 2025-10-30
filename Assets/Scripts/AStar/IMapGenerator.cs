using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地图节点生成器的抽象接口，定义节点生成的规范
/// </summary>
public interface IMapGenerator
{
    /// <summary>
    /// 生成A*寻路所需的节点网格
    /// </summary>
    /// <returns>二维节点数组</returns>
    Node[,] GenerateNodes();

    /// <summary>
    /// 将世界坐标转换为网格中的节点
    /// </summary>
    /// <param name="worldPos">世界坐标</param>
    /// <returns>对应的节点</returns>
    Node GetNodeFromWorldPos(Vector3 worldPos);

    /// <summary>
    /// 获取节点的世界坐标
    /// </summary>
    /// <param name="node">节点</param>
    /// <returns>世界坐标</returns>
    Vector3 GetWorldPosFromNode(Node node);

    /// <summary>
    /// 获取节点的相邻节点
    /// </summary>
    /// <param name="node">目标节点</param>
    /// <returns>相邻节点列表</returns>
    List<Node> GetNeighbors(Node node);
}