using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDialog : MonoBehaviour
{
    [SerializeField] private List<DialogData> allDialogs;

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        //在淡出结束的时候，处理开场逻辑
        // EventCenter.Instance.AddEventListener<int>(E_EventType.E_Morning, HandleMorningEvent);

    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        // EventCenter.Instance.RemoveEventListener<int>(E_EventType.E_Morning, HandleMorningEvent);
    }


    void Update()
    {
        CheckIsTriggeredDialog();
    }

    private void CheckIsTriggeredDialog()
    {
        for (int i = allDialogs.Count - 1; i >= 0; i--)
        {
            if (allDialogs[i].isTrigger)
            {
                allDialogs.Remove(allDialogs[i]);
            }
        }
    }

    public void HandleMorningEvent(int curDay)
    {
        DialogData data = null;
        switch (curDay)
        {
            case 0:
                //如果是第一天的早上
                data = FindDialogData(E_DialogTriggerType.OneDayMorning);
                if (data != null)
                {
                    DialogSystem.Instance.TriggerStartDialog(data);
                }
                break;
            case 1:
                //第二天是需要切场景的
                //初始化玩家位置
                GameManager.Instance.InitPlayerPos();
                SceneLoadManager.Instance.LoadScene("GameScene3", ()=>
                {
                    //切换背景音乐
                    MusicManager.Instance.PlayBKMusic("轻松小曲1");
                    BackToLifeScene();
                    data = FindDialogData(E_DialogTriggerType.SecondDayMorning);
                    if (data != null)
                    {
                        DialogSystem.Instance.TriggerStartDialog(data);
                    }
                    EventCenter.Instance.AddEventListener(E_EventType.E_DialogEnd, EnablePlayerInput);
                });
                
                break;
        }
    }

    public DialogData FindDialogData(E_DialogTriggerType type)
    {
        DialogData data = null;
        //先从玩家的对话数据里找到要触发的对话
        for (int i = 0; i < allDialogs.Count; i++)
        {
            //对话没有触发过，并且对话的触发时机正确
            if (!allDialogs[i].isTrigger && allDialogs[i].triggerType == type)
            {
                data = allDialogs[i];
                break;
            }
        }
        return data;
    }

    public void BackToLifeScene()
    {
        //因为要触发剧情，所以禁用玩家的输入
        Player player = GameManager.Instance.player;
        player.DisablePlayerInput();
        //设置玩家的移动方式为Life
        player.EnableLifeAction();
        UIManager.Instance.ShowPanel<GameUI>();
    }

    public void EnablePlayerInput()
    {
        EventCenter.Instance.RemoveEventListener(E_EventType.E_DialogEnd, EnablePlayerInput);
        Player player = GameManager.Instance.player;
        player.EnablePlayerInput();
    }
}
