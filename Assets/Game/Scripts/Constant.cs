namespace SunnyFarm.Game
{
    public class Constant
    {
        public static class Player
        {
            public static readonly string IS_RUNNING = "isRunning";

            public static readonly string IS_DIGGING = "isDigging";

            public static readonly string IS_AXING = "isAxing";

            public static readonly string IS_PICKAXING = "isPickaxing";

            public static readonly string IS_WATERING = "isWatering";

            public static readonly string INPUT_X = "InputX";

            public static readonly string INPUT_Y = "InputY";

            public static readonly string LAST_INPUT_X = "LastInputX";

            public static readonly string LAST_INPUT_Y = "LastInputY";
        }

        public static class Tag
        {
            public const string BoundConfiner = "BoundConfiner";

            public const string Player = "Player";
        }

        public static class ColorStat
        {
            public const float TargetAlpha = 0.5f;

            public const float FadeOutSeconds = 0.1f;

            public const float FadeInSeconds = 0.1f;
        }

        public static class Enums
        {
            public enum GridBoolProperty
            {
                Diggable,
                Droppable,

            }

            public enum SceneName
            {
                Scene1_Farm,
                Scene2_House
            }

            public enum ItemType
            {
                Tool,
                Seed,
                Crop,
                Commodity,
                Resource,
                Reapable_Scenary,
                None,
            }

            public enum InventoryLocation
            {
                Player,
                Chest,
                Count,
                None
            }

            public enum Season
            {
                Spring,
                Summer,
                Fall,
                Winter
            }

            public enum Weather
            {
                Sunny,
                Rainy,
                Snowny
            }

            public enum WeekDay
            {
                Mon,
                Tue,
                Wed,
                Thur,
                Fri,
                Sat,
                Sun
            }
        }

        public static class Inventory
        {
            public const int PlayerInventoryMinCapacity = 12;
            public const int PlayerInventoryMaxCapacity = 36;
        }

        public static class Time
        {
            public const float secondPerGameSecond = 0.012f;
        }
    }
}

