using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    private StateMachine m_stateMachine;
    private Animator m_animator;
    private Transform target;
    private WaitForSeconds aStarCalInterval;
    private Vector3 lastTargetPos;
    private List<Vector3> currentPath = new();
    private int currentPathIndex = 0;
    private bool isMoving = false;
    public float moveSpeed = 3f;
    public float followDis = 1f;
    public E_Direction Facing = E_Direction.Right;
    public Animator Animator => m_animator;





    void Awake()
    {
        m_stateMachine = new StateMachine();
        m_animator = GetComponent<Animator>();
        aStarCalInterval = new WaitForSeconds(0.2f);
    }

    void Start()
    {
        m_stateMachine.AddState(E_StateType.Idle, new IdleState(this));
        m_stateMachine.AddState(E_StateType.Move, new MoveState(this));
        m_stateMachine.ChangeState(E_StateType.Idle);
    }

    void Update()
    {
        m_stateMachine?.Update();
    }

    public void ChangeDir(E_Direction dir)
    {
        this.Facing = dir;
    }

    public void EnableFollow(Transform target)
    {
        this.target = target;
        StartCoroutine(FollowTarget());
    }

    public void DisableFollow()
    {
        target = null;
    }

    public IEnumerator FollowTarget()
    {
        while (target != null)
        {
            // 目标位置变化时才重新计算路径
            if (lastTargetPos != target.position)
            {
                List<Vector3> newPath = AStarMgr.Instance.FindPath(transform.position, target.position);
                // 更新当前路径（清除旧路径，添加新路径）
                currentPath.Clear();
                if (newPath != null && newPath.Count > 0)
                {
                    currentPath.AddRange(newPath);
                    currentPathIndex = 0; // 重置路径索引
                    isMoving = true;
                    // 如果当前是Idle状态，切换到Move状态
                    if (m_stateMachine.CurrentStateType != E_StateType.Move)
                    {
                        m_stateMachine.ChangeState(E_StateType.Move);
                    }
                }
                else
                {
                    // 没有路径时停止移动
                    isMoving = false;
                    m_stateMachine.ChangeState(E_StateType.Idle);
                }
                lastTargetPos = target.position;
            }
            yield return aStarCalInterval; // 按间隔计算路径
        }
        // 目标为空时停止移动
        isMoving = false;
        m_stateMachine.ChangeState(E_StateType.Idle);
    }

    /// <summary>
    /// 根据传入的点，计算当前的面向
    /// </summary>
    /// <param name="targetPos"></param>
    private void CalDirection(Vector3 targetPos)
    {
        Vector3 v = (targetPos - this.transform.position).normalized;
        //左右
        if (Mathf.Abs(v.x) > Mathf.Abs(v.y))
        {
            if (v.x > 0)
            {
                Facing = E_Direction.Right;
            }
            else
            {
                Facing = E_Direction.Left;
            }
        }
        //上下
        else
        {
            if (v.y > 0)
            {
                Facing = E_Direction.Up;
            }
            else
            {
                Facing = E_Direction.Down;
            }
        }
    }

    /// <summary>
    /// 每帧更新移动（在Move状态中调用）
    /// </summary>
    public void UpdateMovement()
    {
        if (currentPath == null || currentPath.Count == 0 || currentPathIndex >= currentPath.Count)
        {
            // 路径为空或已走完，切换到Idle
            isMoving = false;
            m_stateMachine.ChangeState(E_StateType.Idle);
            return;
        }

        // 获取当前目标点
        Vector3 targetPoint = currentPath[currentPathIndex];
        // 计算到目标点的方向
        Vector3 direction = (targetPoint - transform.position).normalized;
        // 每帧移动一小段距离（基于速度和deltaTime）
        transform.position += direction * moveSpeed * Time.deltaTime;

        // 计算与目标点的距离（忽略Y轴，2D游戏可简化）
        float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.y),
                                          new Vector2(targetPoint.x, targetPoint.y));
        CalDirection(currentPath[currentPathIndex]);

        // 如果到达当前目标点，切换到下一个路径点
        if (distance < 0.1f) // 阈值根据需求调整
        {
            currentPathIndex++;
            // 更新面向方向（根据下一个点）
            // if (currentPathIndex < currentPath.Count)
            // {
            //     CalDirection(currentPath[currentPathIndex]);
            // }
        }

        // 更新动画（根据移动方向）
        // UpdateMoveAnimation(direction);
    }

    /// <summary>
    /// 更新移动动画（根据方向）
    /// </summary>
    private void UpdateMoveAnimation(Vector3 direction)
    {
        m_animator.SetBool("IsMoving", true);
        m_animator.SetFloat("X", direction.x);
        m_animator.SetFloat("Y", direction.y);
    }
}
