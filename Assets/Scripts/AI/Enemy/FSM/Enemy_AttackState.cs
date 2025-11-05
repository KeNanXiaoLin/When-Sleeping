using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AttackState : BaseState
{
    public Enemy_AttackState(AIBase npc) : base(npc)
    {
    }

    public override void Enter()
    {
        owner.Animator.Play(Setting.AnimationName_Enemy_Attack);
    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }
}
