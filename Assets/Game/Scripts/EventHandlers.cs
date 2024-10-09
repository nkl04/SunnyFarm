namespace SunnyFarm.Game
{
    using SunnyFarm.Game.DesignPattern;
    using SunnyFarm.Game.Inventory.Data;
    using SunnyFarm.Game.Inventory.UI;
    using System;
    using System.Collections.Generic;
    using static SunnyFarm.Game.Constant.Enums;

    public static class EventHandlers
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
            OnInventoryUpdated?.Invoke(location, inventoryItems);

        }

        public static event Action<UIInventorySlot> OnItemDroppedOn,
                                                    OnItemBeginDrag,
                                                    OnItemDrag,
                                                    OnItemEndDrag,
                                                    OnItemHover,
                                                    OnItemEndHover;

        public static void CallOnItemDroppedOn(UIInventorySlot item)
        {
            OnItemDroppedOn?.Invoke(item);
        }

        public static void CallOnItemBeginDrag(UIInventorySlot item)
        {
            OnItemBeginDrag?.Invoke(item);
        }

        public static void CallOnItemDrag(UIInventorySlot item)
        {
            OnItemDrag?.Invoke(item);
        }

        public static void CallOnItemEndDrag(UIInventorySlot item)
        {
            OnItemEndDrag?.Invoke(item);
        }

        public static void CallOnItemHover(UIInventorySlot item)
        {
            OnItemHover?.Invoke(item);
        }

        public static void CallOnItemEndHover(UIInventorySlot item)
        {
            OnItemEndHover?.Invoke(item);
        }

        public static event Action<UIInventoryItemKeyData, UIInventoryItemKeyData> OnSwapItems;

        public static void CallOnSwapItems(UIInventoryItemKeyData from, UIInventoryItemKeyData to)
        {
            OnSwapItems?.Invoke(from, to);
        }

        #endregion

        #region Time Events

        #region Minute Events
        /// <summary>
        /// Event to advance the game time by a minute
        /// </summary>
        public static event Action<int, Season, int, WeekDay, int, int, int> OnAdvanceGameMinute;

        /// <summary>
        /// Call the advance game minute event
        /// </summary>
        /// <param name="year"></param>
        /// <param name="season"></param>
        /// <param name="day"></param>
        /// <param name="dayOfWeek"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        public static void CallAdvanceGameMinute(int year, Season season, int day, WeekDay dayOfWeek, int hour, int minute, int second)
        {
            if (OnAdvanceGameMinute != null)
            {
                OnAdvanceGameMinute?.Invoke(year, season, day, dayOfWeek, hour, minute, second);
            }
        }
        #endregion

        #region Hour Events

        /// <summary>
        /// Event to advance the game time by an hour
        /// </summary>
        public static event Action<int, Season, int, WeekDay, int, int, int> OnAdvanceGameHour;

        /// <summary>
        /// Call the advance game hour event
        /// </summary>
        /// <param name="year"></param>
        /// <param name="season"></param>
        /// <param name="day"></param>
        /// <param name="dayOfWeek"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        public static void CallAdvanceGameHour(int year, Season season, int day, WeekDay dayOfWeek, int hour, int minute, int second)
        {
            if (OnAdvanceGameHour != null)
            {
                OnAdvanceGameHour?.Invoke(year, season, day, dayOfWeek, hour, minute, second);
            }
        }
        #endregion

        #region Day Events
        /// <summary>
        /// Event to advance the game time by a day
        /// </summary>
        public static event Action<int, WeekDay, int, Season, int, int, int> OnAdvanceGameDay;

        /// <summary>
        /// Call the advance game day event
        /// </summary>
        /// <param name="year"></param>
        /// <param name="season"></param>
        /// <param name="day"></param>
        /// <param name="dayOfWeek"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        public static void CallAdvanceGameDay(int year, Season season, int day, WeekDay dayOfWeek, int hour, int minute, int second)
        {
            if (OnAdvanceGameDay != null)
            {
                OnAdvanceGameDay?.Invoke(year, dayOfWeek, hour, season, day, minute, second);
            }
        }
        #endregion

        #region Season Events
        /// <summary>
        /// Event to advance the game time by a season
        /// </summary>
        public static event Action<int, WeekDay, int, Season, int, int, int> OnAdvanceGameSeason;

        /// <summary>
        /// Call the advance game season event
        /// </summary>
        /// <param name="year"></param>
        /// <param name="season"></param>
        /// <param name="day"></param>
        /// <param name="dayOfWeek"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        public static void CallAdvanceGameSeason(int year, Season season, int day, WeekDay dayOfWeek, int hour, int minute, int second)
        {
            if (OnAdvanceGameSeason != null)
            {
                OnAdvanceGameSeason?.Invoke(year, dayOfWeek, hour, season, day, minute, second);
            }
        }
        #endregion

        #region Year Events
        /// <summary>
        /// Event to advance the game time by a season
        /// </summary>
        public static event Action<int, WeekDay, int, Season, int, int, int> OnAdvanceGameYear;

        /// <summary>
        /// Call the advance game year event
        /// </summary>
        /// <param name="year"></param>
        /// <param name="season"></param>
        /// <param name="day"></param>
        /// <param name="dayOfWeek"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        public static void CallAdvanceGameYear(int year, Season season, int day, WeekDay dayOfWeek, int hour, int minute, int second)
        {
            if (OnAdvanceGameYear != null)
            {
                OnAdvanceGameYear?.Invoke(year, dayOfWeek, hour, season, day, minute, second);
            }
        }
        #endregion
        #endregion
    }
}
