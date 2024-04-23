using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSitDownState : PlayerState
{
    public PlayerSitDownState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = 1.5f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0)
        {
            stateMachine.ChangeState(player.idleState);
        }

        /*if (rb.velocity.y > 0 )
        {
            stateMachine.ChangeState(player.idleState);
        }*/
    }
}
