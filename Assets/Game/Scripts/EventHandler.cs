namespace SunnyFarm.Game
{
    using SunnyFarm.Game.DesignPattern;
    using SunnyFarm.Game.Inventory.Data;
    using System;
    using System.Collections.Generic;
    using static SunnyFarm.Game.Constant.Enums;

    public static class EventHandler
    {

        #region  Scene Load Events - in the order they are called
        // Before the scene is unloaded - Fade out event
        public static event Action OnBeforeSceneUnloadFadeOut;

        public static void CallOnBeforeSceneUnloadFadeOut()
        {
            if (OnBeforeSceneUnloadFadeOut != null)
            {
                OnBeforeSceneUnloadFadeOut?.Invoke();
            }
        }

        // Before the scene is unloaded event
        public static event Action OnBeforeSceneUnload;

        public static void CallOnBeforeSceneUnload()
        {
            if (OnBeforeSceneUnload != null)
            {
                OnBeforeSceneUnload?.Invoke();
            }
        }

        // After the scene is loaded event
        public static event Action OnAfterSceneLoad;

        public static void CallOnAfterSceneLoad()
        {
            if (OnAfterSceneLoad != null)
            {
                OnAfterSceneLoad?.Invoke();
            }
        }

        // After the scene is loaded - Fade in event
        public static event Action OnAfterSceneLoadFadeIn;

        public static void CallOnAfterSceneLoadFadeIn()
        {
            if (OnAfterSceneLoadFadeIn != null)
            {
                OnAfterSceneLoadFadeIn?.Invoke();
            }
        }
        #endregion

        #region Inventory Events
        // Inventory updated event
        public static event Action<InventoryLocation, List<InventoryItem>> OnInventoryUpdated;

        public static void CallOnInventoryUpdated(InventoryLocation location, List<InventoryItem> inventoryItems)
        {
            if (OnInventoryUpdated != null)
            {
                OnInventoryUpdated?.Invoke(location, inventoryItems);
            }
        }
        #endregion
    }
}
