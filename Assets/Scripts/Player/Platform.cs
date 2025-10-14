using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 平台数据类
/// </summary>
public class Platform : MonoBehaviour
{
    //平台的宽
    public float width = 5;
    //平台是否可以下落
    public bool canFall = true;

    //我们平台对应的Y坐标
    public float Y => this.transform.position.y;
    //平台的左边界
    public float Left => this.transform.position.x - width / 2;
    //平台的右边界
    public float Right => this.transform.position.x + width / 2;
    // 1. 定义浮点误差容忍值（一般取0.01f即可，可根据项目精度调整）
    float epsilon = 0.01f;

    private void Start()
    {
        PlatformDataMgr.Instance.AddPlatform(this);
    }

    /// <summary>
    /// 检测玩家是否可以落在我之上
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool CheckObjFallOnMe(Vector3 pos)
    {
        //对象的Y在我之上 并且在我的左右边界之内 就认为可以落在我身上
        // 2. 竖直方向：玩家Y >= 平台Y - epsilon（允许微小低于平台的误差）
        // 水平方向：玩家X在平台左右边界之间（同样加入误差，避免边缘检测失效）
        bool isYValid = pos.y >= Y - epsilon;
        bool isXValid = pos.x >= Left - epsilon && pos.x <= Right + epsilon;
        
        return isYValid && isXValid;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position - Vector3.right * width / 2, this.transform.position + Vector3.right * width / 2);
    }
}
