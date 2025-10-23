using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEditor.iOS;
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
        private DialogUI dialogNormalUI;
        //对话系统管理的选择对话面板
        private DialogChooseUI dialogChooseUI;

        #region 这里是剧情对话的数据
        private RoleDialogData plotData;
        private DialogData plotSingleData;
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

        /// <summary>
        /// 根据类型得到这种类型对象所拥有的所有对话
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<RoleDialogData> GetDialogDataByType(E_DialogRoleType type)
        {
            List<RoleDialogData> res = new();
            // foreach (var item in allDialogDataDic.Values)
            // {
            //     if (item.owner == type)
            //     {
            //         res.Add(item);
            //     }
            // }
            return res;
        }

        /// <summary>
        /// 根据类型得到这种类型对象所拥有的所有对话
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public RoleDialogData GetDialogDataByType(int id)
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
            ShowDialogUI(E_DialogType.Normal, curDialogData);
        }

        public void TriggerDialog(int id)
        {
            if (!allDialogDataDic.ContainsKey(id))
            {
                Debug.LogError("找不到对话数据，请检查参数传入是否正确");
                return;
            }
            curDialogData = allDialogDataDic[id];
            ShowDialogUI(E_DialogType.Normal, curDialogData);
        }

        /// <summary>
        /// 传入一个角色对话，开始播放这段对话,默认规则第一句话都是NPC的
        /// </summary>
        /// <param name="data"></param>
        public void StartPlayDialog(RoleDialogData data)
        {
            if (data == null || data.startDialog == 0)
            {
                Debug.LogError("不支持播放空对话，请检查对话数据");
                return;
            }
            currentDialogData = data;

            TriggerDialog(data.startDialog);
        }

        /// <summary>
        /// 这种是没有选项的对话，不需要玩家选择就可以手动播放下一句
        /// </summary>
        public void PlayNextDialog()
        {
            //如果当前对话有下一句对话，就播放下一句对话，如果没有，关闭对话窗口
            if (curDialogData.childNodes == 0)
            {
                //关闭面板
                CloseDialogChooseUI();
                CloseDialogNormalUI();
            }
            else
            {
                //根据下一个对话的类型来决定怎么进行播放
                //如果是正常节点，正常播放下一个即可
                //如果是选择节点，那么子节点必然全部是选择节点，进行一个检查，然后播放
                DialogData data = allDialogDataDic[curDialogData.childNodes];
                if (data.dialogType == E_DialogType.Normal)
                {
                    TriggerDialog(data);
                }
                else if (data.dialogType == E_DialogType.Item)
                {
                    //如果是获得道具对话，怎么处理
                }
                else if (data.dialogType == E_DialogType.Effect)
                {
                    //如果是状态变化对话，怎么处理
                }
            }
        }

        /// <summary>
        /// 关闭普通对话窗口
        /// </summary>
        /// <param name="isDestroy">是否销毁，true会销毁面板，false只是禁用面板</param>
        /// <param name="action">面板销毁后调用的委托</param>
        public void CloseDialogNormalUI(bool isDestroy = false, UnityAction action = null)
        {
            if (dialogNormalUI == null) return;
            if (isDestroy)
            {
                GameObject.Destroy(dialogNormalUI.gameObject);
                dialogNormalUI = null;
            }
            else
                dialogNormalUI.gameObject.SetActive(false);
            action?.Invoke();
        }

        /// <summary>
        /// 关闭选择对话窗口
        /// </summary>
        /// <param name="isDestroy">是否销毁，true会销毁面板，false只是禁用面板</param>
        /// <param name="action">面板销毁后调用的委托</param>
        public void CloseDialogChooseUI(bool isDestroy = false, UnityAction action = null)
        {
            if (dialogChooseUI == null) return;
            if (isDestroy)
            {
                GameObject.Destroy(dialogChooseUI.gameObject);
                dialogChooseUI = null;
            }
            else
                dialogChooseUI.gameObject.SetActive(false);
            action?.Invoke();
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
                    if (dialogChooseUI != null)
                    {
                        CloseDialogChooseUI();
                    }
                    //这种情况是没有播放过对话，或者是对话面板销毁的时候
                    if (dialogNormalUI == null)
                    {
                        LoadNormalUI();
                    }
                    //否者可能面板就在界面上，或者隐藏了，我们需要重新显示面板
                    else
                    {
                        ReShowDialogNormalUI();
                    }
                    dialogNormalUI.IsPlot = isPlot;
                    dialogNormalUI.ShowDialog(data);
                    action?.Invoke();
                    break;
            }

        }

        /// <summary>
        /// 封装一下加载普通对话面板资源的方法
        /// </summary>
        private void LoadNormalUI()
        {
            //这里可以替换成自己的资源加载代码
            GameObject prefab = Resources.Load<GameObject>("DialogSystem/UI/DialogUI");
            dialogNormalUI = GameObject.Instantiate(prefab).GetComponent<DialogUI>();
        }

        /// <summary>
        /// 封装一下加载选择对话面板资源的方法
        /// </summary>
        private void LoadChooseUI()
        {
            //这里可以替换成自己的资源加载代码
            GameObject prefab = Resources.Load<GameObject>("DialogSystem/UI/DialogChooseUI");
            dialogChooseUI = GameObject.Instantiate(prefab).GetComponent<DialogChooseUI>();
        }

        /// <summary>
        /// 这里封装一下是为了方便后面可能会修改重现方法
        /// CanvasGroup或者Scale缩放都可以提高性能，现在只是方便测试
        /// </summary>
        private void ReShowDialogNormalUI()
        {
            dialogNormalUI.gameObject.SetActive(true);
        }

        /// <summary>
        /// 这里封装一下是为了方便后面可能会修改重现方法
        /// CanvasGroup或者Scale缩放都可以提高性能，现在只是方便测试
        /// </summary>
        private void ReShowDialogChooseUI()
        {
            dialogChooseUI.gameObject.SetActive(true);
        }

        /// <summary>
        /// 从传入的对话数据中判断当前是否有可以让玩家交互的对话
        /// 这里是方便外部调用，可以在可交互的头顶出现问号等内容
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        // public RoleDialogDataNoSO GetCanInteractiveData(List<RoleDialogDataNoSO> datas)
        // {
        //     //所有可以返回的对话数据，最后根据优先级返回
        //     List<RoleDialogDataNoSO> allCanUseDatas = new();
        //     CalDialogDataID(datas);
        //     foreach (var item in datas)
        //     {
        //         //找到当前对话中没有触发的对话或者可以重复触发的对话，直接返回
        //         //当前没有什么前置条件，所以就这样处理，如果有其他前置条件，添加即可
        //         //判断有没有前置对话
        //         //添加了一个字段Locked，对话内容需要解锁才能返回
        //         if (!item.locked && item.preRoleDialogs.Count != 0)
        //         {
        //             bool flag = true;
        //             //判断前置对话是否全部触发过，如果触发过，能够返回这个对话数据
        //             foreach (var preLog in item.preRoleDialogs)
        //             {
        //                 RoleDialogDataNoSO data = allDialogDataDic[preLog.id];
        //                 if (!data.isTrigger)
        //                 {
        //                     flag = false;
        //                 }
        //             }
        //             if (flag)
        //             {
        //                 allCanUseDatas.Add(item);
        //             }
        //         }
        //         //对话已经被解锁并且没有前置对话并且没有触发或者可以重复触发
        //         else if (!item.locked && (item.preRoleDialogs == null || item.preRoleDialogs.Count == 0)
        //                 && (item.canTriggerRepeat || !item.isTrigger))
        //         {
        //             allCanUseDatas.Add(item);
        //         }
        //     }
        //     if (allCanUseDatas.Count == 0)
        //         return null;
        //     else
        //     {
        //         //降序排序
        //         allCanUseDatas.Sort((a, b) => b.id.CompareTo(a.id));
        //         return allCanUseDatas[0];
        //     }

        // }


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
        public void StartPlayPlotDialog(RoleDialogData data)
        {
            if (data == null || data.startDialog == 0)
            {
                Debug.LogError("不支持播放空对话，请检查对话数据");
                return;
            }
            plotData = data;

            TriggerPlotDialog(plotData.startDialog);
        }
        /// <summary>
        /// 开始播放某段剧情对话
        /// </summary>
        /// <param name="data"></param>
        public void StartPlayPlotDialog(int plotID)
        {
            if (!allRoleDialogDataDic.ContainsKey(plotID))
            {
                Debug.LogError("请检查传入参数，找不到要播放的剧情片段");
            }
            plotData = allRoleDialogDataDic[plotID];

            TriggerPlotDialog(plotData.startDialog);
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
            ShowDialogUI(E_DialogType.Normal, plotSingleData, true);
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
            ShowDialogUI(E_DialogType.Normal, plotSingleData, true);
        }

        /// <summary>
        /// 这种是没有选项的对话，不需要玩家选择就可以手动播放下一句
        /// </summary>
        public void PlayNextPlotDialog()
        {
            //如果当前对话有下一句对话，就播放下一句对话，如果没有，关闭对话窗口
            if (plotSingleData.childNodes == 0)
            {
                //关闭面板
                CloseDialogChooseUI();
                CloseDialogNormalUI();
                //这段剧情播放完毕，不需要再次播放
                plotData = null;
            }
            else
            {
                //根据下一个对话的类型来决定怎么进行播放
                //如果是正常节点，正常播放下一个即可
                //如果是选择节点，那么子节点必然全部是选择节点，进行一个检查，然后播放

                DialogData data = allDialogDataDic[plotSingleData.childNodes];
                if (data.dialogType == E_DialogType.Item)
                {
                    HandleItemDialogData(data);
                }
                else if (data.dialogType == E_DialogType.Normal)
                {
                    TriggerPlotDialog(data);
                }
            }
        }

        public void HandleItemDialogData(DialogData data)
        {

        }
        #endregion
    }
}
