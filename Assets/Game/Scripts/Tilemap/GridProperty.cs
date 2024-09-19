namespace SunnyFarm.Game.Tilemap
{
    using System;
    using static SunnyFarm.Game.Constant.Enums;
    [Serializable]
    public class GridProperty
    {
        public GridCoordinate coordinate;
        public GridBoolProperty gridProperty;
        public bool gridBoolValue = false;

        public GridProperty(GridCoordinate coordinate, GridBoolProperty gridProperty, bool gridBoolValue)
        {
            this.coordinate = coordinate;
            this.gridProperty = gridProperty;
            this.gridBoolValue = gridBoolValue;
        }
    }
}
