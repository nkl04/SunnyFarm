namespace SunnyFarm.Game.Managers
{
    using SunnyFarm.Game;
    using SunnyFarm.Game.DesignPattern;
    using UnityEngine;
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
            EventHandler.CallAdvanceGameMinute(year, season, day, weekDay, hour, minute, second);
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

            EventHandler.CallAdvanceGameMinute(year, season, day, weekDay, hour, minute, second);

            Debug.Log("Game Year: " + year + " - Season: " + season + " - Day: " + day + " - Weekday: " + weekDay + " - Hour: " + hour + " - Minute: " + minute);
        }

        private void UpdateGameHour()
        {
            hour++;

            if (hour > 23)
            {
                hour = 0;
                UpdateGameDay();
            }

            EventHandler.CallAdvanceGameHour(year, season, day, weekDay, hour, minute, second);
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

            EventHandler.CallAdvanceGameDay(year, season, day, weekDay, hour, minute, second);
        }

        private void UpdateGameSeason()
        {
            season++;

            if (season > Season.Winter)
            {
                season = Season.Spring;
                UpdateGameYear();
            }

            EventHandler.CallAdvanceGameSeason(year, season, day, weekDay, hour, minute, second);
        }

        private void UpdateGameYear()
        {
            year++;
            EventHandler.CallAdvanceGameYear(year, season, day, weekDay, hour, minute, second);
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
