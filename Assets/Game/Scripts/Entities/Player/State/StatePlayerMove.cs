namespace SunnyFarm.Game.DesignPattern.StateMachine
{
    using SunnyFarm.Game.Entities;
    using UnityEngine;

    public class StatePlayerMove : StatePlayer
    {
        public StatePlayerMove(Player player, StateMachine<StatePlayer> stateMachine) : base(player, stateMachine)
        { }

        public override void CheckSwitchState()
        {
            if (!this.player.IsMovePressed)
            {
                this.stateMachine.TransitionTo(new StatePlayerIdle(this.player, this.stateMachine));
            }
        }

        public override void Enter()
        {
            this.player.Animator.SetBool(Constant.Player.IS_RUNNING, true);
        }

        public override void Exit()
        {
            this.player.Animator.SetBool(Constant.Player.IS_RUNNING, false);
        }

        public override void Tick()
        {
            Vector2 movement = this.player.MovementInput * this.player.WalkSpeed;

            this.player.Rb2d.velocity = movement;

            this.player.Animator.SetFloat(Constant.Player.INPUT_X, this.player.MovementInput.x);

            this.player.Animator.SetFloat(Constant.Player.INPUT_Y, this.player.MovementInput.y);

            if (this.player.MovementInput != Vector2.zero)
            {
                this.player.Animator.SetFloat(Constant.Player.LAST_INPUT_X, this.player.MovementInput.x);

                this.player.Animator.SetFloat(Constant.Player.LAST_INPUT_Y, this.player.MovementInput.y);
            }

            if (this.player.MovementInput.x > 0 && !this.player.IsFacingRight)
            {
                this.player.Flip();

                this.player.IsFacingRight = true;
            }
            else if (this.player.MovementInput.x < 0 && this.player.IsFacingRight)
            {
                this.player.Flip();

                this.player.IsFacingRight = false;
            }

            CheckSwitchState();
        }
    }
}