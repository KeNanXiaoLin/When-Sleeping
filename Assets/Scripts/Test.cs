using Cinemachine;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 一个测试脚本
/// </summary>
public class Test : MonoBehaviour
{
    public Vector3 spawnPos;

    void Awake()
    {
        // DialogSystem.Instance.Test();
    }
    void Start()
    {
        GameObject playerObj = Instantiate(Resources.Load<GameObject>("Player/Player"), spawnPos, Quaternion.identity);
        GameObject playerCamera = Instantiate(Resources.Load<GameObject>("Player/PlayerCamera"));
        CinemachineVirtualCamera camera = playerCamera.GetComponent<CinemachineVirtualCamera>();
        camera.Follow = playerObj.transform;
        CinemachineConfiner confiner = camera.GetComponent<CinemachineConfiner>();
        confiner.m_BoundingShape2D = GameObject.Find("ViewLimit").GetComponent<PolygonCollider2D>();
        UIManager.Instance.ShowPanel<GameUI>();

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            BagManager.Instance.AddItem(1, 1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            BagManager.Instance.AddItem(2, 1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            BagManager.Instance.AddItem(3, 1);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            BagManager.Instance.ShowBagInfo();
        }
    }
}
