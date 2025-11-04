using System.Collections;
using System.Collections.Generic;
using KNXL.DialogSystem;
using UnityEngine;
using UnityEngine.UI;

public class PlotSystem : SingletonAutoMono<PlotSystem>
{
    /// <summary>
    /// 在播放剧情的时候需要用到bob
    /// </summary>
    private NPCController bobController;
    /// <summary>
    /// 在播放剧情的时候需要用到mom
    /// </summary>
    private NPCController momController;
    void Awake()
    {
        // gameStartDialogData = Resources.Load<RoleDialogData>("PlotData/GameStartDialog");
    }
    private void OnEnable()
    {
        EventCenter.Instance.AddCoroutineListener<int>(E_EventType.E_DialogEnd, CheckPlotCanPlayByDialogID);
        EventCenter.Instance.AddEventListener<string>(E_EventType.E_SceneLoad, CheckPlotCanPlayByChangeScene);
        EventCenter.Instance.AddCoroutineListener<int>(E_EventType.E_PlotDialogStart, CheckPlotStartCanDoSomething);
        EventCenter.Instance.AddCoroutineListener<int>(E_EventType.E_SpecialDialogPlay, CheckSpecialDialogPlayCanDoSomething);
        EventCenter.Instance.AddCoroutineListener<int>(E_EventType.E_BagAddItem, CheckPlotCanPlayByItemID);
    }

    void OnDisable()
    {
        EventCenter.Instance.RemoveCoroutineListener<int>(E_EventType.E_DialogEnd, CheckPlotCanPlayByDialogID);
        EventCenter.Instance.RemoveEventListener<string>(E_EventType.E_SceneLoad, CheckPlotCanPlayByChangeScene);
        EventCenter.Instance.RemoveCoroutineListener<int>(E_EventType.E_PlotDialogStart, CheckPlotStartCanDoSomething);
        EventCenter.Instance.RemoveCoroutineListener<int>(E_EventType.E_SpecialDialogPlay, CheckSpecialDialogPlayCanDoSomething);
        EventCenter.Instance.RemoveCoroutineListener<int>(E_EventType.E_BagAddItem, CheckPlotCanPlayByItemID);
    }

    public void PlayGameStartDialog()
    {
        DialogSystemMgr.Instance.StartPlayDialog(10001, E_DialogPlayType.Plot, GameManager.Instance.EnablePlayerInput);
    }

