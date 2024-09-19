namespace SunnyFarm.Game.Tilemap
{
    using UnityEngine;
    using UnityEngine.Tilemaps;
    using Config;
    using static Constant.Enums;
    using System;
    using UnityEditor;

    [ExecuteAlways]
    public class TilemapGridProperties : MonoBehaviour
    {
        [SerializeField] private ConfigGridProperties configGridProperties = null;
        [SerializeField] private GridBoolProperty gridBoolProperty = GridBoolProperty.Diggable;
        private Tilemap tilemap;

        private void OnEnable()
        {
            // only populate in editor mode
            if (!Application.IsPlaying(gameObject))
            {
                tilemap = GetComponent<Tilemap>();

                if (configGridProperties != null)
                {
                    configGridProperties.gridPropertyList.Clear();
                }
            }
        }

        private void OnDisable()
        {
            // only populate in editor mode    
            if (!Application.IsPlaying(gameObject))
            {
                UpdateGridProperties();
                if (configGridProperties != null)
                {
                    //ensure that updated gridproperties are saved when exiting play mode
                    EditorUtility.SetDirty(configGridProperties);
                }
            }

        }

        private void UpdateGridProperties()
        {
            // compress tilemap bounds 
            tilemap.CompressBounds();

            // only populate in editor mode
            if (!Application.IsPlaying(gameObject))
            {
                if (configGridProperties != null)
                {
                    Vector3Int startCell = tilemap.cellBounds.min;
                    Vector3Int endCell = tilemap.cellBounds.max;

                    for (int x = startCell.x; x <= endCell.x; x++)
                    {
                        for (int y = startCell.y; y <= endCell.y; y++)
                        {
                            TileBase tileBase = tilemap.GetTile(new Vector3Int(x, y, 0));
                            if (tileBase != null)
                            {
                                GridCoordinate gridCoordinate = new GridCoordinate(x, y);
                                GridProperty gridProperty = new GridProperty(gridCoordinate, gridBoolProperty, true);
                                configGridProperties.gridPropertyList.Add(gridProperty);
                                Debug.Log("Add grid property: " + gridProperty);
                            }
                        }
                    }
                }
            }
        }

        private void Update()
        {
            // only populate in editor mode
            if (!Application.IsPlaying(gameObject))
            {

                Debug.Log("Disable property tilemaps");
            }
        }
    }
}

