using Cinemachine;
using UnityEngine;

public class GameManager : BaseManager<GameManager>
{
    /// <summary>
    /// 记录玩家的数据，目前只有第一次进入场景的位置
    /// </summary>
    // public PlayerData pData;
    /// <summary>
    /// 记录玩家进入下一个场景的位置
    /// 这里因为脚本调用顺序的原因，第一次进入场景需要额外的设置角色的位置
    /// 但同时第一次进入场景也会触发传送门的场景加载后处理，所以这里可以第一次直接使用赋值的内容
    /// 后面切换场景的时候这个值就会被覆盖
    /// 并且这里很重要的一点，这个场景加载后处理的时机必须要是玩家实例化出来之后
    /// 否则GameManager中的player记录值是空，会报空引用异常
    /// </summary>
    public Vector3 initPos = new Vector3(-5, 3, 0);
    public Vector3 playerPos;
    /// <summary>
    /// 记录Player
    /// </summary>
    public Player player;
    /// <summary>
    /// 记录跟随玩家的摄像机
    /// </summary>
    public CinemachineVirtualCamera playerCamera;

    private GameManager()
    {
        // pData = Resources.Load<PlayerData>("PlayerData/PlayerData");
        playerPos = initPos;
    }

    /// <summary>
    /// 初始化相机相关参数
    /// </summary>
    public void InitCameraValues()
    {
        if (player == null || playerCamera == null) return;
        playerCamera.Follow = player.transform;
        CinemachineConfiner confiner = playerCamera.GetComponent<CinemachineConfiner>();
        confiner.m_BoundingShape2D = GameObject.Find("ViewLimit").GetComponent<PolygonCollider2D>();
    }

    /// <summary>
    /// 初始化玩家身上的数据
    /// </summary>
    public void InitPlayerData()
    {
        player.transform.position = playerPos;
        Debug.Log("上一次记录的玩家位置是" + playerPos);
    }

    /// <summary>
    /// 测试
    /// </summary>
    public void Update()
    {
        #region 测试时间系统
        if (Input.GetKeyDown(KeyCode.J))
        {
            TimeSystem.Instance.SpeedUpOneDay();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            TimeSystem.Instance.SpeedUpOneHour();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            TimeSystem.Instance.SpeedUpThreeHour();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            TimeSystem.Instance.JumpToNextDay();
        }
        #endregion
    }

    public void InitPlayerPos()
    {
        playerPos = initPos;
    }

    public void DestroyObj()
    {
        EventCenter.Instance.Clear();
        GameObject.DestroyImmediate(player.gameObject);
        GameObject.DestroyImmediate(playerCamera.gameObject);
    }

    public void EnablePlayerInput()
    {
        if (player != null)
        {
            player.EnablePlayerInput();
        }
    }

    public void DisablePlayerInput()
    {
        if (player != null)
        {
            player.DisablePlayerInput();
        }
    }

    public void BackToInitPos()
    {
        InitPlayerPos();
        InitPlayerData();
        InitCameraValues();
    }
}
