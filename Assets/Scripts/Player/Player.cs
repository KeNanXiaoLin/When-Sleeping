using System.Collections;
using System.Collections.Generic;
using KNXL.DialogSystem;
using TMPro;
using UnityEngine;

public enum E_MoveType
{
    Life,
    Battle
}

public enum E_Direction
{
    Left,
    Right,
    Up,
    Down,
    LeftUp,
    LeftDown,
    RightUp,
    RightDown
}

public class Player : MonoBehaviour
{
    public PlayerStatusData statusData;
    public int Max_San => statusData.maxSan;
    public E_MoveType moveType = E_MoveType.Battle;
    public RuntimeAnimatorController liftAnimator;
    public RuntimeAnimatorController battleAnimator;
    #region 战斗场景移动相关数值
    //移动速度
    public float moveSpeed = 5;
    //初始跳跃时的 竖直上抛速度
    public float initYSpeed = 10;
    //重力加速度
    public float G = 9.8f;
    public float jumpMaxH = 5;
    //跳跃当中 实时的速度是多少
    private float nowYSpeed;
    //当前平台的 Y 值
    [SerializeField]
    private float nowPlatformY = -999f;

    //是否可以主动下落
    private bool canFall;

    //二段跳计数
    [SerializeField]
    private int jumpIndex;


    //动画控制相关组件
    private Animator roleAnimator;
    //输入移动相关的系数
    private float horizontalMove;

    //用于处理上下平台的逻辑对象
    PlatformLogic platformLogic;

    //得到当前玩家是否是跳跃状态
    public bool isJump => roleAnimator.GetBool("isJump");
    //得到当前玩家是否在下落状态
    public bool isFall => roleAnimator.GetBool("isFall");
    #endregion

    #region 战斗场景相关的战斗数值
    /// <summary>
    /// 攻击力的大小
    /// </summary>
    public int atkSize = 10;
    /// <summary>
    /// 攻击的范围，因为进行的是盒装检测，所以实际范围会*2
    /// </summary>
    public float atkRange = 0.5f;
    public float atkInterval = 1f;
    public int maxHp = 100;
    private int curHp;
    public Transform damageCheckPoint;
    private float atkTime = 0;
    #endregion

    #region 村庄场景相关参数
    private float verticalMove;
    private E_Direction last_dir = E_Direction.Right;
    private float xFaceMinVal;
    private float yFaceMinVal;
    #endregion

    #region 控制玩家输入相关
    private bool disableInput = false;
    #endregion

    #region 功能性参数
    public TextMeshPro headTip;
    private bool isOnItem = false;
    private Item onItem = null;
    #endregion

    private RoleDialogData curDialogData = null;

    // Start is called before the first frame update
    void Awake()
    {
        curHp = maxHp;
        roleAnimator = this.GetComponent<Animator>();

        platformLogic = new PlatformLogic(this);
        statusData = new();
        Init();
    }

    void Start()
    {
        if (damageCheckPoint != null)
        {
            damageCheckPoint.localPosition = new Vector3(atkRange, 0, 0);
        }
    }

    private void Init()
    {
        switch (moveType)
        {
            case E_MoveType.Life:
                roleAnimator.runtimeAnimatorController = liftAnimator;
                break;
            case E_MoveType.Battle:
                roleAnimator.runtimeAnimatorController = battleAnimator;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            UIManager.Instance.ShowPanel<CheatPanel>();
        }
        if (disableInput) return;
        GetKeyCheck();
        switch (moveType)
        {
            case E_MoveType.Battle:
                BattleActionUpdate();
                break;
            case E_MoveType.Life:
                LifeActionUpdate();
                break;
        }
    }

