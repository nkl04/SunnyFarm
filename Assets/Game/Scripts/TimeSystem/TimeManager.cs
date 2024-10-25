namespace SunnyFarm.Game.Managers
{
    using SunnyFarm.Game;
    using SunnyFarm.Game.DesignPattern;
    using UnityEngine;
    using UnityEngine.Rendering.Universal;
    using static SunnyFarm.Game.Constant.Enums;

    public class TimeManager : Singleton<TimeManager>
    {
        private int year = 1;
        private int day = 1;
        private Season season = Season.Spring;
        private WeekDay weekDay = WeekDay.Mon;
        private int hour = 6;
        private int minute = 0;
        private int second = 0;
        private bool isPaused = false;
        private float gameTick = 0f;

        private void Start()
        {
            EventHandlers.CallAdvanceGameMinute(year, season, day, weekDay, hour, minute, second);
        }

        private void Update()
        {
            if (!isPaused)
            {
                GameTick();
            }

            if (Input.GetKeyDown(KeyCode.M) & Input.GetKey(KeyCode.LeftControl))
            {
                TestAdvanceMinute();
            }

            if (Input.GetKeyDown(KeyCode.N) & Input.GetKey(KeyCode.LeftControl))
            {
                TestAdvanceDay();
            }

            if (Input.GetKeyDown(KeyCode.H) & Input.GetKey(KeyCode.LeftControl))
            {
                TestAdvanceHour();
            }
        }

        private void GameTick()
        {
            gameTick += Time.deltaTime;

            if (gameTick >= Constant.Time.secondPerGameSecond)
            {
                gameTick -= Constant.Time.secondPerGameSecond;

                UpdateGameSecond();
            }

        }

        private void UpdateGameSecond()
        {
            second++;

            if (second > 59)
            {
                second = 0;
                UpdateGameMinute();
            }
        }

        private void UpdateGameMinute()
        {
            minute++;

            if (minute > 59)
            {
                minute = 0;
                UpdateGameHour();
            }

            EventHandlers.CallAdvanceGameMinute(year, season, day, weekDay, hour, minute, second);
        }

        private void UpdateGameHour()
        {
            hour++;

            if (hour > 23)
            {
                hour = 0;
                UpdateGameDay();
            }

            EventHandlers.CallAdvanceGameHour(year, season, day, weekDay, hour, minute, second);
        }

        private void UpdateGameDay()
        {
            day++;

            weekDay = (WeekDay)(((int)season * 28 + day - 1) % 7);

            if (day > 28)
            {
                day = 1;

                UpdateGameSeason();
            }

            EventHandlers.CallAdvanceGameDay(year, season, day, weekDay, hour, minute, second);
        }

        private void UpdateGameSeason()
        {
            season++;

            if (season > Season.Winter)
            {
                season = Season.Spring;
                UpdateGameYear();
            }

            EventHandlers.CallAdvanceGameSeason(year, season, day, weekDay, hour, minute, second);
        }

        private void UpdateGameYear()
        {
            year++;
            EventHandlers.CallAdvanceGameYear(year, season, day, weekDay, hour, minute, second);
        }

        //TODO: Remove
        /// <summary>
        /// Advance 1 game minute
        /// </summary>
        public void TestAdvanceMinute()
        {
            for (int i = 0; i < 60; i++)
            {
                UpdateGameSecond();
            }
        }

        //TODO: Remove
        /// <summary>
        /// Advance 1 game hour
        /// </summary>
        public void TestAdvanceHour()
        {
            for (int i = 0; i < 3600; i++)
            {
                UpdateGameSecond();
            }
        }

        //TODO: Remove
        /// <summary>
        /// Advance 1 game day
        /// </summary>
        public void TestAdvanceDay()
        {
            for (int i = 0; i < 86400; i++)
            {
                UpdateGameSecond();
            }
        }




    }
}
