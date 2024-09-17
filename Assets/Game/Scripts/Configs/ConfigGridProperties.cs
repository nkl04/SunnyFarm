namespace SunnyFarm.Game.Config
{
    using System.Collections.Generic;
    using UnityEngine;
    using SunnyFarm.Game.Tilemap;
    using static SunnyFarm.Game.Constant.Enums;

    [CreateAssetMenu(fileName = "ConfigGridProperties", menuName = "Configs/Grid/GridProperties")]
    public class ConfigGridProperties : ScriptableObject
    {
        public SceneName sceneName;
        public int gridWidth;
        public int gridHeight;
        public int originX;
        public int originY;
        public List<GridProperty> gridPropertyList;
    }
}
