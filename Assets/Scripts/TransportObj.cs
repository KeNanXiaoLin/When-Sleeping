using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportObj : MonoBehaviour
{
    public string targetSceneName;
    public Vector3 targetScenePos;

    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener(E_EventType.E_SceneLoadFaderBefore, InitPlayerInfo);
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener(E_EventType.E_SceneLoadFaderBefore, InitPlayerInfo);
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();
            //禁用玩家的输入
            player.DisablePlayerInput();
            //记录玩家进入下一个场景的位置
            GameManager.Instance.playerPos = targetScenePos;
            //切换场景
            SceneLoadManager.Instance.LoadScene(targetSceneName);
        }
    }

    private void InitPlayerInfo()
    {
        //初始化玩家位置信息，摄像机信息
        GameManager.Instance.InitPlayerData();
        GameManager.Instance.InitCameraValues();
        //启用玩家的输入
        GameManager.Instance.player.EnablePlayerInput();
    }
}