    /// <summary>
    /// 游戏中玩家的按键检测
    /// </summary>
    private void GetKeyCheck()
    {
        CheckBagItemUse();
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isOnItem)
            {
                BagManager.Instance.AddItem(onItem.itemData.itemID);
                Destroy(onItem.gameObject);
                onItem = null;
            }
            else if (curDialogData != null)
            {
                DialogSystemMgr.Instance.StartPlayDialog(curDialogData.id, curDialogData.dialogPlayType);
            }
        }
    }

    /// <summary>
    /// 战斗场景动作的每帧检测
    /// </summary>
    private void BattleActionUpdate()
    {
        #region 移动相关逻辑
        //得到水平方向的输入 一般就是左右键输入
        //-1  0   1 三个值
        horizontalMove = Input.GetAxisRaw("Horizontal");
        //面朝左
        if (horizontalMove != 0)
        {
            roleAnimator.SetBool("isMove", true);
            this.transform.Translate(Vector2.right * moveSpeed * Time.deltaTime * horizontalMove);
        }
        else
        {
            roleAnimator.SetBool("isMove", false);
        }
        //需要改变面朝向
        if (horizontalMove > 0)
        {
            this.transform.localScale = Vector3.one;
        }
        else if (horizontalMove < 0)
        {
            this.transform.localScale = new Vector3(-1, 1, 1);
        }
        #endregion

        #region 跳跃逻辑
        //主动下落按键检测
        //我们没有跳跃没有下落时 才应该响应这个组合键
        if (canFall && !isJump && !isFall &&
             Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space))
        {
            this.Fall();
        }
        //组合键的响应 应该和跳跃时互斥的 所以我们使用 if else
        else if (Input.GetKeyDown(KeyCode.Space) && jumpIndex != 2)
        {
            //跳跃
            roleAnimator.SetBool("isJump", true);
            //当前Y速度 = 竖直上抛的初始速度
            nowYSpeed = initYSpeed;
            //二段跳计数 只能连续跳两次
            ++jumpIndex;
        }

        //跳跃状态时 Y 坐标的变化
        //下落状态时 Y 坐标也需要变化
        if (isJump || isFall)
        {
            //当我们跳跃或者下落时
            //第一帧位移之前 就让当前Y的速度产生变化
            //收到重力加速度的影响 每一帧 都去改变当前的移动速度
            nowYSpeed -= G * Time.deltaTime;
            //位移逻辑
            this.transform.Translate(Vector3.up * nowYSpeed * Time.deltaTime);

            //当竖直方向的速度小于等于0 就应该播放 下落的动画
            roleAnimator.SetBool("isFall", nowYSpeed <= 0);

            //判断是否落在了对应平台上
            if (this.transform.position.y <= nowPlatformY)
            {
                //停止跳跃动画
                roleAnimator.SetBool("isJump", false);
                roleAnimator.SetBool("isFall", false);
                //避免下落时 落到"平台里" 把它拉回来
                Vector3 pos = this.transform.position;
                pos.y = nowPlatformY;
                this.transform.position = pos;
                //落地时才去清楚二段跳计数
                jumpIndex = 0;
            }

        }
        #endregion

        #region 攻击相关逻辑
        atkTime += Time.deltaTime;
        //只有在待机动作下可以攻击
        if (!roleAnimator.GetBool(Setting.PlayerAnimationParameter_IsMove) &&
            !roleAnimator.GetBool(Setting.PlayerAnimationParameter_IsFall) &&
            !roleAnimator.GetBool(Setting.PlayerAnimationParameter_IsJump))
        {
            if (Input.GetKeyDown(KeyCode.J) && atkTime > atkInterval)
            {
                roleAnimator.SetTrigger("Attack1");
                atkTime = 0;
            }
            if (Input.GetKeyDown(KeyCode.K) && atkTime > atkInterval)
            {
                roleAnimator.SetTrigger("Attack2");
                atkTime = 0;
            }
        }

        #endregion
        #region 平台切换相关逻辑
        platformLogic.UpdateCheck();
        #endregion
    }

    /// <summary>
    /// 生活场景动作的每帧检测
    /// </summary>
    private void LifeActionUpdate()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");
        if (horizontalMove > 0 && Mathf.Approximately(verticalMove, 0))
            last_dir = E_Direction.Right;
        else if (horizontalMove < 0 && Mathf.Approximately(verticalMove, 0))
            last_dir = E_Direction.Left;
        else if (verticalMove < 0 && Mathf.Approximately(horizontalMove, 0))
            last_dir = E_Direction.Down;
        else if (verticalMove > 0 && Mathf.Approximately(horizontalMove, 0))
            last_dir = E_Direction.Up;
        else if (horizontalMove > 0 && verticalMove > 0)
            last_dir = E_Direction.RightUp;
        else if (horizontalMove > 0 && verticalMove < 0)
            last_dir = E_Direction.RightDown;
        else if (horizontalMove < 0 && verticalMove > 0)
            last_dir = E_Direction.LeftUp;
        else if (horizontalMove < 0 && verticalMove < 0)
            last_dir = E_Direction.LeftDown;
        transform.Translate(new Vector2(horizontalMove * moveSpeed * Time.deltaTime, verticalMove * moveSpeed * Time.deltaTime));
        ResetAnimatorParameters();
        roleAnimator.SetFloat("x", Mathf.Abs(horizontalMove) < 0.1f ? xFaceMinVal : horizontalMove);
        roleAnimator.SetFloat("y", Mathf.Abs(verticalMove) < 0.1f ? yFaceMinVal : verticalMove);
    }

    /// <summary>
    /// 去改变平台相关信息的
    /// </summary>
    /// <param name="y"></param>
    /// <param name="showShadow"></param>
    /// <param name="canFall"></param>
    public void ChangePlatformData(float y, bool canFall)
    {
        //改变当前平台的Y
        this.nowPlatformY = y;
        //是否可以在该平台下落
        this.canFall = canFall;
    }


    /// <summary>
    /// 玩家下落方法
    /// </summary>
    public void Fall()
    {
        //动作的切换
        roleAnimator.SetBool("isFall", true);
        //相当于把平台设置为null的感觉
        //把它设置为一个非常小的数 那么这样 在更新跳跃下落时
        //就不会把它拉回之前平台的位置了
        nowPlatformY = -9999;
        //由于是自由落体 我们Y上的速度应该从0开始
        //避免之前有残留 所以我们将其清0
        nowYSpeed = -2;
        //由于在下落时只允许跳一次 所以我们将计数从1开始
        //这样再下落过程中就不会进行二段跳了
        jumpIndex = 1;
    }
    /// <summary>
    /// 禁用玩家输入,需要重置动画机参数
    /// </summary>
    public void DisablePlayerInput()
    {
        Debug.Log("玩家的输入被禁用");
        ResetAnimatorParameters();
        disableInput = true;
        roleAnimator.SetFloat("x", xFaceMinVal);
        roleAnimator.SetFloat("y", yFaceMinVal);
    }

    private void ResetAnimatorParameters()
    {
        switch (last_dir)
        {
            case E_Direction.Left:
                xFaceMinVal = -0.01f;
                yFaceMinVal = 0f;
                break;
            case E_Direction.Right:
                xFaceMinVal = 0.01f;
                yFaceMinVal = 0f;
                break;
            case E_Direction.Up:
                yFaceMinVal = 0.01f;
                xFaceMinVal = 0f;
                break;
            case E_Direction.Down:
                yFaceMinVal = -0.01f;
                xFaceMinVal = 0f;
                break;
            case E_Direction.RightUp:
                xFaceMinVal = 0.01f;
                yFaceMinVal = 0.01f;
                break;
            case E_Direction.RightDown:
                xFaceMinVal = 0.01f;
                yFaceMinVal = -0.01f;
                break;
            case E_Direction.LeftUp:
                xFaceMinVal = -0.01f;
                yFaceMinVal = 0.01f;
                break;
            case E_Direction.LeftDown:
                xFaceMinVal = -0.01f;
                yFaceMinVal = -0.01f;
                break;
        }

    }

    /// <summary>
    /// 启用玩家输入
    /// </summary>
    public void EnablePlayerInput()
    {
        Debug.Log("启用玩家的输入");
        disableInput = false;
    }

    public void ShowHeadTip()
    {
        headTip.gameObject.SetActive(true);
    }

    /// <summary>
    /// 如果玩家站在物品上，需要调用这个方法
    /// </summary>
    public void SetItemInfo(Item item)
    {
        isOnItem = true;
        onItem = item;
    }

    /// <summary>
    /// 玩家进入可以触发对话的范围，设置数据
    /// </summary>
    /// <param name="data"></param>
    public void SetDialogData(RoleDialogData data)
    {
        curDialogData = data;
    }

    /// <summary>
    /// 玩家离开可以交互的范围，隐藏头顶提升
    /// </summary>
    public void HideHeadTip()
    {
        headTip.gameObject.SetActive(false);

    }

    /// <summary>
    /// 玩家离开物品的时候需要调用的方法
    /// </summary>
    public void ClearItemInfo()
    {
        isOnItem = false;
        onItem = null;
    }

    /// <summary>
    /// 玩家离开可以触发对话的范围，清空数据
    /// </summary>
    public void ClearDialogInfo()
    {
        curDialogData = null;
    }

    private void CheckBagItemUse()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            BagData data = BagManager.Instance.GetItemByIndex(1);
            UseItem(data);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            BagData data = BagManager.Instance.GetItemByIndex(2);
            UseItem(data);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            BagData data = BagManager.Instance.GetItemByIndex(3);
            UseItem(data);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            BagData data = BagManager.Instance.GetItemByIndex(4);
            UseItem(data);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            BagData data = BagManager.Instance.GetItemByIndex(5);
            UseItem(data);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            BagData data = BagManager.Instance.GetItemByIndex(6);
            UseItem(data);
        }
    }

    /// <summary>
    /// 启用战斗场景的操作方式
    /// </summary>
    public void EnableBattleAction()
    {
        moveType = E_MoveType.Battle;
        roleAnimator.runtimeAnimatorController = battleAnimator;
    }

    /// <summary>
    /// 启动生活场景的操作方式
    /// </summary>
    public void EnableLifeAction()
    {
        this.transform.localScale = Vector3.one;
        moveType = E_MoveType.Life;
        roleAnimator.runtimeAnimatorController = liftAnimator;
    }

    public void UseItem(BagData data)
    {
        if (data == null) return;
        if (!data.CanUse) return;
        //如果物品是牛奶，切换到战斗场景
        if (data.id == 2)
        {
            UIManager.Instance.ShowPanel<UsePanel>((panel) =>
            {
                panel.UpdateInfo(data.UseInfo);
                panel.RegisterOKAction(() =>
                {
                    SceneLoadManager.Instance.LoadScene("BattleScene", sceneFaderBefore: InitBattleInfo);
                    BagManager.Instance.RemoveItem(2);
                });
            });
        }

    }

    /// <summary>
    /// 初始化玩家在战斗场景的一些信息
    /// </summary>
    public void InitBattleInfo()
    {
        MusicManager.Instance.PlayBKMusic("战斗时激昂的小曲1");
        //回复玩家的血量
        curHp = maxHp;
        jumpIndex = 0;
        nowPlatformY = -999f;
        this.transform.position = new Vector3(-12, -3f, 0);
        this.EnableBattleAction();
        GameManager.Instance.InitCameraValues();
        UIManager.Instance.HidePanel<GameUI>();
        UIManager.Instance.ShowPanel<BattleUI>();
    }

    /// <summary>
    /// 改变玩家的朝向
    /// </summary>
    public void UpdatePlayerFacing(E_Direction dir)
    {
        last_dir = dir;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            Item item = collision.GetComponent<Item>();
            //如果这个物品可以交互
            if (item.itemData.canInteractive)
            {
                this.ShowHeadTip();
                this.SetItemInfo(item);
            }
        }
        else if (collision.gameObject.CompareTag("Interactive"))
        {
            DialogObj dialogObj = collision.GetComponent<DialogObj>();
            //只有当前可以播放对话的时候，才显示头顶的提示交互信息
            //先看是不是单次剧情触发器
            //如果是单次剧情触发器，不用自己处理，在对象那里会进行处理
            if (dialogObj.enterTrigger)
            {
                //看这个剧情是否达到触发条件，进行触发
                // if (DialogSystemMgr.Instance.CheckDialogCanPlay(dialogObj.dialogId, out E_DialogPlayType playType))
                // {
                //     this.SetDialogData(dialogObj.GetDialogData());
                //     //应该是物品交互，电视，相框等内容
                //     this.ShowHeadTip();
                // }
            }
            //如果不是，再找他身上可以触发的对话
            else
            {
                if (dialogObj.IsHaveDialogCanPlay())
                {
                    this.SetDialogData(dialogObj.GetFirstCanTriggerDialogData());
                    //应该是物品交互，电视，相框等内容
                    this.ShowHeadTip();
                }
            }


        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            Item item = collision.GetComponent<Item>();
            //如果这个物品可以交互
            if (item.itemData.canInteractive)
            {
                this.HideHeadTip();
                this.ClearItemInfo();
            }
        }
        else if (collision.gameObject.CompareTag("Interactive"))
        {
            //应该是物品交互，电视，相框等内容
            this.HideHeadTip();
            this.ClearDialogInfo();
        }
    }

    /// <summary>
    /// 玩家进行攻击是否能够对敌人造成伤害的检测
    /// </summary>
    private void AttackDamageCheckEvent()
    {
        //这里进行盒状检测
        Vector3 center = damageCheckPoint.position;
        Collider2D col = Physics2D.OverlapBox(center, Vector2.one * atkRange, 0, 1 << LayerMask.NameToLayer("Enemy"));
        if (col != null)
        {
            //得到敌人身上的脚本，进行造成伤害处理
            if (col.TryGetComponent<EnemyObj>(out EnemyObj enemyObj))
            {
                enemyObj.Damage(atkSize);
            }
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
        EventCenter.Instance.EventTrigger<float>(E_EventType.E_UpdateHp, (float)curHp / maxHp);
        Debug.Log(name + "受到伤害" + damage);
        roleAnimator.SetTrigger("Damage");
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
        roleAnimator.SetBool("Death", true);
    }

    /// <summary>
    /// 死亡动画播放结束触发的事件
    /// </summary>
    private void DeathPlayOverEvent()
    {
        Destroy(this.gameObject);
        UIManager.Instance.ShowPanel<LosePanel>();
    }
}
