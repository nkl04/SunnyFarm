namespace SunnyFarm.Game
{
    using UnityEngine;
    using UnityEngine.Rendering.Universal;
    using UnityEngine.Experimental.Rendering.Universal;

    public class DayNightSystem : MonoBehaviour
    {
        [SerializeField] private Light2D globalLight;
        [SerializeField] private Color dayLightColor = Color.white;
        [SerializeField] private Color nightLightColor;
        [SerializeField] private AnimationCurve nightLightCurve;

        private void Start()
        {
            EventHandler.OnAdvanceGameHour += UpdateLight;
        }

        private void UpdateLight(int year, Constant.Enums.Season season, int day, Constant.Enums.WeekDay dayOfWeek, int hour, int minute, int second)
        {
            float v = nightLightCurve.Evaluate(hour);
            Color color = Color.Lerp(dayLightColor, nightLightColor, v);
            globalLight.color = color;
        }
    }
}
