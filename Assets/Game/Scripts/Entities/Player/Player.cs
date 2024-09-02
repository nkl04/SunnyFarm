namespace SunnyFarm.Game.Entities.Player
{
    using UnityEngine;
    using SunnyFarm.Game.DesignPattern.StateMachine;
    using Unity.VisualScripting;
    using SunnyFarm.Game.Input;
    using UnityEngine.InputSystem;
    using System;

    public class Player : MonoBehaviour
    {
        public Rigidbody2D Rb2d => rb2d;

        public Vector2 MovementInput => movementInput;

        public float WalkSpeed => walkSpeed;

        public float RunSpeed => runSpeed;

        public bool IsMovePressed { get; set; } = false;

        [SerializeField] private float walkSpeed = 5f;

        [SerializeField] private float runSpeed = 10f;

        private PlayerInputAction inputActions;

        private StateMachine<StatePlayer> stateMachine;

        private Vector2 movementInput;

        private Rigidbody2D rb2d;

        private void Awake()
        {
            inputActions = new PlayerInputAction();         // Create a new PlayerInputAction 

            stateMachine = new StateMachine<StatePlayer>();  // Create a new state machine

            rb2d = GetComponent<Rigidbody2D>();             // Get the Rigidbody2D component

            inputActions.Enable();

            inputActions.Player.Move.performed += OnMoveInput;

            inputActions.Player.Move.canceled += OnMoveInput;

            stateMachine.TransitionTo(new StatePlayerIdle(this, stateMachine)); // Set the initial state
        }

        private void OnEnable()
        {
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
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

        private void FixedUpdate()
        {
            stateMachine.FixedTick();   // Update the current state
        }



    }
}

