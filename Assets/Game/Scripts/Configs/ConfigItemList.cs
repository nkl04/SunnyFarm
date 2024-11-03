namespace SunnyFarm.Game.Configs
{
    using SunnyFarm.Game.Entities.Item.Data;
    using UnityEngine;

    [CreateAssetMenu(fileName = "ConfigItemList", menuName = "Configs/Items/ConfigItem List")]
    public class ConfigItemList : ScriptableObject
    {
        [SerializeField] public ConfigItem[] itemDetails;
    }
}

