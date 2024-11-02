namespace SunnyFarm.Game.Entities.Player
{
    using SunnyFarm.Game.Input;
    using SunnyFarm.Game.Inventory;
    using SunnyFarm.Game.Inventory.Data;
    using SunnyFarm.Game.Managers.GameInput;
    using SunnyFarm.Game.State.Player;
    using SunnyFarm.Game.StateMachine;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using static SunnyFarm.Game.Constant.Enums;

    public class Player : MonoBehaviour
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

        public InventoryKey InventoryKey { get; private set; }

        [SerializeField] private float walkSpeed = 5f;

        [SerializeField] private float runSpeed = 10f;

        private GameInputManager gameInputManager;

        private PlayerInputAction inputActions;

        public StateMachine<StatePlayer> stateMachine;

        private Vector2 movementInput;

        private Vector2 lastMovementInput;

        private Rigidbody2D rb2d;

        private Animator animator;

        private Camera mainCamera;

        private void Start()
        {
            mainCamera = Camera.main;

            gameInputManager = GameInputManager.Instance;

            inputActions = gameInputManager.InputActions;

            stateMachine = new StateMachine<StatePlayer>();  // Create a new state machine

            rb2d = GetComponent<Rigidbody2D>();             // Get the Rigidbody2D component

            animator = GetComponentInChildren<Animator>();            // Get the Animator component in child Visual

            inputActions.Player.Move.performed += OnMoveInput;

            inputActions.Player.Move.canceled += OnMoveInput;

            stateMachine.TransitionTo(new StatePlayerIdle(this, stateMachine)); // Set the initial state

            // create inventory data for this player
            InventoryKey = new InventoryKey(InventoryLocation.Player, Random.Range(0, 999999));

            InventoryManager.Instance.AddInventory(InventoryKey);
        }

        private void OnMoveInput(InputAction.CallbackContext context)
        {
            if (!gameInputManager.CanPlayerKeyBoardInput) return;
            movementInput = context.ReadValue<Vector2>().normalized;

            IsMovePressed = movementInput.magnitude > 0;
        }

        private void Update()
        {
            stateMachine.Tick();
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

        public Vector2Int GetPlayerDirection()
        {
            Vector2Int dir;

            if (lastMovementInput.x > 0)
            {
                dir = new Vector2Int(1, 0);
            }
            else if (lastMovementInput.x < 0)
            {
                dir = new Vector2Int(-1, 0);
            }
            else if (lastMovementInput.y > 0)
            {
                dir = new Vector2Int(0, 1);
            }
            else if (lastMovementInput.y < 0)
            {
                dir = new Vector2Int(0, -1);
            }
            else
            {
                dir = new Vector2Int(0, 0);
            }

            return dir;
        }
    }
}

