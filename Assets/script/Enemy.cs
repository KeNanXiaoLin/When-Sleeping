using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
        public GameObject EnemyAttackRate;

        private bool FlipToRight = false;
        private bool CanMove = true;
        private float EnemyAttackRateDistence_ToEnemy;

        public Animator EnemyAnim;
        public Animator EnemyAttAnim;


        private float CC_EnemyHealth;
        private GameObject Player;

        //敌人检测范围内，发现玩家就移动
        //敌人攻击范围内进行攻击操作
        //敌人避开障碍物
        //敌人受到伤害

        private void Awake()
        {
            CC_EnemyHealth = EnemyMaxHealth;
            EnemyAttackRateDistence_ToEnemy = EnemyAttackRate.transform.localPosition.x;

            BattleSceneManager.Instance.Enemy.Add(this.gameObject);
            Player = BattleSceneManager.Instance.Player;

            EventListener.OnGameLose += EnemyMotionStop;
        }

        private void Update()
        {
            EnemyChasePlayer();

            if (CC_EnemyHealth <= 0)
                EnemyDead();
        }

        public void EnemyGetDamage(float _Damage)
        {
            CC_EnemyHealth -= _Damage;
        }

        public void EnemyDead()
        {
            BattleSceneManager.Instance.Enemy.Remove(this.gameObject);

            Destroy(this.gameObject);
        }

        private void EnemyMotionStop()
        {
            EnemyAttAnim.speed = 0;
            CanMove = false;
        }

        //敌人追逐
        private void EnemyChasePlayer()
        {
            if (CanMove == false) return;

            if (Vector2.Distance(this.transform.position, Player.transform.position) < EnemyChaseRate)
            {

                //达到攻击距离时，停止移动并攻击
                if (Vector2.Distance(this.transform.position, Player.transform.position) < EnemyAttackDistence)
                {
                    //TODO enemyAttack"开始攻击动画“
                    EnemyAnim.speed = 1;
                    EnemyAttAnim.Play("EnemyRate", 0);
                }
                else
                {
                    EnemyAttAnim.Play("New State", 0, 1);
                    //检查玩家位置在左边还是右边
                    Vector2 ChaseDirection = (Player.transform.position - this.transform.position).normalized;
                    ChaseDirection.y = 0;
                    EnemyFlip(ChaseDirection);

                    //敌人朝向玩家位置移动
                    this.transform.Translate(ChaseDirection * EnemyMoveSpeed * Time.deltaTime);
                }

                //TODO 增加“前方可移动位置”检测，防止敌人坠机
            }
        }

        //敌人左右反转
        public void EnemyFlip(Vector2 _FilpDirection)
        {
            if (_FilpDirection.normalized.x == 0) return;
            if (FlipToRight == false && _FilpDirection.normalized.x < 0) return;
            if (FlipToRight == true && _FilpDirection.normalized.x > 0) return;

            //求出战斗区域和玩家的相对距离

            //向左反转
            if (FlipToRight == true)
            {

                this.GetComponent<SpriteRenderer>().flipX = true;
                EnemyAttackRate.transform.localPosition = new Vector3(EnemyAttackRateDistence_ToEnemy, 0, 0);
                FlipToRight = false;
            }

            else if (FlipToRight == false)
            {
                //向右反转
                this.GetComponent<SpriteRenderer>().flipX = false;
                EnemyAttackRate.transform.localPosition = new Vector3(-EnemyAttackRateDistence_ToEnemy, 0, 0);
                FlipToRight = true;
            }

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