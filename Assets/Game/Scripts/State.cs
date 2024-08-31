namespace SunnyFarm.Game.DesignPattern.StateMachine
{
    public abstract class State
    {
        protected StateMachine stateMachine;
        protected FactoryPlayerState factoryPlayerState;

        public abstract void Enter();
        public abstract void Tick();
        public abstract void Exit();

    }
}
