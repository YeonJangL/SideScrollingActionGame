using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerGroundState
{
    public PlayerRunState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(xInput * player.moveSpeed * 1.2f, rb.velocity.y);

        if (xInput == 0 || player.IsWallDetected() || Input.GetKeyUp(KeyCode.LeftShift))
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}