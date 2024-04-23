using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [Header("Attack info")]
    public Vector2[] attackMovement;

    public bool isBusy { get; private set; }

    [Header("Move info")]
    public float moveSpeed = 5f;
    public float jumpForce;

    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerAttack playerattack { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerWallSlideState wallSlide { get; private set; }
    public PlayerWallJumpState wallJump { get; private set; }
    public PlayerSitDownState sitDown { get; private set; }
    public PlayerRunState runState { get; private set; }
    public PlayerDieState dieState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Walk");
        playerattack = new PlayerAttack(this, stateMachine, "Attack");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        wallSlide = new PlayerWallSlideState(this, stateMachine, "Fall");
        wallJump = new PlayerWallJumpState(this, stateMachine, "Jump");
        sitDown = new PlayerSitDownState(this, stateMachine, "SitDown");
        runState = new PlayerRunState(this, stateMachine, "Run");
        dieState = new PlayerDieState(this, stateMachine, "Die");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
    }

    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(dieState);
    }

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;

        yield return new WaitForSeconds(_seconds);

        isBusy = false;
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
}
