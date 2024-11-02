using SunnyFarm.Game;
using SunnyFarm.Game.Config;
using SunnyFarm.Game.DesignPattern;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static SunnyFarm.Game.Constant.Enums;

[RequireComponent(typeof(GenerateGUID))]
public class GridPropertiesController : Singleton<GridPropertiesController>, ISavable
{
    public string ISavableUniqueID { get => iSavableUniqueID; set => iSavableUniqueID = value; }
    public GameObjectSave GameObjectSave { get => gameObjectSave; set => gameObjectSave = value; }

    [HideInInspector] public Tilemap groundDecoration1;

    [HideInInspector] public Tilemap groundDecoration2;

    [SerializeField] private ConfigGridProperties[] configGrids;

    private Grid grid;

    private Dictionary<string, GridPropertiesDetail> gridPropertiesDetails;

    private string iSavableUniqueID;

    private GameObjectSave gameObjectSave;

    [SerializeField] private RuleTile dugTile;

    [SerializeField] private RuleTile landTile;

    [SerializeField] private RuleTile wateredTile;

    protected override void Awake()
    {
        base.Awake();

        ISavableUniqueID = GetComponent<GenerateGUID>().GUID;

        GameObjectSave = new GameObjectSave();
    }

    private void OnEnable()
    {
        ISavableRegister();

        EventHandlers.OnAfterSceneLoad += AfterSceneLoad;

        EventHandlers.OnAdvanceGameDay += UpdateTileDetailEachDay;
    }

    private void OnDisable()
    {
        ISavableUnregister();

        EventHandlers.OnAfterSceneLoad -= AfterSceneLoad;

        EventHandlers.OnAdvanceGameDay -= UpdateTileDetailEachDay;
    }
    void Start()
    {
        Initialize();
    }

    private void ClearDisplayGroundDecoration()
    {
        // remove ground decorations
        groundDecoration1.ClearAllTiles();

        groundDecoration2.ClearAllTiles();
    }

    private void ClearDisplayGridPropertiesDetails()
    {
        ClearDisplayGroundDecoration();
    }

    void Initialize()
    {
        foreach (var config in configGrids)
        {
            Dictionary<string, GridPropertiesDetail> gridPropertiesDictionary = new Dictionary<string, GridPropertiesDetail>();

            foreach (var gridProperty in config.gridPropertyList)
            {
                GridPropertiesDetail gridPropertiesDetail = new GridPropertiesDetail();

                switch (gridProperty.gridProperty)
                {
                    case GridBoolProperty.Diggable:
                        gridPropertiesDetail.TileType = TileType.Land;
                        break;
                }
                SetGridPropertyDetail(gridProperty.coordinate.x, gridProperty.coordinate.y, gridPropertiesDetail, gridPropertiesDictionary);
            }

            // save the grid property dictionary to the scene save system
            SceneSave sceneSave = new SceneSave();

            // add grid properties dictionary to the scene save data
            sceneSave.gridPropertiesDetailDictionary = gridPropertiesDictionary;

            // If starting scene is set
            if (config.sceneName.ToString() == SceneController.Instance.sceneName.ToString())
            {
                this.gridPropertiesDetails = gridPropertiesDictionary;
            }

            GameObjectSave.sceneData.Add(config.sceneName.ToString(), sceneSave);
        }
    }
    private void DisplayGridPropertyDetails()
    {
        foreach (var detail in gridPropertiesDetails)
        {
            GridPropertiesDetail gridPropertiesDetail = detail.Value;

            if (gridPropertiesDetail.TileType == TileType.Dug)
                DisplayTileGround(groundDecoration1, gridPropertiesDetail, dugTile);
        }
    }

    public void SetDugGround(List<GridPropertiesDetail> gridPropertiesDetails)
    {
        if (gridPropertiesDetails.Count == 0) return;

        foreach (GridPropertiesDetail gridPropertiesDetail in gridPropertiesDetails)
        {
            if (gridPropertiesDetail.TileType == TileType.Land)
            {
                UpdateTileType(gridPropertiesDetail, TileType.Dug);

            DisplayTileGround(groundDecoration1, gridPropertiesDetail, dugTile);


            }

        }
    }

    public void SetWaterGround(List<GridPropertiesDetail> gridPropertiesDetails)
    {
        if (gridPropertiesDetails.Count == 0) return;

        foreach (GridPropertiesDetail gridPropertiesDetail in gridPropertiesDetails)
        {
            if (gridPropertiesDetail.TileType == TileType.Dug)
            {
                UpdateTileType(gridPropertiesDetail, TileType.Watered);


            DisplayTileGround(groundDecoration2, gridPropertiesDetail, wateredTile);
            }

        }
    }
    public void SetLandGround(GridPropertiesDetail gridPropertiesDetail)
    {
        if (gridPropertiesDetail == null) return;

        if (gridPropertiesDetail.TileType > TileType.Land)
        {
            UpdateTileType(gridPropertiesDetail, TileType.Land);

            DisplayTileGround(groundDecoration1, gridPropertiesDetail, landTile);
        }
    }

