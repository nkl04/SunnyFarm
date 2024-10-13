namespace SunnyFarm.Game.Entities.Player
{
    using SunnyFarm.Game.StateMachine;
    using SunnyFarm.Game.Input;
    using SunnyFarm.Game.Managers.GameInput;
    using System;
    using SunnyFarm.Game.State.Player;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using SunnyFarm.Game.DesignPattern;
    using SunnyFarm.Game.Inventory;
    using TMPro;

    public class Player : Singleton<Player>
    {
        public Animator Animator => animator;

        public Rigidbody2D Rb2d => rb2d;

        public Vector2 MovementInput => movementInput;

        public Vector2 LastMovementInput { get => lastMovementInput; set => lastMovementInput = value; }

        public float WalkSpeed => walkSpeed;

        public float RunSpeed => runSpeed;

        public bool IsMovePressed { get; set; } = false;
        public bool IsAxePressed { get; set; } = false;
        public bool IsDigPressed { get; set; } = false;
        public bool IsPickaxePressed { get; set; } = false;
        public bool IsWaterPressed { get; set; } = false;
        public bool IsFacingRight { get; set; } = true;

        public bool CanToggleInventory { get; set; } = true;

        public StatePlayerIdle StatePlayerIdle { get; private set; }
        public StatePlayerMove StatePlayerMove { get; private set; }
        public StatePlayerAxe StatePlayerAxe { get; private set; }
        public StatePlayerDig StatePlayerDig { get; private set; }
        public StatePlayerPickaxe StatePlayerPickaxe { get; private set; }
        public StatePlayerWater StatePlayerWater { get; private set; }


        [SerializeField] private float walkSpeed = 5f;

        [SerializeField] private float runSpeed = 10f;


        private PlayerInputAction inputActions;

        private StateMachine<StatePlayer> stateMachine;

        private Vector2 movementInput;

        private Vector2 lastMovementInput;

        private Rigidbody2D rb2d;

        private Animator animator;

        private Camera mainCamera;


        private void Start()
        {
            mainCamera = Camera.main;

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

            inputActions.Player.ToggleInventory.started += OnToggleInventory;

            inputActions.Player.QuickSelectSlot.started += SelectInventorySlot;

            stateMachine.TransitionTo(new StatePlayerIdle(this, stateMachine)); // Set the initial state
        }

        private void SelectInventorySlot(InputAction.CallbackContext context)
        {
            var bindings = inputActions.Player.QuickSelectSlot.bindings;

            int bindingIndex = inputActions.Player.QuickSelectSlot.GetBindingIndexForControl(context.control);
        }

        private void OnToggleInventory(InputAction.CallbackContext context)
        {
            if (!CanToggleInventory) return;
            EventHandlers.CallOnToggleInventory();
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

        /// <summary>
        /// Get the viewport position of the player
        /// </summary>
        /// <returns></returns>
        public Vector3 GetViewportPosition()
        {
            // Vector3 Viewport position for player (0,0) is bottom left and (1,1) is top right
            return mainCamera.WorldToViewportPoint(transform.position);
        }
    }
}

