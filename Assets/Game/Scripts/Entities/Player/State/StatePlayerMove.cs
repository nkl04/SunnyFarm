namespace SunnyFarm.Game.DesignPattern.StateMachine
{
    using SunnyFarm.Game.Entities.Player;
    using UnityEditor.U2D.Aseprite;
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
        }

        public override void Exit()
        {
        }

        public override void FixedTick()
        {

        }

        public override void Tick()
        {
            Debug.Log("Player is moving");

            Vector2 movement = this.player.MovementInput * this.player.WalkSpeed;

            this.player.Rb2d.velocity = movement;

            CheckSwitchState();
        }
    }
}