    /// <summary>
    /// 检测因为对话播放而解锁的剧情事件
    /// </summary>
    /// <param name="dialogId"></param>
    private IEnumerator CheckPlotCanPlayByDialogID(int dialogId)
    {
        Player player = GameManager.Instance.player;
        //这个对话播放完毕，看是否有能够解锁的剧情对话
        DialogSystemMgr.Instance.UnLockDialogByPreID(dialogId);
        switch (dialogId)
        {
            //这里是玩家触发了Bob敲门的剧情，可以让玩家出门了
            case 10005:
                RoleDialogData plotData = DialogSystemMgr.Instance.GetPlotByID(10037);
                DisableNormalRoleDialog(plotData);
                break;
            //旁白提示玩家回家，这个时候可以让Bob回家，可以让玩家正常从院子回家
            case 10014:
                bobController.BackToHome();
                plotData = DialogSystemMgr.Instance.GetPlotByID(10038);
                DisableNormalRoleDialog(plotData);
                break;
            //旁白提示玩家回家，这个时候可以让Bob回家，可以让玩家正常从院子回家
            case 10028:
                bobController.BackToHome();
                plotData = DialogSystemMgr.Instance.GetPlotByID(10039);
                DisableNormalRoleDialog(plotData);
                break;
            //玩家选择选项之后出现Mom出现让玩家喝牛奶
            case 10015:
                player.UpdatePlayerFacing(E_Direction.Down);
                SpawnMom(new Vector3(-5, -3, 0));
                momController.GotoTargetPos(player.transform.position + new Vector3(0, -1, 0));
                while (Vector2.Distance(player.transform.position, momController.transform.position) > 1f)
                {
                    yield return null;
                }
                DialogSystemMgr.Instance.StartPlayDialog(10016, E_DialogPlayType.Plot, () =>
                {
                    UIManager.Instance.ShowPanel<TipPanel>((panel) =>
                    {
                        panel.UpdateInfo("你喝下了牛奶");
                        panel.AddOKEvent(() =>
                        {
                            //这里应该是切换到战斗场景，但是先把剧情做完，所以这里直接到第二天的剧情
                            // SceneLoadManager.Instance.LoadScene("BattleScene", sceneFaderBefore: player.InitBattleInfo);
                            DialogSystemMgr.Instance.StartPlayDialog(10018, E_DialogPlayType.Plot);
                        });
                    })
                    ;
                });
                break;
            case 10006:
                bobController.EnableFollow(GameManager.Instance.player.transform);
                break;
                //这是Mom禁止白天喝牛奶的剧情，结束之后可以去院子
            case 10020:
                plotData = DialogSystemMgr.Instance.GetPlotByID(10041);
                DisableNormalRoleDialog(plotData);
                break;
            //两人商量将牛奶给猫喝
            case 10021:
                //将前置对话给关闭了，播放新的对话
                plotData = DialogSystemMgr.Instance.GetPlotByID(10007);
                DisableNormalRoleDialog(plotData);
                plotData = DialogSystemMgr.Instance.GetPlotByID(10040);
                DisableNormalRoleDialog(plotData);
                bobController.EnableFollow(GameManager.Instance.player.transform);
                break;
            case 10022:
                DialogSystemMgr.Instance.StartPlayDialog(10023, E_DialogPlayType.Plot);
                break;
            //这个时候播放完毕Mike和Mom的对话，应该切换到Bob视角
            case 10029:
                yield return SceneLoadManager.Instance.FadeAndLoadScene(Setting.GameScene1);
                yield return SimulateBobAction();
                break;
            case 10030:
                DialogSystemMgr.Instance.StartPlayDialog(10031, E_DialogPlayType.Plot);
                yield return null;
                break;
            //播放完Bob视角，回到主角视角
            case 10031:
                yield return SceneLoadManager.Instance.FadeAndLoadScene(Setting.GameScene3, sceneFaderBefore: GameManager.Instance.BackToInitPos);
                yield return BackToPlayerView();
                break;
        }
    }

    private IEnumerator CheckPlotCanPlayByItemID(int itemID)
    {
        Player player = GameManager.Instance.player;
        switch (itemID)
        {
            // 拿到了牛奶
            case 2:
                //必须要在场景2
                if (GameManager.Instance.currentSceneName == Setting.GameScene2)
                {
                    SpawnMom(new Vector3(-1, -5, 0));
                    momController.GotoTargetPos(player.transform.position + new Vector3(0, 1, 0));
                    while (Vector2.Distance(player.transform.position, momController.transform.position) > 1f)
                    {
                        yield return null;
                    }
                    player.UpdatePlayerFacing(E_Direction.Up);
                    momController.SetNPCFacing(E_Direction.Down);
                    DialogSystemMgr.Instance.StartPlayDialog(10020, E_DialogPlayType.Plot);
                }
                break;
        }
    }

