using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_IdleState : BaseState
{
    public Enemy_IdleState(AIBase npc) : base(npc)
    {
    }

    public override void Enter()
    {
        owner.Animator.SetBool(Setting.PlayerAnimationParameter_Enemy_IsMove, false);
    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }
}
