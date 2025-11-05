using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "MyAssets/EnemyData")]
/// <summary>
/// 敌人的数据类
/// </summary>
public class EnemyData : ScriptableObject
{
    [Header("攻击力的大小")]
    public int atkSize = 5;
    [Header("攻击范围")]
    public float atkRange = 0.5f;
    [Header("血量")]
    public int hp = 30;
    [Header("移动速度")]
    public float moveSpeed = 2f;
    [Header("搜寻敌人的范围")]
    public float searchRange = 2f;
    [Header("追逐停止的距离，应该略小于攻击范围，以便能够打到敌人")]
    public float chaseRange = 0.3f;
    [Header("攻击间隔")]
    public float atkInterval = 1f;
}
