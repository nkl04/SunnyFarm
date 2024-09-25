namespace SunnyFarm.Game.Configs
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "ConfigWeatherImage", menuName = "Configs/ImageConfig/ConfigWeatherImage")]
    public class ConfigWeatherImage : ScriptableObject
    {
        [Header("Weather")]
        public Sprite sunnySprite;
        public Sprite rainySprite;
        public Sprite snownySprite;

        [Header("Season")]
        public Sprite springSprite;
        public Sprite summerSprite;
        public Sprite fallSprite;
        public Sprite winterSprite;
    }
}
