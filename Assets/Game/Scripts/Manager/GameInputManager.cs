namespace SunnyFarm.Game.Managers.GameInput
{
    using SunnyFarm.Game.DesignPattern;
    using SunnyFarm.Game.Input;

    public class GameInputManager : Singleton<GameInputManager>
    {
        public PlayerInputAction InputActions => inputActions;

        private PlayerInputAction inputActions;

        protected override void Awake()
        {
            base.Awake();

            inputActions = new PlayerInputAction();
        }

        private void OnEnable()
        {
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }
    }
}
