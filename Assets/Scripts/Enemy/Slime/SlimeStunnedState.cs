using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStunnedState : EnemyState
{
    private Enemy_Slime enemy;

    public SlimeStunnedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Enemy_Slime enemy) : base(enemyBase, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.fx.InvokeRepeating("RedColorBlink", 0, 0.1f);

        stateTimer = enemy.stunDuration;

        rb.velocity = new Vector2(-enemy.facingDir * enemy.stunDirection.x, enemy.stunDirection.y);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.fx.Invoke("CancelRedBlink", 0f);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0f)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
