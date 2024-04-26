using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : Entity
{
    [Header("UI")]
    public Image healthBar;

    [Header("Attack info")]
    public Vector2[] attackMovement;
    protected override int attackDamage => 10;

    public bool isBusy { get; private set; }

    bool isplayerDead = false;

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

        StartCoroutine(CheckDead());

        if (isplayerDead) return;
    }

    IEnumerator CheckDead()
    {
        // 땅으로 떨어지면
        if (transform.position.y < -8)
        {
            SceneManager.LoadScene("SampleScene");
        }

        // 체력 0이하
        if (currentHealth == 0)
        {
            isplayerDead = true;
            ZeroVelocity();

            anim.SetBool("Die", true);

            //GetComponent<Collider2D>().enabled = false; // 충돌체 비활성화


            yield return new WaitForSeconds(2);

            SceneManager.LoadScene("SampleScene");
        }
    }

    // 체력이 감소할 때마다 healthBar의 filled 이미지를 조절하는 메서드
    public override void Damage()
    {
        base.Damage();

        if (healthBar != null)
        {
            // 체력을 감소한 비율로 filled 이미지 조절
            healthBar.fillAmount = (float)currentHealth / maxHealth;
        }
    }

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;

        yield return new WaitForSeconds(_seconds);

        isBusy = false;
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
}
