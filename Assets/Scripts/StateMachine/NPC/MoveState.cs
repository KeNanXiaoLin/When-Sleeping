using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : BaseState
{
    private E_Direction lastFace;
    public MoveState(NPCController npc) : base(npc)
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
        owner.UpdateMovement();
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
                owner.Animator.Play(Setting.AnimationName_MoveRight);
                break;
            case E_Direction.Left:
                owner.Animator.Play(Setting.AnimationName_MoveLeft);
                break;
            case E_Direction.Up:
                owner.Animator.Play(Setting.AnimationName_MoveUp);
                break;
            case E_Direction.Down:
                owner.Animator.Play(Setting.AnimationName_MoveDown);
                break;
        }
    }
}
