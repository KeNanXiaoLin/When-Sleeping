using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarTest : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 endPos;
    // public Pathfinding pathfinding;
    public TilemapGrid tilemapGrid;
    private List<Vector3> res;
    public string sceneName = "1";

    void Start()
    {
        // res = AStarMgr.Instance.FindPath(startPos, endPos);
        AStarMapNode mapNode = new AStarMapNode(sceneName, tilemapGrid);
        AStarMgr.Instance.AddMapNodeInfo(sceneName, mapNode);
        GameManager.Instance.currentSceneName = sceneName;
        res = AStarMgr.Instance.FindPath(startPos, endPos);

    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Node startNode = tilemapGrid.GetNodeFromWorldPos(startPos);
            Node endNode = tilemapGrid.GetNodeFromWorldPos(endPos);
            // 绘制每个节点
            // 可通行=白色，障碍物=红色
            Gizmos.color = Color.blue;
            // 绘制节点的立方体（位置为瓦片中心）
            Vector3 sPos = tilemapGrid.GetWorldPosFromNode(startNode);
            Gizmos.DrawCube(sPos, Vector3.one * (tilemapGrid.nodeSize - 0.1f));
            Vector3 ePos = tilemapGrid.GetWorldPosFromNode(endNode);
            Gizmos.DrawCube(ePos, Vector3.one * (tilemapGrid.nodeSize - 0.1f));
            if (res != null && res.Count > 0)
            {
                Gizmos.color = Color.yellow;
                foreach (Vector3 v3 in res)
                {
                    Gizmos.DrawCube(v3, Vector3.one * (tilemapGrid.nodeSize - 0.1f));
                }
            }
        }

    }
}
