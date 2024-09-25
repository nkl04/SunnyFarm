
using SunnyFarm.Game;
using SunnyFarm.Game.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SunnyFarm.Game.Constant.Enums;

public class GameClock : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private Image seasonImage;
    [SerializeField] private Image weatherImage;
    private void OnEnable()
    {
        EventHandler.OnAdvanceGameMinute += UpdateGameTime;
    }

    private void OnDisable()
    {
        EventHandler.OnAdvanceGameMinute -= UpdateGameTime;
    }

    private void UpdateGameTime(int year, Season season, int day, WeekDay dayofWeek, int hour, int minute, int second)
    {
        minute -= (minute % 10);

        string am_pm = "";

        if (hour >= 12)
        {
            am_pm = "pm";
        }
        else am_pm = "am";

        if (hour > 12)
        {
            hour -= 12;
        }

        string minuteString = (minute - minute % 10) < 10 ? "00" : (minute - minute % 10).ToString();

        string time = hour.ToString() + ":" + minuteString + " " + am_pm;

        timeText?.SetText(time);
        dayText?.SetText(dayofWeek.ToString() + ". " + day.ToString());
        seasonImage.sprite = GameUIManager.Instance.GetSeasonUISprite(season);
        weatherImage.sprite = GameUIManager.Instance.GetWeatherUISprite(Weather.Sunny);
    }
}
