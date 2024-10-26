using System;
using UnityEngine;
using static SunnyFarm.Game.Constant.Enums;

[Serializable]
public class GridPropertiesDetail
{
    public Vector2Int Position { get; set; }
    public TileType TileType { get; set; }
    public int DaysSinceLastModified { get; set; } = 0;
    public CropDetail Crop { get; private set; }
    public bool HasCrop => Crop != null;

    public void SetCropDetail(CropDetail crop)
    {
        Crop = crop;
    }
}
