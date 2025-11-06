using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DeadState : BaseState
{
    public Enemy_DeadState(AIBase npc) : base(npc)
    {
    }

    public override void Enter()
    {
        owner.Animator.SetTrigger(Setting.PlayerAnimationParameter_Enemy_IsDead);
    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }
}
