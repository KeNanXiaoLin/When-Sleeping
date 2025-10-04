using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float EnemyMaxHealth;
    public float EnemyDamage;
    public float EnemyMoveSpeed;
    public float EnemyDetectRate;

    private float CC_EnemyHealth;

    //敌人检测范围内，发现玩家就移动
    //敌人攻击范围内进行攻击操作
    //敌人避开障碍物
    //敌人受到伤害

    public void Awake()
    {
        CC_EnemyHealth = EnemyMaxHealth;
    }

    public void EnemyGetDamage(float _Damage)
    {
        CC_EnemyHealth -= _Damage;
    }

    

}
