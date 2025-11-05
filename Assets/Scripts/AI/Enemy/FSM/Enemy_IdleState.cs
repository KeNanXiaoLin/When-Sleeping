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
        owner.Animator.Play(Setting.AnimationName_Enemy_Idle);
    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }
}
