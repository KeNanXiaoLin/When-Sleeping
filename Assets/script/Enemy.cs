using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJ
{

    public class Enemy : MonoBehaviour
    {
        [Header("敌人基本参数")]
        public float EnemyMaxHealth;
        public float EnemyDamage;
        public float EnemyMoveSpeed;

        [Header("敌人追逐玩家相关参数")]
        public float EnemyChaseRate;
        public float EnemyAttackDistence;


        private float CC_EnemyHealth;
        private GameObject Player;

        //敌人检测范围内，发现玩家就移动
        //敌人攻击范围内进行攻击操作
        //敌人避开障碍物
        //敌人受到伤害

        private void Awake()
        {
            CC_EnemyHealth = EnemyMaxHealth;

            BattleSceneManager.instence.Enemy.Add(this.gameObject);
            Player = BattleSceneManager.instence.Player;
        }

        private void Update()
        {
            EnemyChasePlayer();
        }

        public void EnemyGetDamage(float _Damage)
        {
            CC_EnemyHealth -= _Damage;
        }

        public void EnemyDead()
        {
            BattleSceneManager.instence.Enemy.Remove(this.gameObject);

            Destroy(this.gameObject);
        }

        private void EnemyChasePlayer()
        {

            if (Vector2.Distance(this.transform.position, Player.transform.position) < EnemyChaseRate)
            {

                //达到攻击距离时，停止移动并攻击
                if (Vector2.Distance(this.transform.position, Player.transform.position) < EnemyAttackDistence)
                {
                    //TODO enemyAttack"开始攻击动画“

                }
                else
                {
                //检查玩家位置在左边还是右边
                Vector2 ChaseDirection = (Player.transform.position - this.transform.position).normalized;
                ChaseDirection.y = 0;

                //敌人朝向玩家位置移动
                this.transform.Translate(ChaseDirection * EnemyMoveSpeed * Time.deltaTime);
                }

                //TODO 增加“前方可移动位置”检测，防止敌人坠机
            }
        }

        //用于动画的攻击函数
        public void Anim_EnemyAttack()
        {
            EventListener.EnemyDamage(EnemyDamage);
        }

        public void Anim_EnemyReWork()
        {
            //敌人动画重新播放
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(this.transform.position, EnemyChaseRate);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, EnemyAttackDistence);
        }


    }

}