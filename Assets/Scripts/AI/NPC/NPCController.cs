using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : AIBase
{
    #region 私有成员
    /// <summary>
    /// 记录跟随的对象
    /// </summary>
    private Transform target;
    /// <summary>
    /// A*寻路计算的频率
    /// </summary>
    private WaitForSeconds aStarCalInterval;
    private Coroutine followCoroutine;
    /// <summary>
    /// 目标的上次位置，主要是避免目标没有移动的时候进行计算
    /// </summary>
    private Vector3 lastTargetPos;
    /// <summary>
    /// 当前到目标的路径点
    /// </summary>
    private List<Vector3> currentPath = new();
    /// <summary>
    /// 当前走到了哪个点
    /// </summary>
    private int currentPathIndex = 0;
    /// <summary>
    /// 是否处于移动状态
    /// </summary>
    public bool isMoving {get{return npcData.isMoving;} set{npcData.isMoving = value;}}
    #endregion
    /// <summary>
    /// 每个NPC应该有一个家的位置，如果没有，不用赋值即可
    /// </summary>
    public Vector3 homePos {get{return npcData.homePos;} set{npcData.homePos.Set(value);}}
    /// <summary>
    /// NPC的移动速度
    /// </summary>
    public float moveSpeed {get{return npcData.moveSpeed;} set{npcData.moveSpeed = value;}}
    /// <summary>
    /// 跟随保持的距离
    /// </summary>
    public float followDis {get{return npcData.followDis;} set{npcData.followDis = value;}}
    /// <summary>
    /// NPC的类型
    /// </summary>
    public E_NPCType npcType {get{return npcData.npcType;} set{npcData.npcType = value;}}
    /// <summary>
    /// NPC的位置
    /// </summary>
    public Vector3 pos {get{return npcData.pos;} set{npcData.pos.Set(value);}}
    /// <summary>
    /// 是否跟随玩家，因为在游戏里，NPC只会跟随玩家
    /// </summary>
    public bool isFollowPlayer {get{return npcData.isFollowPlayer;} set{npcData.isFollowPlayer = value;}}
    public NPCData npcData; 


    void Awake()
    {
        m_animator = GetComponent<Animator>();
        //按理说在这个场景，这个场景的数据就不可能为空，但是为了保险起见，还是判断一下
        var sceneData = SaveSystemMgr.Instance.saveData.sceneData.GetSceneData(GameManager.Instance.currentSceneName);
        if (sceneData != null)
        {
            npcData = sceneData.GetNPCData(npcData.npcType);
        }
        //兜底
        else
        {
            npcData = new NPCData();
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
        m_stateMachine?.Update();
    }

    // public void Init(E_NPCType npcType,Vector3 pos,Vector3 homePos)
    // {
    //     this.npcType = npcType;
    //     this.pos = pos;
    //     this.homePos = homePos;
    // }

    public void Init(NPCData npcData)
    {
        this.npcData = npcData;
        //初始化状态机
        m_stateMachine = new StateMachine();
        m_stateMachine.AddState(E_StateType.Idle, new IdleState(this));
        m_stateMachine.AddState(E_StateType.Move, new MoveState(this));
        if(isMoving)
            m_stateMachine.ChangeState(E_StateType.Move);
        else
            m_stateMachine.ChangeState(E_StateType.Idle);
        //初始化跟随目标状态
        if(isFollowPlayer)
            EnableFollow(GameManager.Instance.player.transform);
        aStarCalInterval = new WaitForSeconds(0.2f);
    }

    public void ChangeDir(E_Direction dir)
    {
        this.Facing = dir;
    }

    public void EnableFollow(Transform target)
    {
        isFollowPlayer = true;
        this.target = target;
        followCoroutine = StartCoroutine(FollowTarget());
    }

    public void DisableFollow()
    {
        isFollowPlayer = false;
        target = null;
        currentPath.Clear();
        currentPathIndex = 0;
        if (followCoroutine != null)
            StopCoroutine(followCoroutine);
    }

    public IEnumerator FollowTarget()
    {
        while (target != null)
        {
            // 目标位置变化时才重新计算路径
            if (lastTargetPos != target.position)
            {
                List<Vector3> newPath = AStarMgr.Instance.FindPath(transform.position, target.position);
                // 如果返回空，证明这个时候A*寻路网格还没有找到，等下一次循环再试
                if (newPath == null)
                    yield return null;
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
        this.pos = transform.position;

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
    /// 回家
    /// </summary>
    public void BackToHome()
    {
        //首先禁用跟随
        DisableFollow();
        if (homePos == Vector3.zero)
        {
            Debug.LogWarning("请初始化家的位置");
            return;
        }
        List<Vector3> newPath = AStarMgr.Instance.FindPath(transform.position, homePos);
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
            Debug.LogWarning("找不到一条通往家的路");
            isMoving = false;
            m_stateMachine.ChangeState(E_StateType.Idle);
        }
    }

    public void GotoTargetPos(Vector3 targetPos)
    {
        //首先禁用跟随
        DisableFollow();
        List<Vector3> newPath = AStarMgr.Instance.FindPath(transform.position, targetPos);
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
            Debug.LogWarning($"找不到一条通往目标位置的路，目标位置{targetPos}");
            isMoving = false;
            m_stateMachine.ChangeState(E_StateType.Idle);
        }
    }

    public void SetNPCFacing(E_Direction dir)
    {
        Facing = dir;
    }
}
