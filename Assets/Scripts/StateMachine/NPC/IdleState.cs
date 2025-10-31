using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    private E_Direction lastFace;
    public IdleState(NPCController npc) : base(npc)
    {
    }

    public override void Enter()
    {
        lastFace = owner.Facing;
        ChooseFaceAnim();
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        if (lastFace != owner.Facing)
        {
            ChooseFaceAnim();
            lastFace = owner.Facing;
        }
    }

    private void ChooseFaceAnim()
    {
        switch (owner.Facing)
        {
            case E_Direction.Right:
                owner.Animator.Play(Setting.AnimationName_IdleRight);
                break;
            case E_Direction.Left:
                owner.Animator.Play(Setting.AnimationName_IdleLeft);
                break;
            case E_Direction.Up:
                owner.Animator.Play(Setting.AnimationName_IdleUp);
                break;
            case E_Direction.Down:
                owner.Animator.Play(Setting.AnimationName_IdleDown);
                break;
        }
    }
}
