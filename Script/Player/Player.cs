using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    private Player player;

    public SkillManager skill {  get; private set; }
    public GameObject sword {  get; private set; }

    public bool isBusy {  get; private set; }

    [Header("Attack Details")]
    public Vector2[] attackMovement;
    //public float attackSpeed;
    public float counterAttackDuration = .2f;

    [Header("Move Info")]
    public float moveSpeed;
    public float jumpForce;
    public float swordReturnImpact;
    private float defaultMoveSpeed;
    private float defaultJumpForce;

    [Header("Dash Info")]
    public float dashSpeed;
    public float dashDuration;
    public float dashingDir {  get; set; }
    private float defaultDashSpeed;

    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState {  get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }
    public PlayerPrimaryAttackState primaryAttack { get; private set; }
    public PlayerAimSwordState aimSwordState { get; private set; }
    public PlayerCatchSwordState catchSwordState { get; private set; }
    public PlayerBlackholeState blackholeState { get; private set; }
    public PlayerDeadState deadState { get; private set; }

    #endregion

    override protected void Awake()
    {
        base.Awake();

        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
        primaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        aimSwordState = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSwordState = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        blackholeState = new PlayerBlackholeState(this, stateMachine, "Jump");
        deadState = new PlayerDeadState(this, stateMachine, "Die");
    }

    override protected void Start()
    {
        base.Start();

        skill = SkillManager.instance;

        stateMachine.Initialize(idleState);

        player = PlayerManager.instance.player;

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;
    }

    override protected void Update()
    {
        if (Time.timeScale == 0)
            return;

        base.Update();

        stateMachine.currentState.Update();

        DashInputCheck();

        if (Input.GetKeyDown(KeyCode.F) && skill.crystal.crystalUnlocked)
            skill.crystal.CanUseSkill();

        if (Input.GetKeyDown(KeyCode.Alpha1))
            Inventory.instance.UseFlask();
    }

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = (1 - _slowPercentage) * moveSpeed;
        jumpForce = (1 - _slowPercentage) * jumpForce;
        dashSpeed = (1 - _slowPercentage) * dashSpeed;
        ani.speed = (1 - _slowPercentage) * ani.speed;

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }


    public void AnimationTrriger() => stateMachine.currentState.AnimationFinishTrriger();

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;

        yield return new WaitForSeconds(_seconds);

        isBusy = false;
    }

    #region Dash Methods
    public void DashInputCheck()
    {
        if (IsWallDetected())
            return;

        if (skill.dash.dashUnlocked == false)
            return;

        if (player.GetComponent<PlayerStats>().isdead)
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill())
        {
            dashingDir = Input.GetAxisRaw("Horizontal");

            if (dashingDir == 0)
                dashingDir = facingDir;

            stateMachine.ChangeState(dashState);
        }
    }
    #endregion

    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }

    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSwordState);
        Destroy(sword);
    }


    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }

    protected override void SetupZeroKnockbackPower()
    {
        knockbackPower = new Vector2(0, 0);
    }
}
