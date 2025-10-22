// using System.Collections;
// using System.Collections.Generic;
// using System.Text;
// using UnityEngine;

// public class DialogSystem : BaseManager<DialogSystem>
// {
//     private Dictionary<int, DialogData> dialogDic;
//     private DialogData curData;
//     /// <summary>
//     /// 用于实现文字打字机效果
//     /// </summary>
//     private StringBuilder sb = new();
//     /// <summary>
//     /// 当前对话是否显示全部
//     /// </summary>
//     private bool isShowAll = false;

//     /// <summary>
//     /// 私有构造
//     /// </summary>
//     private DialogSystem()
//     {
//         Init();

//     }

//     /// <summary>
//     /// 初始化所有对话数据
//     /// </summary>
//     public void Init()
//     {
//         dialogDic = new();
//         DialogData[] dialogs = Resources.LoadAll<DialogData>("DialogData");
//         foreach (DialogData data in dialogs)
//         {
//             if (!dialogDic.ContainsKey(data.id))
//             {
//                 dialogDic.Add(data.id, data);
//             }
//             else
//             {
//                 Debug.Log("存在重复id属性，请检查是否存在配置错误");
//             }
//         }
//     }

//     /// <summary>
//     /// 测试脚本，用于初始化单例
//     /// </summary>
//     public void Test()
//     {

//     }
//     /// <summary>
//     /// 触发开始对话
//     /// </summary>
//     /// <param name="data"></param>
//     public void TriggerStartDialog(DialogData data)
//     {
//         if (!dialogDic.ContainsKey(data.id)) return;
//         curData = data;

//         if (data.isStart)
//         {
//             //显示对话面板，播放对话
//             UIManager.Instance.ShowPanel<DialogPanel>((panel) =>
//             {
//                 // panel.UpdateInfo(data);
//                 panel.UpdateDialogOtherInfo(data);
//                 TimeSystem.Instance.StartCoroutine(PlayDialogCoroutine(data.content));
//                 //设置对话已经触发，防止重复触发
//                 data.isTrigger = true;

//             });
//         }
//         else
//         {
//             Debug.Log("当前没有办法触发对话，请检查数据是否存在问题");
//         }
//     }

//     public void TriggerDialog(DialogData data)
//     {
//         if (!dialogDic.ContainsKey(data.id)) return;
//         curData = data;
//         if (data.type == E_DialogType.God)
//         {
//             UIManager.Instance.ShowPanel<TipPanel>((panel) =>
//             {
//                 panel.UpdateInfo(data.item.des);
//                 BagManager.Instance.AddItem(data.item.itemID);
//             });
//         }
//         else
//         {
//             //显示对话面板，播放对话
//             UIManager.Instance.ShowPanel<DialogPanel>((panel) =>
//             {
//                 // panel.UpdateInfo(data);
//                 panel.UpdateDialogOtherInfo(data);
//                 TimeSystem.Instance.StartCoroutine(PlayDialogCoroutine(data.content));
//             });
//         }

//     }

//     /// <summary>
//     /// 触发下一句对话
//     /// </summary>
//     public void TriggerNextDialog()
//     {
//         if (curData == null)
//         {
//             Debug.LogError("当前还没有触发过对话，无法触发下一句对话");
//             return;
//         }
//         if (curData.isEnd)
//         {
//             Debug.Log("对话已经触发完毕");
//             //关闭对话面板，做一些其他事情
//             UIManager.Instance.HidePanel<DialogPanel>();
//             //分发对话结束事件
//             EventCenter.Instance.EventTrigger(E_EventType.E_DialogEnd);
//         }
//         else
//         {
//             curData = curData.nextData;
//             TriggerDialog(curData);
//         }
//     }

//     /// <summary>
//     /// 让文字向打字机一样慢慢的出现
//     /// 英文的处理
//     /// </summary>
//     /// <param name="info">要做动画的文字</param>
//     /// <returns></returns>
//     private IEnumerator PlayDialogCoroutine(string info)
//     {
//         DialogPanel panel = UIManager.Instance.GetPanel<DialogPanel>();
//         string[] strs = info.Split(" ");
//         isShowAll = false;

//         for (int i = 0; i < strs.Length; i++)
//         {
//             for (int j = 0; j <= i; ++j)
//             {
//                 //如果这一帧，玩家点击了对话框，那么直接显示所有内容
//                 if (isShowAll)
//                 {
//                     sb.Append(info);
//                     panel.UpdateDialogText(sb.ToString());
//                     //重置数据
//                     sb.Clear();
//                     yield break;
//                 }
//                 else
//                 {
//                     sb.Append(strs[j]);
//                     Debug.Log("应该显示：" + strs[i]);
//                     sb.Append(" ");
//                 }
//             }
//             panel.UpdateDialogText(sb.ToString());
//             sb.Clear();
//             yield return new WaitForSeconds(0.1f);
//         }
//         isShowAll = true;
//     }

//     /// <summary>
//     /// 尝试播放下一句对话
//     /// </summary>
//     public void TryPlayNextDialog()
//     {
//         //如果这一句对话都没有播放完毕，先把这一句话播放完毕
//         if (!isShowAll)
//         {
//             isShowAll = true;
//         }
//         else
//         {
//             TriggerNextDialog();
//         }
//     }
// }
