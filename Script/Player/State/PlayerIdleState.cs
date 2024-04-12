using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        player.SetZeroVelocity();

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (xInput != 0 && !player.isBusy)
        {
            stateMachine.ChangeState(player.moveState);
        }
    }
}
