namespace SunnyFarm.Game.Managers.GameInput
{
    using SunnyFarm.Game.DesignPattern;
    using SunnyFarm.Game.Input;
    using System;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class GameInputManager : Singleton<GameInputManager>
    {
        public PlayerInputAction InputActions => inputActions;
        public bool CanToggleInventory { get; set; } = true;
        public bool CanMouseScroll { get; set; } = true;
        public bool CanPlayerKeyBoardInput { get; set; } = true;

        private PlayerInputAction inputActions;

        private void OnEnable()
        {
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        protected override void Awake()
        {
            base.Awake();

            inputActions = new PlayerInputAction();

            inputActions.Player.ToggleInventory.started += OnToggleInventory;

            inputActions.Player.QuickSelectSlot.started += SelectInventorySlot;

            inputActions.Player.MouseSroll.performed += OnMouseScroll;
        }

        private void OnMouseScroll(InputAction.CallbackContext context)
        {
            if (!CanMouseScroll) return;

            Vector2 scrollValue = context.ReadValue<Vector2>();

            EventHandlers.CallOnMouseScroll(scrollValue.y);
        }

        private void SelectInventorySlot(InputAction.CallbackContext context)
        {
            var bindings = inputActions.Player.QuickSelectSlot.bindings;

            int bindingIndex = inputActions.Player.QuickSelectSlot.GetBindingIndexForControl(context.control);

            EventHandlers.CallOnQuickSelectSlot(bindingIndex);
        }

        private void OnToggleInventory(InputAction.CallbackContext context)
        {
            if (!CanToggleInventory) return;
            EventHandlers.CallOnToggleInventory();
        }
    }
}
