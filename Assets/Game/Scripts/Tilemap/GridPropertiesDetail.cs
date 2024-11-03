using System;
using UnityEngine;
using SunnyFarm.Game.Entities.Crops.Data;
using static SunnyFarm.Game.Constant.Enums;

[Serializable]
public class GridPropertiesDetail
{
    public Vector2Int Position { get; set; }
    public TileType TileType { get; set; }
    public int DaysSinceLastModified { get; set; } = 0;
    public ConfigCrop Crop { get; private set; }
    public bool HasCrop => Crop != null;

    public void SetCropDetail(ConfigCrop crop)
    {
        Crop = crop;
    }
}
