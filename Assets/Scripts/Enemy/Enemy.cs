using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : Entity
{
    [SerializeField]
    protected LayerMask whatIsPlayer;

    [Header("Stunned info")]
    public float stunDuration;
    public Vector2 stunDirection;
    protected bool canBeStunned;

    [SerializeField] protected GameObject counterImage;

    [Header("Move info")]
    public float moveSpeed;
    public float idleTime;
    public float battleTime;

    [Header("Attack info")]
    public float attackDistance;
    public float attackCooldown;
    [HideInInspector] public float lastTimeAttacked;
    public int enemyAttackDamage;

    protected override int attackDamage => 10;


    bool isenemyDead = false;

    public EnemyStateMachine stateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();

        StartCoroutine(CheckDead());

        if (isenemyDead) return;
    }

    IEnumerator CheckDead()
    {
        // 땅으로 떨어지면
        if (transform.position.y < -8)
        {
            Destroy(gameObject);
        }

        // 체력 0이하
        if (currentHealth <= 0)
        {
            isenemyDead = true;
            anim.SetBool("Die", true);

            Destroy(gameObject);
            //GetComponent<Collider2D>().enabled = false; // 충돌체 비활성화

            //Destroy(gameObject, 2f);

            yield return new WaitForSeconds(2);
        }
    }

    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }

    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }

    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, whatIsPlayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
    }
}
