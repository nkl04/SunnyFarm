namespace SunnyFarm.Game.Entities.Player
{
    using UnityEngine;
    using SunnyFarm.Game.DesignPattern.StateMachine;
    using Unity.VisualScripting;
    using SunnyFarm.Game.Input;
    using UnityEngine.InputSystem;
    using SunnyFarm.Game.Managers.GameInput;
    using System;

    public class Player : MonoBehaviour
    {
        public readonly string IS_RUNNING = "isRunning";

        public readonly string IS_DIGGING = "isDigging";

        public readonly string IS_AXING = "isAxing";

        public readonly string IS_PICKAXING = "isPickaxing";

        public readonly string IS_WATERING = "isWatering";

        public readonly string INPUT_X = "InputX";

        public readonly string INPUT_Y = "InputY";

        public readonly string LAST_INPUT_X = "LastInputX";

        public readonly string LAST_INPUT_Y = "LastInputY";

        public Animator Animator => animator;

        public Rigidbody2D Rb2d => rb2d;

        public Vector2 MovementInput => movementInput;

        public float WalkSpeed => walkSpeed;

        public float RunSpeed => runSpeed;

        public bool IsMovePressed { get; set; } = false;

        public bool IsAxePressed { get; set; } = false;

        public bool IsDigPressed { get; set; } = false;

        public bool IsPickaxePressed { get; set; } = false;

        public bool IsWaterPressed { get; set; } = false;

        public bool IsFacingRight { get; set; } = true;

        [SerializeField] private float walkSpeed = 5f;

        [SerializeField] private float runSpeed = 10f;

        private PlayerInputAction inputActions;

        private StateMachine<StatePlayer> stateMachine;

        private Vector2 movementInput;

        private Rigidbody2D rb2d;

        private Animator animator;

        private void Start()
        {
            inputActions = GameInputManager.Instance.InputActions;

            stateMachine = new StateMachine<StatePlayer>();  // Create a new state machine

            rb2d = GetComponent<Rigidbody2D>();             // Get the Rigidbody2D component

            animator = GetComponentInChildren<Animator>();            // Get the Animator component in child Visual

            inputActions.Player.Move.performed += OnMoveInput;

            inputActions.Player.Move.canceled += OnMoveInput;

            inputActions.Player.Axe.performed += OnAxeInput;

            inputActions.Player.Axe.canceled += OnAxeInput;

            inputActions.Player.Dig.performed += OnDigInput;

            inputActions.Player.Dig.canceled += OnDigInput;

            inputActions.Player.Pickaxe.performed += OnPickaxeInput;

            inputActions.Player.Pickaxe.canceled += OnPickaxeInput;

            inputActions.Player.Water.performed += OnWaterInput;

            inputActions.Player.Water.canceled += OnWaterInput;

            stateMachine.TransitionTo(new StatePlayerIdle(this, stateMachine)); // Set the initial state
        }

        private void OnWaterInput(InputAction.CallbackContext context)
        {
            IsWaterPressed = context.ReadValueAsButton();
        }

        private void OnPickaxeInput(InputAction.CallbackContext context)
        {
            IsPickaxePressed = context.ReadValueAsButton();
        }

        private void OnDigInput(InputAction.CallbackContext context)
        {
            IsDigPressed = context.ReadValueAsButton();
        }

        private void OnAxeInput(InputAction.CallbackContext context)
        {
            IsAxePressed = context.ReadValueAsButton();
        }

        private void OnMoveInput(InputAction.CallbackContext context)
        {
            movementInput = context.ReadValue<Vector2>().normalized;

            IsMovePressed = movementInput.magnitude > 0;

        }

        private void Update()
        {
            stateMachine.Tick();    // Update the current state
        }

        public void Flip()
        {
            transform.Rotate(0f, 180f, 0f);
        }

    }
}

