using Cinemachine;
using KNXL.DialogSystem;
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
    public Vector3 initPos  = new MyVector3(-5, 3, 0);
    /// <summary>
    /// 记录玩家进入下一个场景的位置
    /// </summary>
    public Vector3 nextPos;
    public Vector3 playerPos { get { return gameData.playerPos; } set { gameData.playerPos.Set(value); } }
    /// <summary>
    /// 记录Player
    /// </summary>
    public Player player;
    /// <summary>
    /// 记录跟随玩家的摄像机
    /// </summary>
    public CinemachineVirtualCamera playerCamera;
    /// <summary>
    /// 当前玩家所在的场景的名字
    /// </summary>
    public string currentSceneName { get { return gameData.curSceneName; } set { gameData.curSceneName = value; } }
    private string lastSceneName;
    /// <summary>
    /// 因为第一次进入场景是不需要消灭所有敌人的，并且没有更新敌人的数量，所以给一个比较大的初始值即可
    /// </summary>
    private int enemyCount = 10;
    public GameData gameData;
    public SceneData sceneData;
    private SingleSceneData currentSceneData = new SingleSceneData();

    private GameManager()
    {
        ResetData();
        nextPos = initPos;
        EventCenter.Instance.AddEventListener<string>(E_EventType.E_SceneLoad, BindMapNodeInfo);
    }

    ~GameManager()
    {
        EventCenter.Instance.RemoveEventListener<string>(E_EventType.E_SceneLoad, BindMapNodeInfo);
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
        player.transform.position = nextPos;
        Debug.Log("上一次记录的玩家位置是" + nextPos);
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
        nextPos = initPos;
    }

    public void DestroyObj()
    {
        // EventCenter.Instance.Clear();
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

    /// <summary>
    /// 场景切换是后触发的事件
    /// 在场景切换的时候，我们需要保存当前场景的数据到场景数据中
    /// 并且需要加载下一个场景的数据，如果不存在，就是用默认数据
    /// </summary>
    /// <param name="sceneName"></param>
    private void BindMapNodeInfo(string sceneName)
    {
        lastSceneName = currentSceneName;
        currentSceneName = sceneName;
        switch(lastSceneName)
        {
            case Setting.GameScene1:
            case Setting.GameScene2:
            case Setting.GameScene3:
                //存储上一个场景的数据
                sceneData.AddSceneData(lastSceneName,currentSceneData);
                break;
        }
        switch (sceneName)
        {
            case Setting.GameScene1:
            case Setting.GameScene2:
            case Setting.GameScene3:
                //拿到下一个场景的数据
                currentSceneData = sceneData.GetSceneData(currentSceneName);
                if(currentSceneData == null)
                {
                    currentSceneData = new SingleSceneData();
                    currentSceneData.sceneName = currentSceneName;
                }
                // 初始化A*地图节点
                GameObject gridObj = GameObject.FindWithTag("Path");
                TilemapGrid tilemapGrid = gridObj.GetComponent<TilemapGrid>();
                AStarMapNode aStarMapNode = new AStarMapNode(currentSceneName, tilemapGrid);
                AStarMgr.Instance.currentMapNode = aStarMapNode;
                break;
        }
    }

    public void UpdateEnemyCount()
    {
        enemyCount = 6;
    }

    public void MinusEnemyCount()
    {
        enemyCount--;
        if(enemyCount == 0)
        {
            EventCenter.Instance.EventTrigger(E_EventType.E_EnemyZero);
        }
    }

    public void ResetData()
    {
        gameData = SaveSystemMgr.Instance.saveData.gameData;
        sceneData = SaveSystemMgr.Instance.saveData.sceneData;
    }

    /// <summary>
    /// 恢复场景数据
    /// </summary>
    public void RestoreScene()
    {
        SingleSceneData singleSceneData = sceneData.GetSceneData(currentSceneName);
        if (singleSceneData == null)
        {
            return;
        }
        foreach (var item in singleSceneData.npcData)
        {
            NPCController npcController = NPCFactory.Instance.CreateNPC(item.Key,item.Value);
            PlotSystem.Instance.SetNPCController(npcController);
        }
    }

    /// <summary>
    /// 向当前场景中添加一个NPC数据，需要删除其他场景中的相同类型的NPC数据
    /// </summary>
    /// <param name="npcType">NPC类型</param>
    /// <param name="npcData">NPC数据</param>
    public void AddNPCData(E_NPCType npcType,NPCData npcData)
    {
        var curSceneData = currentSceneData;
        if (curSceneData == null)
        {
            Debug.LogError($"场景数据中不存在{currentSceneName}的场景数据");
            return;
        }
        curSceneData.AddNPCData(npcType,npcData);
        sceneData.UpdateSceneData(currentSceneName,curSceneData);
        //删除其他场景中的相同类型的NPC数据
        foreach (var sceneItem in sceneData.sceneData)
        {
            if(sceneItem.Key == currentSceneName) continue;
            sceneItem.Value.RemoveNPCData(npcType);
        }
    }

    public bool TryGetNPCDataInCurrentScene(E_NPCType npcType,out NPCData npcData)
    {
        var curSceneData = sceneData.GetSceneData(currentSceneName);
        if (curSceneData == null)
        {
            npcData = null;
            return false;
        }
        npcData = curSceneData.GetNPCData(npcType);
        return npcData != null;
    }
}
