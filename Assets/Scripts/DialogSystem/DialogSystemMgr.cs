using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;

namespace KNXL.DialogSystem
{
    public class DialogSystemMgr
    {
        public Transform parentTransform;
        private static DialogSystemMgr _instance;
        public static DialogSystemMgr Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new();
                }
                return _instance;
            }
        }
        //存储所有的对话数据
        private Dictionary<int, DialogData> allDialogDataDic;
        //存储所有角色的对话数据
        private Dictionary<int, RoleDialogData> allRoleDialogDataDic;
        //记录当前对话数据
        private RoleDialogData currentDialogData;
        //记录当前正在播放的对话数据
        private DialogData curDialogData;
        //对话系统管理的普通对话面板
        // private DialogUI dialogNormalUI;
        //对话系统管理的选择对话面板
        // private DialogChooseUI dialogChooseUI;
        //普通对话播放结束的委托
        private UnityAction normalPlayEndAction;

        #region 这里是剧情对话的数据
        private RoleDialogData plotData;
        private DialogData plotSingleData;
        private UnityAction plotPlayEndAction;
        #endregion

        private DialogSystemMgr()
        {
            string content = File.ReadAllText(Application.streamingAssetsPath + "/DialogDatas.json");
            List<DialogData> datas = JsonConvert.DeserializeObject<List<DialogData>>(content);
            content = File.ReadAllText(Application.streamingAssetsPath + "/RoleDialogDatas.json");
            List<RoleDialogData> datas1 = JsonConvert.DeserializeObject<List<RoleDialogData>>(content);
            Debug.Log(JsonConvert.SerializeObject(datas1));
            allDialogDataDic = new();
            allRoleDialogDataDic = new();
            foreach (var item in datas)
            {
                if (!allDialogDataDic.ContainsKey(item.id))
                {
                    allDialogDataDic.Add(item.id, item);
                }
                else
                {
                    Debug.LogError("存在id相同的数据，请检查配置文件");
                }
            }
            foreach (var item in datas1)
            {
                if (!allRoleDialogDataDic.ContainsKey(item.id))
                {
                    allRoleDialogDataDic.Add(item.id, item);
                }
                else
                {
                    Debug.LogError("存在id相同的数据，请检查配置文件");
                }
            }
            //这里事件中心监听解锁对话事件
            // EventCenter.Instance.AddEventListener(E_EventType.UnlockDialogData, UnLockedDialogData);
            //对话管理器初始化的时候调用一次,初始化所有的对话数据
            // UnLockedDialogData();
        }

        /// <summary>
        /// 因为这里要把对话单独拎出来做模块，所以直接用这个管理器管理UI
        /// 这里使用Resource动态加载UI，如果有资源加载框架，可以更换成自己的资源加载代码
        /// </summary>
        public void Init()
        {

        }

        public RoleDialogData GetPlotByID(int id)
        {
            if (allRoleDialogDataDic.ContainsKey(id))
            {
                return allRoleDialogDataDic[id];
            }
            else
            {
                Debug.LogError("请传入正确的ID");
                return null;
            }
        }

        /// <summary>
        /// 根据类型得到这种类型对象所拥有的所有对话
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<RoleDialogData> GetDialogDataByType(E_DialogRoleType type)
        {
            List<RoleDialogData> res = new();
            foreach (var item in allRoleDialogDataDic.Values)
            {
                if (item.owner == type)
                {
                    res.Add(item);
                }
            }
            return res;
        }

        /// <summary>
        /// 根据类型得到这种类型对象所拥有的所有对话
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public RoleDialogData GetDialogDataByID(int id)
        {
            if (allRoleDialogDataDic.ContainsKey(id))
            {
                return allRoleDialogDataDic[id];
            }
            return null;
        }

        /// <summary>
        /// 触发对话
        /// </summary>
        /// <param name="npcRoleData">是和哪个npc触发的对话</param>
        /// <param name="dialogId">触发的对话id</param>
        public void TriggerDialog(DialogData data)
        {
            if (data == null)
            {
                Debug.LogError("不支持播放空对话，请检查对话数据");
                return;
            }
            curDialogData = data;
            ShowDialogUI(data.dialogType, curDialogData);
        }

        public void TriggerDialog(int id)
        {
            if (!allDialogDataDic.ContainsKey(id))
            {
                Debug.LogError("找不到对话数据，请检查参数传入是否正确");
                return;
            }
            curDialogData = allDialogDataDic[id];
            TriggerDialog(curDialogData);
        }

        /// <summary>
        /// 传入一个角色对话，开始播放这段对话,默认规则第一句话都是NPC的
        /// </summary>
        /// <param name="data">要播放的对话数据</param>
        /// <param name="action">对话播放结束执行的委托</param>
        public void StartPlayNormalDialog(RoleDialogData data, UnityAction action = null)
        {
            if (data == null || data.startDialog == 0)
            {
                Debug.LogError("不支持播放空对话，请检查对话数据");
                return;
            }
            //如果对话会触发，那么禁用玩家的输入
            if (GameManager.Instance.player)
                GameManager.Instance.player.DisablePlayerInput();
            else
            {
                Debug.LogError("这个时候玩家都没有，怎么开始触发剧情了");
                return;
            }
            //普通的这种交互对话需要进行特殊的判断
            //如果没有触发过，或者已经触发过但是可以重复触发，才进行播放对话
            if (!data.isLocked && (!data.isTrigger || data.isTrigger && data.canTriggerRepeat))
            {
                normalPlayEndAction += action;
                currentDialogData = data;

                TriggerDialog(data.startDialog);
            }

        }

        /// <summary>
        /// 这种是没有选项的对话，不需要玩家选择就可以手动播放下一句
        /// </summary>
        public void PlayNextDialog()
        {
            //如果当前对话有下一句对话，就播放下一句对话，如果没有，关闭对话窗口
            if (curDialogData.childNodes == 0)
            {
                NormalDialogPlayEnd();
            }
            else
            {
                //根据下一个对话的类型来决定怎么进行播放
                //如果是正常节点，正常播放下一个即可
                //如果是选择节点，那么子节点必然全部是选择节点，进行一个检查，然后播放
                DialogData data = allDialogDataDic[curDialogData.childNodes];
                TriggerDialog(data);
            }
        }

        private void NormalDialogPlayEnd()
        {
            //关闭面板
            CloseAllDialogPanel();
            //分发这个对话触发完毕的事件，因为电视，相框这些东西可以重复交互，但是我只希望它分发一次事件即可
            //所以这里需要加条件
            //如果是第一次触发，那么分发事件，设置为触发过，下次进来就不会在分发这里的事件了
            if (!currentDialogData.isTrigger)
            {
                EventCenter.Instance.EventTrigger<int>(E_EventType.E_DialogEnd, currentDialogData.id);
                currentDialogData.isTrigger = true;
            }
            normalPlayEndAction?.Invoke();
            normalPlayEndAction = null;
            currentDialogData = null;
            //恢复玩家输入，这时候一定有玩家的，如果没有，在播放阶段就会报错
            GameManager.Instance.player.EnablePlayerInput();
        }

        private void CloseAllDialogPanel()
        {
            UIManager.Instance.HidePanel<DialogChooseUI>();
            UIManager.Instance.HidePanel<DialogUI>();
            UIManager.Instance.HidePanel<DialogInfoPanel>();
        }
        /// <summary>
        /// 这里是打开UI面板的方法，仅供内部调用
        /// </summary>
        /// <param name="action">在这段对话播放完毕后调用的委托</param>
        private void ShowDialogUI(E_DialogType type, DialogData data = null, bool isPlot = false, UnityAction action = null)
        {
            switch (type)
            {
                case E_DialogType.Normal:
                    //同一时间只能显示一个对话面板，所以播放这一个面板，需要把另一个销毁
                    //暂时不用这种设计
                    // if (dialogChooseUI != null)
                    // {
                    //     CloseDialogChooseUI();
                    // }
                    //这种情况是没有播放过对话，或者是对话面板销毁的时候
                    UIManager.Instance.ShowPanel<DialogUI>((panel) =>
                    {
                        panel.IsPlot = isPlot;
                        panel.ShowDialog(data);
                        action?.Invoke();
                    });
                    break;
                case E_DialogType.Item:
                    BagManager.Instance.AddItem(data.itemID);
                    break;
                case E_DialogType.Choose:
                    break;
                case E_DialogType.Effect:
                    UIManager.Instance.ShowPanel<DialogUI>((panel) =>
                    {
                        panel.IsPlot = isPlot;
                        panel.ShowDialog(data);
                        action?.Invoke();
                        GameManager.Instance.player.statusData.ChangeSan(data.effectSize);
                        // UIManager.Instance.ShowPanel<TipPanel>((panel) =>
                        // {
                        //     if (data.effectSize > 0)
                        //     {
                        //         panel.UpdateInfo($"玩家San值增加{data.effectSize}");
                        //     }
                        //     else if (data.effectSize < 0)
                        //     {
                        //         panel.UpdateInfo($"玩家San值减少{-data.effectSize}");
                        //     }
                        // });
                    });

                    break;
                case E_DialogType.Info:
                    //暂时就把它显示出来并且设置内容
                    UIManager.Instance.ShowPanel<DialogInfoPanel>((panel) =>
                    {
                        panel.ShowDialog(data);
                    });
                    break;
            }

        }

        /// <summary>
        /// 这里封装一下是为了方便后面可能会修改重现方法
        /// CanvasGroup或者Scale缩放都可以提高性能，现在只是方便测试
        /// </summary>
        private void ReShowDialogNormalUI()
        {
            // dialogNormalUI.gameObject.SetActive(true);
            // 因为没有做隐藏逻辑，所以这里也不需要了
        }

        /// <summary>
        /// 这里封装一下是为了方便后面可能会修改重现方法
        /// CanvasGroup或者Scale缩放都可以提高性能，现在只是方便测试
        /// </summary>
        private void ReShowDialogChooseUI()
        {
            // dialogChooseUI.gameObject.SetActive(true);
            // 因为没有做隐藏逻辑，所以这里也不需要了
        }

        public void UnLockedDialogData(UnlockDialogData data)
        {
            //1.读取本地的对话的配置文件
            //2.根据本地的配置文件和传入的数据进行对比，看那个数据满足了，就解锁这个对话数据
        }

        #region 这里是播放剧情对话相关的内容
        /// <summary>
        /// 开始播放某段剧情对话
        /// </summary>
        /// <param name="data"></param>
        public void StartPlayPlotDialog(RoleDialogData data, UnityAction action = null)
        {
            if (data == null || data.startDialog == 0)
            {
                Debug.LogError("不支持播放空对话，请检查对话数据");
                return;
            }
            //如果对话会触发，那么禁用玩家的输入
            if (GameManager.Instance.player)
                GameManager.Instance.player.DisablePlayerInput();
            else
            {
                Debug.LogError("这个时候玩家都没有，怎么开始触发剧情了");
                return;
            }
            //剧情对话没有触发过，才会触发一次，不会多次触发
            if (!data.isLocked && !data.isTrigger)
            {
                plotData = data;
                plotPlayEndAction += action;

                TriggerPlotDialog(plotData.startDialog);
            }
            else
            {
                Debug.LogError("你正在尝试多次触发剧情，请检查代码");
            }

        }
        /// <summary>
        /// 开始播放某段剧情对话
        /// </summary>
        /// <param name="data"></param>
        public void StartPlayPlotDialog(int plotID, UnityAction action = null)
        {
            if (!allRoleDialogDataDic.ContainsKey(plotID))
            {
                Debug.LogError("请检查传入参数，找不到要播放的剧情片段");
            }
            plotData = allRoleDialogDataDic[plotID];

            StartPlayPlotDialog(plotData, action);
        }
        /// <summary>
        /// 开始播放某一段具体的对话
        /// </summary>
        /// <param name="data"></param>
        private void TriggerPlotDialog(DialogData data)
        {
            if (data == null)
            {
                Debug.LogError("不支持播放空对话，请检查对话数据");
                return;
            }
            plotSingleData = data;
            ShowDialogUI(plotSingleData.dialogType, plotSingleData, true);
        }

        /// <summary>
        /// 开始播放某一段具体的对话
        /// </summary>
        /// <param name="data"></param>
        private void TriggerPlotDialog(int id)
        {
            if (!allDialogDataDic.ContainsKey(id))
            {
                Debug.LogError("找不到对话数据，请检查参数传入是否正确");
                return;
            }
            plotSingleData = allDialogDataDic[id];
            ShowDialogUI(plotSingleData.dialogType, plotSingleData, true);
        }

        /// <summary>
        /// 这种是没有选项的对话，不需要玩家选择就可以手动播放下一句
        /// </summary>
        public void PlayNextPlotDialog()
        {
            //如果当前对话有下一句对话，就播放下一句对话，如果没有，关闭对话窗口
            if (plotSingleData.childNodes == 0)
            {
                PlotDialogPlayEnd();
            }
            else
            {
                //根据下一个对话的类型来决定怎么进行播放
                //如果是正常节点，正常播放下一个即可
                //如果是选择节点，那么子节点必然全部是选择节点，进行一个检查，然后播放

                DialogData data = allDialogDataDic[plotSingleData.childNodes];
                TriggerPlotDialog(data);
            }
        }

        private void PlotDialogPlayEnd()
        {
            //关闭面板
            CloseAllDialogPanel();
            //这段剧情播放完毕，不需要再次播放
            //剧情内容只会触发一次，所以直接分发事件即可
            EventCenter.Instance.EventTrigger<int>(E_EventType.E_DialogEnd, plotData.id);
            plotData.isTrigger = true;
            plotData = null;
            plotPlayEndAction?.Invoke();
            plotPlayEndAction = null;
            //恢复玩家输入，这时候一定有玩家的，如果没有，在播放阶段就会报错
            GameManager.Instance.player.EnablePlayerInput();
        }

        public void HandleItemDialogData(DialogData data)
        {

        }
        /// <summary>
        /// 解锁指定前置剧情id的剧情
        /// </summary>
        /// <param name="id"></param>
        public void UnLockDialogByPreID(int id)
        {
            foreach (var item in allRoleDialogDataDic.Values)
            {
                //找到指定前置剧情的ID，解锁他
                if(item.preRoleDialogs == id)
                {
                    if (item.isLocked)
                    {
                        item.isLocked = false;
                    }
                    else
                    {
                        Debug.LogError("这个剧情已经解锁了，不要重复解锁");
                    }
                }
            }
        }
        #endregion
    }
}
