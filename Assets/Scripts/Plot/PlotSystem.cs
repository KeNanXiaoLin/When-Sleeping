using System.Collections;
using System.Collections.Generic;
using KNXL.DialogSystem;
using UnityEngine;

public class PlotSystem : SingletonAutoMono<PlotSystem>
{
    /// <summary>
    /// 在播放剧情的时候需要用到bob
    /// </summary>
    private NPCController bobController;
    void Awake()
    {
        // gameStartDialogData = Resources.Load<RoleDialogData>("PlotData/GameStartDialog");
    }
    private void OnEnable()
    {
        EventCenter.Instance.AddCoroutineListener<int>(E_EventType.E_DialogEnd, CheckPlotCanPlayByDialogID);
        EventCenter.Instance.AddEventListener<string>(E_EventType.E_SceneLoad, CheckPlotCanPlayByChangeScene);
        EventCenter.Instance.AddCoroutineListener<int>(E_EventType.E_PlotDialogStart, CheckPlotStartCanDoSomething);
    }

    void OnDisable()
    {
        EventCenter.Instance.RemoveCoroutineListener<int>(E_EventType.E_DialogEnd, CheckPlotCanPlayByDialogID);
        EventCenter.Instance.RemoveEventListener<string>(E_EventType.E_SceneLoad, CheckPlotCanPlayByChangeScene);
        EventCenter.Instance.RemoveCoroutineListener<int>(E_EventType.E_PlotDialogStart, CheckPlotStartCanDoSomething);
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
            case 10014:
                //在让Bob回家后销毁Bob
                var bobObj = player.transform.Find(Setting.bobName);
                Destroy(bobObj.gameObject);
                break;
            //玩家选择选项之后出现Mom出现让玩家喝牛奶
            case 10015:
                GameObject momPrefab = Resources.Load<GameObject>($"NPC/{Setting.momName}");
                var momObj = GameObject.Instantiate(momPrefab, new Vector3(-5, -3, 0), Quaternion.identity);
                momObj.name = Setting.momName;
                while (Vector2.Distance(player.transform.position, momObj.transform.position) > 1f)
                {
                    Vector2 dir = (player.transform.position - momObj.transform.position).normalized;
                    momObj.transform.Translate(dir * Time.deltaTime);
                    yield return null;
                }
                DialogSystemMgr.Instance.StartPlayDialog(10016, E_DialogPlayType.Plot);
                break;
            case 10006:
                bobController.EnableFollow(GameManager.Instance.player.transform);
                break;
        }
    }

    private void CheckPlotCanPlayByItemID(int itemID)
    {
        switch (itemID)
        {
            case 1:
                break;
        }
    }

    private void CheckPlotCanPlayByChangeScene(string sceneName)
    {
        switch (sceneName)
        {
            case Setting.GameScene1:
                //玩家在切换到场景1的时候，可以播放和Bob对话的剧情，前提是剧情没有被触发过，并且剧情的前置已经解锁
                RoleDialogData plotData = DialogSystemMgr.Instance.GetPlotByID(10006);
                if (!plotData.isTrigger &&
                    DialogSystemMgr.Instance.GetPlotByID(plotData.preRoleDialogs).isTrigger)
                {
                    //实例化一个Bob出来和玩家模拟对话
                    GameObject BobPrefab = Resources.Load<GameObject>($"NPC/{Setting.bobName}");
                    // GameObject.Instantiate(BobPrefab,new Vector3(-3,15.8f,0f),Quaternion.identity);
                    var bobObj = GameObject.Instantiate(BobPrefab, new Vector3(-3, 15.8f, 0f), Quaternion.identity);
                    // bobObj.transform.localPosition = new Vector3(-1, 0, 0);
                    bobObj.name = Setting.bobName;
                    bobController = bobObj.GetComponent<NPCController>();
                    DialogSystemMgr.Instance.StartPlayDialog(10006, E_DialogPlayType.Plot);
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
        switch (dialogId)
        {
            //这里是玩家和电视剧交互完毕后解锁的剧情
            case 10005:
                //这里解锁Bob敲门的剧情
                //播放敲门声
                MusicManager.Instance.PlaySound("按门铃音效6");
                break;
            case 10015:
                //切换到场景3
                yield return SceneLoadManager.Instance.FadeAndLoadScene(Setting.GameScene3, sceneFaderBefore: GameManager.Instance.BackToInitPos);
                break;
        }
    }
}
