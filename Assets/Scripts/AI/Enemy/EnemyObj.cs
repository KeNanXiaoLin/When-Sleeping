using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObj : AIBase
{
    public EnemyData m_data;
    private int maxHp;
    private int curHp;
    private bool isDeath => curHp <= 0;
    private float moveSpeed => m_data.moveSpeed;
    private float searchRange => m_data.searchRange;
    private float chaseRange => m_data.chaseRange;
    private float atkInterval => m_data.atkInterval;
    private float atkRange => m_data.atkRange;
    private WaitForSeconds wait3s;
    private WaitForSeconds wait1s;
    private Coroutine patrolCoroutine;
    private Coroutine checkEnemyCoroutine;
    private Coroutine chaseEnemyCoroutine;
    private Transform target;
    public Transform damageCheckPoint;

    private void Awake()
    {
        maxHp = m_data.hp;
        curHp = maxHp;
        m_animator = GetComponent<Animator>();
        m_stateMachine = new StateMachine();
        m_stateMachine.AddState(E_StateType.Battle_Idle, new Enemy_IdleState(this));
        m_stateMachine.AddState(E_StateType.Battle_Move, new Enemy_WalkState(this));
        m_stateMachine.AddState(E_StateType.Battle_Attack, new Enemy_AttackState(this));
        m_stateMachine.AddState(E_StateType.Battle_Damage, new Enemy_DamageState(this));
        m_stateMachine.AddState(E_StateType.Battle_Death, new Enemy_DeadState(this));
        m_stateMachine.ChangeState(E_StateType.Battle_Idle);
        wait1s = new WaitForSeconds(1);
        wait3s = new WaitForSeconds(3);
    }

    void Start()
    {
        patrolCoroutine = StartCoroutine(SimulatorAIBehavior());
        checkEnemyCoroutine = StartCoroutine(SearchEnemy());
        chaseEnemyCoroutine = StartCoroutine(ChaseEnemy());
        if (damageCheckPoint != null)
        {
            damageCheckPoint.localPosition = new Vector3(atkRange, 0, 0);
        }
    }

    /// <summary>
    /// 受伤
    /// </summary>
    /// <param name="damage"></param>
    public void Damage(int damage)
    {
        if (curHp == 0) return;
        curHp -= damage;
        m_stateMachine.ChangeState(E_StateType.Battle_Damage);
        if (curHp <= 0)
        {
            curHp = 0;
            Death();
        }
    }

    /// <summary>
    /// 死亡
    /// </summary>
    private void Death()
    {
        m_stateMachine.ChangeState(E_StateType.Battle_Death);
    }

    /// <summary>
    /// 死亡动画播放完毕触发的事件
    /// </summary>
    private void DeathPlayOverEvent()
    {
        Destroy(this.gameObject);
    }

    /// <summary>
    /// 攻击动画播放触发的事件
    /// </summary>
    private void AttakPlayEvent()
    {
        //这里进行盒状检测
        Vector3 center = damageCheckPoint.position;
        Collider2D col = Physics2D.OverlapBox(center, Vector2.one * atkRange, 0, 1 << LayerMask.NameToLayer(Setting.LayerName_Player));
        if (col != null)
        {
            //得到敌人身上的脚本，进行造成伤害处理
            if (col.TryGetComponent<Player>(out Player p))
            {
                p.Damage(m_data.atkSize);
            }
        }
    }

    /// <summary>
    /// 模拟一下AI的行为，待机，走路，巡逻等行为
    /// </summary>
    /// <returns></returns>
    private IEnumerator SimulatorAIBehavior()
    {
        //待机3s
        m_stateMachine.ChangeState(E_StateType.Battle_Idle);
        yield return wait3s;
        //向左走3格,停留1s
        //向右走6个,停留1s
        //回到原来的位置停留3s，重复前面的动作
        Vector3 firstPos = this.transform.position + Vector3.left * 3;
        Vector3 secondPos = this.transform.position + Vector3.right * 6;
        Vector3 thirdPos = this.transform.position;
        List<(Vector3, WaitForSeconds)> targetPosList = new() { (firstPos, wait1s), (secondPos, wait1s), (thirdPos, wait3s) };
        Vector3 targetPos;
        WaitForSeconds waitTime;
        //没有目标的时候，进行巡逻
        while (target == null)
        {
            (targetPos, waitTime) = targetPosList[0];
            CalDirection(targetPos);
            //没有到达目标点,继续向目标点移动
            if (Vector3.Distance(targetPos, this.transform.position) > 0.2f)
            {
                Vector3 dir = (targetPos - this.transform.position).normalized;
                this.transform.Translate(dir * moveSpeed * Time.deltaTime);
                //切换到移动动画
                m_stateMachine.ChangeState(E_StateType.Battle_Move);
                yield return null;
            }
            //到达目标点，停下来播放待机动画
            else
            {
                m_stateMachine.ChangeState(E_StateType.Battle_Idle);
                targetPosList.RemoveAt(0);
                targetPosList.Add((targetPos, waitTime));
                yield return waitTime;
            }
        }
    }

    /// <summary>
    /// 在自己的周围寻找敌人，盒装检测
    /// </summary>
    /// <returns></returns>
    private IEnumerator SearchEnemy()
    {
        while (true)
        {
            Collider2D col = Physics2D.OverlapBox(this.transform.position, Vector2.one * searchRange, 0, 1 << LayerMask.NameToLayer(Setting.LayerName_Player));
            if (col != null)
            {
                target = col.transform;
            }
            //控制检测的频率，1s检测一次
            yield return wait1s;
        }
    }
    /// <summary>
    /// 发现了敌人，并且开始追逐敌人
    /// </summary>
    /// <returns></returns>
    private IEnumerator ChaseEnemy()
    {
        float t = 0;
        while (target != null)
        {
            t += Time.deltaTime;
            // 攻击不到敌人，需要先靠近
            if (Vector3.Distance(this.transform.position, target.transform.position) > chaseRange)
            {
                CalDirection(target.position);
                Vector3 dir = (target.position - transform.position).normalized;
                this.transform.Translate(dir * moveSpeed * Time.deltaTime);
                m_stateMachine.ChangeState(E_StateType.Battle_Move);
                yield return null;
            }
            //攻击的到敌人
            else
            {
                // 主要是不能让敌人一直攻击
                if (t > atkInterval)
                {
                    m_stateMachine.ChangeState(E_StateType.Battle_Attack);
                    t = 0;
                }
                yield return null;
            }
        }
    }
}
