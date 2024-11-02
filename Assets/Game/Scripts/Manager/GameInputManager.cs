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

            inputActions.Player.QuickSelectInventorySlot0.started += context => EventHandlers.CallOnQuickSelectSlot(0);

            inputActions.Player.QuickSelectInventorySlot1.started += context => EventHandlers.CallOnQuickSelectSlot(1);

            inputActions.Player.QuickSelectInventorySlot2.started += context => EventHandlers.CallOnQuickSelectSlot(2);

            inputActions.Player.QuickSelectInventorySlot3.started += context => EventHandlers.CallOnQuickSelectSlot(3);

            inputActions.Player.QuickSelectInventorySlot4.started += context => EventHandlers.CallOnQuickSelectSlot(4);

            inputActions.Player.QuickSelectInventorySlot5.started += context => EventHandlers.CallOnQuickSelectSlot(5);

            inputActions.Player.QuickSelectInventorySlot6.started += context => EventHandlers.CallOnQuickSelectSlot(6);

            inputActions.Player.QuickSelectInventorySlot7.started += context => EventHandlers.CallOnQuickSelectSlot(7);

            inputActions.Player.QuickSelectInventorySlot8.started += context => EventHandlers.CallOnQuickSelectSlot(8);

            inputActions.Player.QuickSelectInventorySlot9.started += context => EventHandlers.CallOnQuickSelectSlot(9);

            inputActions.Player.QuickSelectInventorySlot10.started += context => EventHandlers.CallOnQuickSelectSlot(10);

            inputActions.Player.QuickSelectInventorySlot11.started += context => EventHandlers.CallOnQuickSelectSlot(11);


            inputActions.Player.MouseSroll.performed += OnMouseScroll;
        }

        private void OnMouseScroll(InputAction.CallbackContext context)
        {
            if (!CanMouseScroll) return;

            Vector2 scrollValue = context.ReadValue<Vector2>();

            EventHandlers.CallOnMouseScroll(scrollValue.y);
        }

        private void OnToggleInventory(InputAction.CallbackContext context)
        {
            if (!CanToggleInventory) return;
            EventHandlers.CallOnToggleInventory();
        }
    }
}
