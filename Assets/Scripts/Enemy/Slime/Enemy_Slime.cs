using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Slime : Enemy
{
    #region States
    public SlimeIdleState idleState { get; private set; }
    public SlimeAttackState attackState { get; private set; }
    public SlimeBattleState battleState { get; private set; }
    public SlimeMoveState moveState { get; private set; }
    public SlimeStunnedState stunnedState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new SlimeIdleState(this, stateMachine, "Idle", this);
        attackState = new SlimeAttackState(this, stateMachine, "Attack", this);
        battleState = new SlimeBattleState(this, stateMachine, "Jump", this);
        moveState = new SlimeMoveState(this, stateMachine, "Move", this);
        stunnedState = new SlimeStunnedState(this, stateMachine, "Stun", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.U))
        {
            stateMachine.ChangeState(stunnedState);
        }
    }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }
        return false;
    }
}