    public void DisplayTileGround(Tilemap tilemap, GridPropertiesDetail gridPropertiesDetail, RuleTile ruleTile)
    {
        tilemap.SetTile(new Vector3Int(gridPropertiesDetail.Position.x, gridPropertiesDetail.Position.y, 0), ruleTile);
    }

    private void UpdateTileType(GridPropertiesDetail gridPropertiesDetail, TileType type)
    {
        gridPropertiesDetail.TileType = type;

        gridPropertiesDetail.DaysSinceLastModified = 0;
    }
    private void AfterSceneLoad()
    {
        grid = GameObject.FindObjectOfType<Grid>();

        groundDecoration1 = GameObject.FindGameObjectWithTag("GroundDecoration1").GetComponent<Tilemap>();

        groundDecoration2 = GameObject.FindGameObjectWithTag("GroundDecoration2").GetComponent<Tilemap>();
    }
    private void SetGridPropertyDetail(int x, int y, GridPropertiesDetail gridPropertiesDetail, Dictionary<string,
        GridPropertiesDetail> gridPropertyDictionary)
    {
        // convert key from coordinate
        string key = "x" + x + "x" + y;

        gridPropertiesDetail.Position = new Vector2Int(x, y);

        gridPropertyDictionary[key] = gridPropertiesDetail;
    }
    public GridPropertiesDetail GetGridPropertyDetail(int x, int y)
    {
        // convert key from coordinate
        string key = "x" + x + "x" + y;

        GridPropertiesDetail gridPropertiesDetail;

        if (!gridPropertiesDetails.TryGetValue(key, out gridPropertiesDetail))
        {
            return null;
        }
        else
        {
            return gridPropertiesDetail;
        }
    }

    public void ISavableRegister()
    {
        SaveLoadManager.Instance.ISavableObjectList.Add(this);
    }

    public void ISavableUnregister()
    {
        SaveLoadManager.Instance.ISavableObjectList.Remove(this);
    }

    public void StoreScene(string sceneName)
    {
        // Remove sceneSave for scene
        GameObjectSave.sceneData.Remove(sceneName);

        // Create sceneSave for scene
        SceneSave sceneSave = new SceneSave();

        // Create & add dict grid property details dictionary
        sceneSave.gridPropertiesDetailDictionary = gridPropertiesDetails;

        // Add scene save to game object scene data
        GameObjectSave.sceneData.Add(sceneName, sceneSave);
    }

    public void RestoreScene(string sceneName)
    {
        // get sceneSave for scene = it exists since we created it in initailize
        if (GameObjectSave.sceneData.TryGetValue(sceneName, out SceneSave sceneSave))
        {
            // get grid properties dictionary
            if (sceneSave.gridPropertiesDetailDictionary != null)
            {
                gridPropertiesDetails = sceneSave.gridPropertiesDetailDictionary;
            }

            if (gridPropertiesDetails.Count > 0)
            {
                ClearDisplayGridPropertiesDetails();

                DisplayGridPropertyDetails();
            }
        }
    }

    private void UpdateTileDetailEachDay(int year, WeekDay dayOfWeek, int day, Season season, int hour, int minute, int second)
    {
        // Clear display all grid property details
        ClearDisplayGridPropertiesDetails();


        foreach (var config in configGrids)
        {
            if (GameObjectSave.sceneData.TryGetValue(config.sceneName.ToString(), out SceneSave sceneSave))
            {
                if (sceneSave.gridPropertiesDetailDictionary != null)
                {
                    foreach (var item in gridPropertiesDetails)
                    {
                        if (item.Value.TileType != TileType.Land)
                        {
                            GridPropertiesDetail detail = item.Value;
                            detail.DaysSinceLastModified++;

                            // Logic for Watered Tile: After 1 day, revert to Dug
                            if (detail.TileType == TileType.Watered && detail.DaysSinceLastModified >= 1)
                            {
                                detail.TileType = TileType.Dug;
                                detail.DaysSinceLastModified = 0; // Reset day counter for dug state
                            }

                            // Logic for Dug Tile: After X days, revert to Land
                            if (detail.TileType == TileType.Dug && detail.DaysSinceLastModified >= 3) // Example: 3 days
                            {
                                detail.TileType = TileType.Land;
                                detail.DaysSinceLastModified = 0; // Reset day counter for land state
                            }
                        }
                    }
                }
            }
        }

        // Display grid property details to reflect changes
        DisplayGridPropertyDetails();
    }
}
