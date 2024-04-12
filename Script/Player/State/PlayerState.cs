using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    protected Rigidbody2D rb;
    protected float xInput;
    protected float yInput;
    protected float stateTimer;
    protected bool trigerCalled;

    private string animBoolName;

    public PlayerState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName)
    {
        this.stateMachine = _playerStateMachine;
        this.player = _player;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        player.ani.SetBool(animBoolName, true);
        rb = player.rb;
        trigerCalled = false;
    }

    public virtual void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        player.ani.SetFloat("yVelocity", rb.velocity.y);

        stateTimer -=Time.deltaTime;
    }

    public virtual void Exit()
    {
        player.ani.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrriger()
    {
        trigerCalled = true;
    }

}
