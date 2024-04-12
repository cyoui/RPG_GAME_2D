using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine stateMachine;
    protected Enemy enemyBase;
    protected Rigidbody2D rb;


    protected bool trrigerCalled;
    protected float stateTimer;

    private string animBoolName;
    public EnemyState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName)
    {
        enemyBase = _enemyBase;
        stateMachine = _stateMachine;
        animBoolName = _animBoolName;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Enter()
    {
        enemyBase.ani.SetBool(animBoolName, true);
        trrigerCalled = false;

        rb = enemyBase.rb;
    }

    public virtual void Exit()
    {
        enemyBase.ani.SetBool(animBoolName, false);
        enemyBase.AssignLastAnimName(animBoolName);
    }

    public virtual void AnimationFinishTrriger()
    {
        trrigerCalled = true;
    }

}