    private void CheckPlotCanPlayByChangeScene(string sceneName)
    {
        Player player = GameManager.Instance.player;
        switch (sceneName)
        {
            case Setting.GameScene1:
                //在被Mom阻止喝牛奶后再次回到场景一
                RoleDialogData plotData = DialogSystemMgr.Instance.GetPlotByID(10021);
                if (!plotData.isTrigger &&
                    DialogSystemMgr.Instance.GetPlotByID(plotData.preRoleDialogs).isTrigger)
                {
                    player.UpdatePlayerFacing(E_Direction.Left);
                    SpawnBob(player.transform.position + Vector3.left);
                    DialogSystemMgr.Instance.StartPlayDialog(10021, E_DialogPlayType.Plot);
                    return;
                }
                //这是第二天在院子里的对话
                plotData = DialogSystemMgr.Instance.GetPlotByID(10019);
                if (!plotData.isTrigger &&
                    DialogSystemMgr.Instance.GetPlotByID(plotData.preRoleDialogs).isTrigger)
                {
                    player.UpdatePlayerFacing(E_Direction.Left);
                    SpawnBob(player.transform.position + Vector3.left);
                    DialogSystemMgr.Instance.StartPlayDialog(10019, E_DialogPlayType.Plot);
                    return;
                }
                //玩家在切换到场景1的时候，可以播放和Bob对话的剧情，前提是剧情没有被触发过，并且剧情的前置已经解锁
                plotData = DialogSystemMgr.Instance.GetPlotByID(10006);
                if (!plotData.isTrigger &&
                    DialogSystemMgr.Instance.GetPlotByID(plotData.preRoleDialogs).isTrigger)
                {
                    player.UpdatePlayerFacing(E_Direction.Left);
                    SpawnBob(player.transform.position + Vector3.left);
                    DialogSystemMgr.Instance.StartPlayDialog(10006, E_DialogPlayType.Plot);
                }
                break;
            case Setting.GameScene2:
                plotData = DialogSystemMgr.Instance.GetPlotByID(10019);
                //这个对话必须触发才会在桌上出现牛奶
                if (plotData.isTrigger)
                {
                    SpawnBob(new Vector3(-1, -5f, 0f));
                    bobController.EnableFollow(player.transform);
                    GameObject milkObj = GameObject.Instantiate(Resources.Load<GameObject>("Item/ItemPrefab"));
                    milkObj.transform.position = new Vector3(2.7f, 1.2f, 0);
                    Item milk = milkObj.GetComponent<Item>();
                    milk.Init(BagManager.Instance.GetBagItemByItemID(2));
                }
                break;
        }
    }

    /// <summary>
    /// 在某段剧情触发前做的事情
    /// </summary>
    /// <param name="dialogId"></param>
    private IEnumerator CheckPlotStartCanDoSomething(int dialogId)
    {
        Player player = GameManager.Instance.player;
        switch (dialogId)
        {
            //这里是玩家和电视剧交互完毕后解锁的剧情
            case 10005:
                //这里解锁Bob敲门的剧情
                //播放敲门声
                MusicManager.Instance.PlaySound("按门铃音效6");
                break;
            case 10015:
            case 10018:
                //切换到场景3
                yield return SceneLoadManager.Instance.FadeAndLoadScene(Setting.GameScene3, sceneFaderBefore: GameManager.Instance.BackToInitPos);
                break;
            case 10029:
                //切换到场景3
                player.UpdatePlayerFacing(E_Direction.Down);
                yield return SceneLoadManager.Instance.FadeAndLoadScene(Setting.GameScene3, sceneFaderBefore: GameManager.Instance.BackToInitPos);
                SpawnMom(new Vector3(-5, -3, 0));
                momController.GotoTargetPos(player.transform.position + new Vector3(0, -1, 0));
                while (Vector2.Distance(player.transform.position, momController.transform.position) > 1f)
                {
                    yield return null;
                }
                break;
            case 10023:
                //喂小猫喝牛奶，需要使用牛奶
                BagManager.Instance.RemoveItem(2);
                break;
                //这里是Bob睡不着，出门走走的剧情
            case 10031:
                yield return SimulateBobGoOutSide();
                break;
        }
    }

    /// <summary>
    /// 检测特殊的对话播放的时候可以做什么事情
    /// </summary>
    /// <param name="dialogId"></param>
    /// <returns></returns>
    private IEnumerator CheckSpecialDialogPlayCanDoSomething(int dialogId)
    {
        switch (dialogId)
        {
            case 10803:
                MusicManager.Instance.PlaySound("按门铃音效6");
                yield return null;
                break;
        }
    }

