using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

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

        [Header("跳跃相关")]
        [SerializeField] private float PlayerJumpForce;
        [SerializeField] private float PlayerGroundDetectDistence;

        #region 状态机相关

        public PlayerStateMescine StateMachine { get; private set; }

        public PlayerIdleState IdleState { get; private set; }
        public PlayerMoveState MoveState { get; private set; }
        public PlayerJumpState JumpState { get; private set; }
        public PlayerAttackState AttackState { get; private set; }
        public PlayerInsteractState InsteractState { get; private set; }
        #endregion

        private void Awake()
        {
            StateMachine = GetComponent<PlayerStateMescine>();

            IdleState = new PlayerIdleState(StateMachine, this, "Idle");
            MoveState = new PlayerMoveState(StateMachine, this, "Move");
            JumpState = new PlayerJumpState(StateMachine, this, "Jump");
            AttackState = new PlayerAttackState(StateMachine, this, "Attack");
            InsteractState = new PlayerInsteractState(StateMachine, this, "Insteract");

            CC_PlayerHealth = PlayerMaxHealth;

            BattleSceneManager.instence.Player = this.gameObject;
        }

        void Start()
        {
            anim = GetComponent<Animator>();
            rd = GetComponent<Rigidbody2D>();

            StateMachine.Initialize(IdleState);

            EventListener.OnEnemyDamage += PlayerGetHurt;
        }

        void Update()
        {
            StateMachine.CurrentState.Update();
        }

        public bool GroundDetect()
        {
            var Raycastall = Physics2D.RaycastAll(this.transform.position, this.transform.position - new Vector3(0, PlayerGroundDetectDistence));
            if (Raycastall.Length > 1) return true;
            else return false;
        }

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
        public float GetGroundDetectDistence_Player() => PlayerGroundDetectDistence;

        #endregion

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawLine(this.transform.position, this.transform.position - new Vector3(0, PlayerGroundDetectDistence));
        }
    }
}
