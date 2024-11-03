namespace SunnyFarm.Game.Entities.Item.Data
{
    using System.Collections.Generic;
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    [CreateAssetMenu(fileName = "ConfigItemList", menuName = "Configs/Items/ConfigItem List")]
    public class ConfigItemList : ScriptableObject
    {
        public string pathToLoad = "Assets/Game/Configs/Items";
        public ConfigItem[] configItemDetails;

#if UNITY_EDITOR
        private void OnValidate()
        {
            LoadAllItems();
        }

        private void LoadAllItems()
        {
            List<ConfigItem> items = new();

            string[] guids = AssetDatabase.FindAssets("t:ConfigItem", new[] { pathToLoad });

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);

                ConfigItem item = AssetDatabase.LoadAssetAtPath<ConfigItem>(assetPath);

                if (item != null)
                {
                    items.Add(item);
                }
            }

            configItemDetails = null;

            configItemDetails = items.ToArray();
        }
#endif
    }
}

