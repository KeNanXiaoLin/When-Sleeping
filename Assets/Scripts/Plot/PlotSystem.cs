using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotSystem : SingletonAutoMono<PlotSystem>
{
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        EventCenter.Instance.AddEventListener<int>(E_EventType.E_Morning, HandleMorning);
        EventCenter.Instance.AddEventListener<int>(E_EventType.E_Afternoon, HandleAfternoon);
        EventCenter.Instance.AddEventListener<int>(E_EventType.E_Night, HandleNightEvent);
    }

    void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener<int>(E_EventType.E_Morning, HandleMorning);
        EventCenter.Instance.RemoveEventListener<int>(E_EventType.E_Afternoon, HandleAfternoon);
        EventCenter.Instance.RemoveEventListener<int>(E_EventType.E_Night, HandleNightEvent);
    }

    public void Init()
    {

    }

    /// <summary>
    /// 因为第一天只会有玩家有事情需要处理，所以暂时这样写
    /// 后面添加功能可以修改
    /// </summary>
    /// <param name="curData"></param>
    private void HandleMorning(int curData)
    {
        PlayerDialog dialog = GameManager.Instance.player.playerDialog;
        dialog.HandleMorningEvent(curData);
    }

    /// <summary>
    /// 下午触发的剧情
    /// </summary>
    /// <param name="curData"></param>
    private void HandleAfternoon(int curData)
    {
        switch (curData)
        {
            case 1:
                GameManager.Instance.InitPlayerPos();
                //强制把场景切换到场景3，触发剧情
                SceneLoadManager.Instance.LoadScene("GameScene3",PlayClockCG,RefreshPlayerPos, Day2AfternoonPlot);
                break;
        }
    }

    /// <summary>
    /// 晚上触发的剧情
    /// </summary>
    /// <param name="curData"></param>
    private void HandleNightEvent(int curData)
    {
        switch (curData)
        {
            case 0:
                GameManager.Instance.InitPlayerPos();
                //强制把场景切换到场景3，触发剧情
                SceneLoadManager.Instance.LoadScene("GameScene3",PlayClockCG,RefreshPlayerPos, DayNightPlot);
                break;
        }
    }

    private void DayNightPlot()
    {
        //切换背景音乐
        MusicManager.Instance.PlayBKMusic("轻松小曲1");
        //因为要触发剧情，所以禁用玩家的输入
        Player player = GameManager.Instance.player;
        player.DisablePlayerInput();
        //加载剧情触发的角色
        GameObject momObj = Instantiate(Resources.Load<GameObject>("NPC/Mom"), new Vector3(-9.5f, -4f, 0), Quaternion.identity);
        NPC mom = momObj.GetComponent<NPC>();
        mom.Init();
        mom.MoveToMainRole(player.transform, () =>
        {
            DialogData data = mom.FindDialogData(E_DialogTriggerType.OneDayNight);
            if (data != null)
            {
                DialogSystem.Instance.TriggerStartDialog(data);
            }
        });
        EventCenter.Instance.AddEventListener(E_EventType.E_DialogEnd, GetItem);
    }

    public void GetItem()
    {
        EventCenter.Instance.RemoveEventListener(E_EventType.E_DialogEnd, GetItem);
        BagManager.Instance.AddItem(2);
        UIManager.Instance.ShowPanel<TipPanel>((panel) =>
        {
            panel.UpdateInfo("Get A Milk.go to bag to check");
        });
        GameManager.Instance.player.EnablePlayerInput();
    }

    private IEnumerator PlayClockCG()
    {
        yield return CGManager.Instance.PlayClockAnim();
    }

    private void Day2AfternoonPlot()
    {
        //切换背景音乐
        MusicManager.Instance.PlayBKMusic("轻松小曲1");
        //因为要触发剧情，所以禁用玩家的输入
        Player player = GameManager.Instance.player;
        player.DisablePlayerInput();
        //加载剧情触发的角色
        GameObject momObj = Instantiate(Resources.Load<GameObject>("NPC/Bob"), new Vector3(-9.5f, -4f, 0), Quaternion.identity);
        NPC bob = momObj.GetComponent<NPC>();
        bob.Init();
        bob.MoveToMainRole(player.transform, () =>
        {
            DialogData data = bob.FindDialogData(E_DialogTriggerType.SecondDayAfternoon);
            if (data != null)
            {
                DialogSystem.Instance.TriggerStartDialog(data);
            }
        });
        EventCenter.Instance.AddEventListener(E_EventType.E_DialogEnd, GetItem);
    }

    private void RefreshPlayerPos()
    {
        GameManager.Instance.InitPlayerPos();
        GameManager.Instance.InitPlayerData();
        GameManager.Instance.InitCameraValues();
    }
}
