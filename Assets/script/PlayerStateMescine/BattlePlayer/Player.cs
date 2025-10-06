using UnityEngine;

namespace GJ
{
    [RequireComponent(typeof(PlayerStateMescine))]
    public class Player : MonoBehaviour
    {
        public Rigidbody2D rd { get; private set; }
        public Animator anim { get; private set; }

        [Header("基本数值")]
        [SerializeField] private float PlayerMaxHealth;
        [SerializeField] private float MoveSpeed;
        private float CC_PlayerHealth;

        [Header("攻击相关")]
        [SerializeField] private float PlayerDamage;
        [SerializeField] private float PlayerAttackTime;
        [SerializeField] private GameObject PlayerAttackRange;

        [Header("跳跃相关")]
        [SerializeField] private float PlayerJumpForce;
        [SerializeField] private float PlayerGroundDetectDistence;
        [HideInInspector] public bool isAttacking;
        [HideInInspector] public bool isJumping;


        public bool FlipToRight { get; private set; }
        private float AttackRateDistance_ToPlayer;


        #region 状态机相关

        public PlayerStateMescine StateMachine { get; private set; }

        public PlayerIdleState IdleState { get; private set; }
        public PlayerMoveState MoveState { get; private set; }
        public PlayerJumpState JumpState { get; private set; }
        public PlayerAttackState AttackState { get; private set; }
        public PlayerInsteractState InsteractState { get; private set; }
        public PlayerStopState StopState{ get; private set; }
        #endregion

        private void Awake()
        {
            StateMachine = GetComponent<PlayerStateMescine>();

            IdleState = new PlayerIdleState(StateMachine, this, "Idle");
            MoveState = new PlayerMoveState(StateMachine, this, "Move");
            JumpState = new PlayerJumpState(StateMachine, this, "Jump");
            AttackState = new PlayerAttackState(StateMachine, this, "Attack");
            InsteractState = new PlayerInsteractState(StateMachine, this, "Insteract");
            StopState = new PlayerStopState(StateMachine, this, "");

            isAttacking = false;
            isJumping = false;
            FlipToRight = false;
            CC_PlayerHealth = PlayerMaxHealth;
            AttackRateDistance_ToPlayer = PlayerAttackRange.transform.localPosition.x;

            BattleSceneManager.Instance.Player = this.gameObject;
        }

        void Start()
        {
            anim = GetComponent<Animator>();
            rd = GetComponent<Rigidbody2D>();

            StateMachine.Initialize(IdleState);

            EventListener.OnEnemyDamage += PlayerGetHurt;

            if (SceneLoadManager.Instance.MilkDrinked == false)
            {
                CC_PlayerHealth = 1;
            }
                
        }

        void Update()
        {
            StateMachine.CurrentState.Update();

            if (CC_PlayerHealth <= 0)
            {
                StateMachine.ChangeState(StopState);
            }
        }

        //玩家地面检测
        public bool GroundDetect()
        {
            RaycastHit2D[] Raycastall = Physics2D.RaycastAll(this.transform.position, this.transform.position - new Vector3(0, PlayerGroundDetectDistence));
            foreach (RaycastHit2D i in Raycastall)
            {
                if (i.collider.gameObject.CompareTag("Ground") == true)
                {
                    return true;
                }

            }

            return false;
        }

        //反转玩家的函数
        public void PlayerFlip(Vector2 _FilpDirection)
        {
            if (_FilpDirection.normalized.x == 0) return;
            if (FlipToRight == false && _FilpDirection.normalized.x < 0) return;
            if (FlipToRight == true && _FilpDirection.normalized.x > 0) return;

            //求出战斗区域和玩家的相对距离

            //向左反转
            if (FlipToRight == true)
            {

                this.GetComponent<SpriteRenderer>().flipX = true;
                PlayerAttackRange.transform.localPosition = new Vector3(-AttackRateDistance_ToPlayer, 0.604f, 0);
                FlipToRight = false;
            }

            else if (FlipToRight == false)
            {
                //向右反转
                this.GetComponent<SpriteRenderer>().flipX = false;
                PlayerAttackRange.transform.localPosition = new Vector3(AttackRateDistance_ToPlayer, 0.604f, 0);
                FlipToRight = true;
            }

        }

        //玩家受到伤害时调用
        public void PlayerGetHurt(float _Damage)
        {
            CC_PlayerHealth -= _Damage;
        }


        
        #region 回传Player相关参数

        public float GetCCPlayerHealth_Player() => CC_PlayerHealth;
        public float GetMoveSpeed_Player() => MoveSpeed;
        public float GetAttackRate_Player() => PlayerDamage;
        public float GetAttackTime_Player() => PlayerAttackTime;
        public float GetJumpForce_Player() => PlayerJumpForce;
        public float GetHealthPer_Player() => CC_PlayerHealth / PlayerMaxHealth;

        #endregion

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawLine(this.transform.position, this.transform.position - new Vector3(0, PlayerGroundDetectDistence));
        }
    }
}
