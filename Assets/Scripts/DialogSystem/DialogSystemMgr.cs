using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;

namespace KNXL.DialogSystem
{
    // 新增：统一对话类型枚举（区分普通/剧情）
    public enum E_DialogPlayType
    {
        Normal,   // 普通对话（支持重复触发）
        Plot      // 剧情对话（仅触发一次）
    }

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

        // 存储所有的对话数据
        private Dictionary<int, DialogData> allDialogDataDic;
        // 存储所有角色的对话数据
        private Dictionary<int, RoleDialogData> allRoleDialogDataDic;
        // 记录当前对话数据（合并普通/剧情的当前数据）
        private RoleDialogData currentRoleDialogData;
        // 记录当前正在播放的单条对话数据
        private DialogData currentSingleDialogData;
        // 对话播放结束的委托（统一回调）
        private UnityAction dialogPlayEndAction;
        // 记录当前对话类型（区分普通/剧情）
        private E_DialogPlayType currentPlayType;

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
        }

        public void Init() { }

        // 保留原有查询方法（无修改）
        public RoleDialogData GetPlotByID(int id)
        {
            if (allRoleDialogDataDic.ContainsKey(id))
            {
                return allRoleDialogDataDic[id];
            }
            Debug.LogError("请传入正确的ID");
            return null;
        }

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

        public RoleDialogData GetDialogDataByID(int id)
        {
            return allRoleDialogDataDic.TryGetValue(id, out var data) ? data : null;
        }

        // 保留原有单条对话触发（无修改）
        public void TriggerDialog(DialogData data)
        {
            if (data == null)
            {
                Debug.LogError("不支持播放空对话，请检查对话数据");
                return;
            }
            currentSingleDialogData = data;
            ShowDialogUI(data.dialogType, currentSingleDialogData);
        }

        public void TriggerDialog(int id)
        {
            if (allDialogDataDic.TryGetValue(id, out var data))
            {
                TriggerDialog(data);
            }
            else
            {
                Debug.LogError("找不到对话数据，请检查参数传入是否正确");
            }
        }

        #region 核心修改：合并普通/剧情对话播放方法
        /// <summary>
        /// 统一播放对话（支持普通/剧情类型）
        /// </summary>
        /// <param name="dialogID">对话ID</param>
        /// <param name="playType">对话类型（普通/剧情）</param>
        /// <param name="endAction">播放结束回调</param>
        public void StartPlayDialog(int dialogID, E_DialogPlayType playType, UnityAction endAction = null)
        {
            // 1. 校验对话数据
            if (!allRoleDialogDataDic.TryGetValue(dialogID, out var roleDialogData) || roleDialogData.startDialog == 0)
            {
                Debug.LogError($"找不到对话数据或对话配置错误（ID：{dialogID}）");
                return;
            }

            // 2. 校验玩家存在
            if (GameManager.Instance.player == null)
            {
                Debug.LogError("玩家未初始化，无法触发对话");
                return;
            }

            // 3. 校验触发条件（核心差异点）
            if (!CheckCanPlayDialog(roleDialogData, playType))
            {
                Debug.LogError($"对话无法触发（ID：{dialogID}，类型：{playType}）");
                return;
            }

            // 4. 初始化对话状态（统一赋值）
            currentRoleDialogData = roleDialogData;
            currentPlayType = playType;
            dialogPlayEndAction += endAction;
            GameManager.Instance.player.DisablePlayerInput();

            // 5. 剧情对话专属前置逻辑
            if (playType == E_DialogPlayType.Plot)
            {
                EventCenter.Instance.EventTrigger<int>(E_EventType.E_PlotDialogStart, dialogID);
            }

            // 6. 开始播放第一条对话（统一逻辑）
            TriggerDialog(roleDialogData.startDialog);
        }

        /// <summary>
        /// 校验对话是否可播放（封装差异条件）
        /// </summary>
        private bool CheckCanPlayDialog(RoleDialogData data, E_DialogPlayType playType)
        {
            // 通用条件：未锁定
            if (data.isLocked) return false;

            switch (playType)
            {
                case E_DialogPlayType.Normal:
                    // 普通对话：未触发 或 已触发但支持重复
                    return !data.isTrigger || (data.isTrigger && data.canTriggerRepeat);
                case E_DialogPlayType.Plot:
                    // 剧情对话：仅未触发时可播放
                    return !data.isTrigger;
                default:
                    return false;
            }
        }
        #endregion

        #region 核心修改：合并下一句播放方法
        /// <summary>
        /// 播放下一句对话（普通/剧情通用）
        /// </summary>
        public void PlayNextDialog()
        {
            if (currentSingleDialogData == null || currentRoleDialogData == null)
            {
                Debug.LogError("无当前播放的对话数据");
                return;
            }

            // 有下一句则继续播放，无则结束
            if (currentSingleDialogData.childNodes != 0)
            {
                if (allDialogDataDic.TryGetValue(currentSingleDialogData.childNodes, out var nextData))
                {
                    TriggerDialog(nextData);
                }
                else
                {
                    Debug.LogError($"找不到下一句对话（子节点ID：{currentSingleDialogData.childNodes}）");
                    DialogPlayEnd(); // 找不到下一句也强制结束
                }
            }
            else
            {
                DialogPlayEnd();
            }
        }
        #endregion

        #region 核心修改：合并对话结束方法
        /// <summary>
        /// 对话播放结束（统一处理逻辑）
        /// </summary>
        private void DialogPlayEnd()
        {
            // 1. 关闭所有面板（通用）
            CloseAllDialogPanel();

            // 2. 分发结束事件 + 标记已触发（通用）
            if (!currentRoleDialogData.isTrigger)
            {
                EventCenter.Instance.EventTrigger<int>(E_EventType.E_DialogEnd, currentRoleDialogData.id);
                currentRoleDialogData.isTrigger = true;
            }

            // 3. 执行结束回调（通用）
            dialogPlayEndAction?.Invoke();

            // 4. 重置状态（通用）
            dialogPlayEndAction = null;
            currentRoleDialogData = null;
            currentSingleDialogData = null;

            // 5. 恢复玩家输入（通用）
            GameManager.Instance.player.EnablePlayerInput();
        }
        #endregion

        // 以下方法无核心修改，仅适配合并后的变量名
        private void CloseAllDialogPanel()
        {
            UIManager.Instance.HidePanel<DialogChooseUI>();
            UIManager.Instance.HidePanel<DialogUI>();
            UIManager.Instance.HidePanel<DialogInfoPanel>();
        }

        private void ShowDialogUI(E_DialogType type, DialogData data = null, UnityAction action = null)
        {
            // 标记是否是剧情对话（通过当前播放类型判断）
            bool isPlot = currentPlayType == E_DialogPlayType.Plot;

            switch (type)
            {
                case E_DialogType.Normal:
                case E_DialogType.Task:
                    UIManager.Instance.ShowPanel<DialogUI>((panel) =>
                    {
                        panel.IsPlot = isPlot;
                        panel.ShowDialog(data);
                        action?.Invoke();
                    });
                    break;
                case E_DialogType.Item:
                    UIManager.Instance.ShowPanel<TipPanel>((panel) =>
                    {
                        panel.UpdateInfo(data.tipInfoText);
                    });
                    BagManager.Instance.AddItem(data.itemID);
                    break;
                case E_DialogType.Choose:
                    UIManager.Instance.ShowPanel<DialogChooseUI>((panel) =>
                    {
                        panel.ShowDialog(data);
                    });
                    break;
                case E_DialogType.Effect:
                    UIManager.Instance.ShowPanel<TipPanel>((panel) =>
                    {
                        panel.UpdateInfo(data.tipInfoText);
                        // 仅第一次触发时改变San值（通用逻辑）
                        if (!currentRoleDialogData.isTrigger)
                            GameManager.Instance.player.statusData.ChangeSan(data.effectSize);
                    });
                    break;
                case E_DialogType.Info:
                    UIManager.Instance.ShowPanel<DialogInfoPanel>((panel) =>
                    {
                        panel.ShowDialog(data);
                    });
                    break;
            }
        }

        public void UnLockedDialogData(UnlockDialogData data)
        {
            // 原有逻辑不变
        }

        public void UnLockDialogByPreID(int id)
        {
            foreach (var item in allRoleDialogDataDic.Values)
            {
                if (item.preRoleDialogs == id)
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

        public bool CheckDialogCanPlay(int dialogID, E_DialogPlayType playType)
        {
            if (!allRoleDialogDataDic.ContainsKey(dialogID))
            {
                Debug.LogError($"找不到对话数据（ID：{dialogID}）");
                return false;
            }

            var data = allRoleDialogDataDic[dialogID];
            return CheckCanPlayDialog(data, playType);
        }

        public void HandleItemDialogData(DialogData data) { }
    }
}