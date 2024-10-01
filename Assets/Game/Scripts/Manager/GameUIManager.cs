namespace SunnyFarm.Game.Managers
{
    using SunnyFarm.Game.Configs;
    using SunnyFarm.Game.DesignPattern;
    using UnityEngine;
    using static SunnyFarm.Game.Constant.Enums;

    public class GameUIManager : Singleton<GameUIManager>
    {
        [Header("Game Clock")]
        [SerializeField] private ConfigWeatherImage configWeatherImage;

        public Sprite GetSeasonUISprite(Season season)
        {
            switch (season)
            {
                case Season.Spring:
                    return configWeatherImage.springSprite;
                case Season.Summer:
                    return configWeatherImage.summerSprite;
                case Season.Fall:
                    return configWeatherImage.fallSprite;
                case Season.Winter:
                    return configWeatherImage.winterSprite;
                default:
                    Debug.LogError("Season " + season + " sprite not found!");
                    return null;
            }
        }

        public Sprite GetWeatherUISprite(Weather weather)
        {
            switch (weather)
            {
                case Weather.Sunny:
                    return configWeatherImage.sunnySprite;
                case Weather.Rainy:
                    return configWeatherImage.rainySprite;
                case Weather.Snowny:
                    return configWeatherImage.snownySprite;
                default:
                    Debug.LogError("Weather " + weather + " sprite not found!");
                    return null;
            }
        }
    }
}
