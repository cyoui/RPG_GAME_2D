using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    public int comboCounter {  get; private set; }

    private float lastTimeAttacked;
    private float comboWindow = 2;


    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        xInput = 0;//you need it to fix the direction of Attacking

        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
            comboCounter = 0;

        player.ani.SetInteger("ComboCounter", comboCounter);

        float attackDir = player.facingDir;

        if(xInput != 0)
            attackDir = xInput;

        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);

        //player.ani.speed = attackSpeed;

        stateTimer = .1f;
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", .15f);

        //player.ani.speed = 1;

        comboCounter++;
        lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            player.SetZeroVelocity();

        if (trigerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
