using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KNXL.DialogSystem
{
    /// <summary>
    /// 单条对话数据
    /// </summary>
    public class DialogData
    {
        /// <summary>
        /// 对话数据id
        /// </summary>
        public int id;
        /// <summary>
        /// 对话角色的类型
        /// </summary>
        public E_DialogRoleType type = E_DialogRoleType.MainRole;
        /// <summary>
        /// 对话类型
        /// </summary>
        public E_DialogType dialogType = E_DialogType.Normal;
        /// <summary>
        /// 对话的时机，目前用到的只有开始，用来找开始节点
        /// </summary>
        public E_DialogTime dialogTime = E_DialogTime.isSpeaking;
        public string dialogName;
        public string headIconRes;
        /// <summary>
        /// 如果是一个获得物品的对话，获得物品的id
        /// </summary>
        public int itemID = -1;
        /// <summary>
        /// 如果是一个状态变化的对话，状态提升多少
        /// </summary>
        public int effectSize = 0;
        /// <summary>
        /// 对话的文本
        /// </summary>
        public string dialogText;
        /// <summary>
        /// 这个对话的父节点
        /// </summary>
        public int parentNodes;
        /// <summary>
        /// 这个对话的子节点
        /// </summary>
        public int childNodes;
        /// <summary>
        /// 提示面板显示出的信息
        /// </summary>
        public string tipInfoText;
        /// <summary>
        /// 对话节点的选项一
        /// </summary>
        public string option1;
        /// <summary>
        /// 对话节点的选项二
        /// </summary>
        public string option2;
        /// <summary>
        /// 对话节点的选项三
        /// </summary>
        public string option3;
        /// <summary>
        /// 对话节点的选项四
        /// </summary>
        public string option4;
    }
}