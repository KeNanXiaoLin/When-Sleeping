using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace GJ
{

    public class Player : MonoBehaviour
    {
        public Rigidbody2D rd { get; private set; }
        public Animator anim { get; private set; }

        public PlayerStateMescine StateMachine { get; private set; }
        
        public PlayerIdleState IdleState { get; private set; }
        public PlayerInsteractState InsteractState{ get; private set; }
        public PlayerAttackState AttackState{ get; private set; }

        private void Awake()
        {
            StateMachine = new PlayerStateMescine();

            IdleState = new PlayerIdleState(StateMachine, this, "Idle");
            InsteractState = new PlayerInsteractState(StateMachine, this, "Insteract");
            AttackState = new PlayerAttackState(StateMachine, this, "Attack");
        }

        void Start()
        {
            anim = GetComponent<Animator>();
            rd = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            StateMachine.CurrentState.Update();
        }
    }
}
