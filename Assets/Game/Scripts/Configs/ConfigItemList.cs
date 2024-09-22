namespace SunnyFarm.Game.Configs
{
    using SunnyFarm.Game.Entities.Item.Data;
    using UnityEngine;

    [CreateAssetMenu(fileName = "ConfigItemList", menuName = "Configs/Item/ConfigItemList")]
    public class ConfigItemList : ScriptableObject
    {
        [SerializeField] public ItemDetail[] itemDetails;
    }
}

