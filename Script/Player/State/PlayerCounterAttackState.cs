using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private bool canCreateClone;

    public PlayerCounterAttackState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        canCreateClone = true;
        stateTimer = player.counterAttackDuration;
        player.ani.SetBool("SucessfulCounterAttack", false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().CanBeStunned())
                {
                    stateTimer = 10;
                    player.ani.SetBool("SucessfulCounterAttack", true);

                    player.skill.parry.UseSkill();

                    if (canCreateClone)
                    {
                        canCreateClone = false;
                        player.skill.parry.MakeMirrageOnParry(hit.transform);
                    }
                }
            }
        }

        if (stateTimer < 0 || trigerCalled)
            stateMachine.ChangeState(player.idleState);

    }
}
