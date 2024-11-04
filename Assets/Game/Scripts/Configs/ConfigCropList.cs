namespace SunnyFarm.Game.Entities.Crops.Data
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    using System.IO;

    [CreateAssetMenu(fileName = "New ConfigCropList", menuName = "Configs/Crop/ConfigCrop List")]
    public class ConfigCropList : ScriptableObject
    {
        public string pathToLoad = "Assets/Game/Configs/Crops";
        public ConfigCrop[] configCropDetails;


#if UNITY_EDITOR
        private void OnValidate()
        {
            LoadAllCrops();
        }

        private void LoadAllCrops()
        {
            List<ConfigCrop> crops = new();

            string[] files = Directory.GetFiles(pathToLoad, "*.asset");

            foreach (string file in files)
            {
                var crop = AssetDatabase.LoadAssetAtPath<ConfigCrop>(file);
                if (crop != null)
                {
                    crops.Add(crop);
                }
            }

            configCropDetails = null;

            configCropDetails = crops.ToArray();
        }
#endif
    }
}