    /// <summary>
    /// 在当前场景中产生一个Bob
    /// </summary>
    private void SpawnBob(Vector3 bobPos)
    {
        //实例化一个Bob出来和玩家模拟对话
        GameObject BobPrefab = Resources.Load<GameObject>($"NPC/{Setting.bobName}");
        // GameObject.Instantiate(BobPrefab,new Vector3(-3,15.8f,0f),Quaternion.identity);
        var bobObj = GameObject.Instantiate(BobPrefab, bobPos, Quaternion.identity);
        // bobObj.transform.localPosition = new Vector3(-1, 0, 0);
        bobObj.name = Setting.bobName;
        bobController = bobObj.GetComponent<NPCController>();
    }

    private void SpawnMom(Vector3 momPos)
    {
        GameObject momPrefab = Resources.Load<GameObject>($"NPC/{Setting.momName}");
        var momObj = GameObject.Instantiate(momPrefab, momPos, Quaternion.identity);
        momObj.name = Setting.momName;
        momController = momObj.GetComponent<NPCController>();
    }

    /// <summary>
    /// 关闭在场景中放置的可以正常触发的对话
    /// </summary>
    /// <param name="plotData"></param>
    private void DisableNormalRoleDialog(RoleDialogData plotData)
    {
        plotData.canTriggerRepeat = false;
        plotData.isTrigger = true;
    }

    /// <summary>
    /// 模拟Bob的行为
    /// </summary>
    /// <returns></returns>
    private IEnumerator SimulateBobAction()
    {
        //首先要把玩家给禁用了
        Player player = GameManager.Instance.player;
        GameManager.Instance.InitCameraValues();
        player.DisablePlayerInput();
        player.gameObject.SetActive(false);
        SpawnBob(Vector3.zero);
        bobController.transform.position = bobController.homePos;
        //设置相机跟随为bob
        GameManager.Instance.playerCamera.Follow = bobController.transform;
        //模拟Bob踱步
        Vector3 firstPos = bobController.homePos + Vector2.right * 5;
        bobController.GotoTargetPos(firstPos);
        while (Vector2.Distance(firstPos, bobController.transform.position) > 1f)
        {
            yield return null;
        }
        Vector3 secondPos = bobController.homePos + Vector2.left * 5;
        bobController.GotoTargetPos(secondPos);
        while (Vector2.Distance(secondPos, bobController.transform.position) > 1f)
        {
            yield return null;
        }
        Vector3 thirdPos = bobController.homePos;
        bobController.GotoTargetPos(thirdPos);
        while (Vector2.Distance(thirdPos, bobController.transform.position) > 1f)
        {
            yield return null;
        }
        //播放关于bob的对话
        DialogSystemMgr.Instance.StartPlayDialog(10030, E_DialogPlayType.Plot);
    }

    private IEnumerator SimulateBobGoOutSide()
    {
        Vector3 firstPos = bobController.homePos + Vector2.down * 10;
        bobController.GotoTargetPos(firstPos);
        while (Vector2.Distance(firstPos, bobController.transform.position) > 1f)
        {
            yield return null;
        }
        Vector3 secondPos = bobController.transform.position + Vector3.right * 20;
        bobController.GotoTargetPos(secondPos);
        while (Vector2.Distance(secondPos, bobController.transform.position) > 1f)
        {
            yield return null;
        }
        Vector3 thirdPos = bobController.transform.position + Vector3.up * 10;
        bobController.GotoTargetPos(thirdPos);
        while (Vector2.Distance(thirdPos, bobController.transform.position) > 1f)
        {
            yield return null;
        }
    }
    
    private IEnumerator BackToPlayerView()
    {
        Player player = GameManager.Instance.player;
        player.gameObject.SetActive(true);
        GameManager.Instance.InitPlayerPos();
        player.EnablePlayerInput();
        yield return null;
    }
}

