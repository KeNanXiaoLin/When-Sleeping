using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KNXL.DialogSystem
{
    public enum E_DialogTime
    {
        /// <summary>
        /// 对话的开始
        /// </summary>
        isStart,
        /// <summary>
        /// 处于对话中
        /// </summary>
        isSpeaking,
        /// <summary>
        /// 对话的结束
        /// </summary>
        isEnd,
    }
}
