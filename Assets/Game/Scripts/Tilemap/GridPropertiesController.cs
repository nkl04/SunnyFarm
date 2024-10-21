using SunnyFarm.Game;
using SunnyFarm.Game.Config;
using SunnyFarm.Game.DesignPattern;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static SunnyFarm.Game.Constant.Enums;

public class GridPropertiesController : Singleton<GridPropertiesController>
{
    [SerializeField] public Tilemap groundDecoration1;
    public Tilemap groundDecoration2;
    private Grid grid;
    private Dictionary<string, GridPropertiesDetail> gridPropertiesDetails;
    [SerializeField] private ConfigGridProperties[] configGrids;

    [SerializeField] private RuleTile dugTile;
    [SerializeField] private RuleTile landTile;
    [SerializeField] private RuleTile wateredTile;


    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        EventHandler.OnAfterSceneLoad += AfterSceneLoad;
    }

    private void OnDisable()
    {
        EventHandler.OnAfterSceneLoad -= AfterSceneLoad;
    }
    void Start()
    {
        Initialize();
        groundDecoration1 = GameObject.FindGameObjectWithTag("GroundDecoration1").GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {

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


            // If starting scene is set
            if (config.sceneName.ToString() == "")
            {
            }

            // test
            this.gridPropertiesDetails = gridPropertiesDictionary;

        }
    }
    public void DisplayDugGround(GridPropertiesDetail gridPropertiesDetail)
    {
        if (gridPropertiesDetail.TileType == TileType.Land)
        {
            SetDugGround(gridPropertiesDetail);
        }
    }

    private void SetDugGround(GridPropertiesDetail gridPropertiesDetail)
    {
        if (IsGroundDug(gridPropertiesDetail.Position.x, gridPropertiesDetail.Position.y)) return;

        UpdateTileType(gridPropertiesDetail, TileType.Dug);
        groundDecoration1.SetTile(new Vector3Int(gridPropertiesDetail.Position.x, gridPropertiesDetail.Position.y, 0), dugTile);
    }
    public void DisplayLandGround(GridPropertiesDetail gridPropertiesDetail)
    {
        if (gridPropertiesDetail.TileType > TileType.Land)
        {
            SetLandGround(gridPropertiesDetail);
        }
    }

    private void SetLandGround(GridPropertiesDetail gridPropertiesDetail)
    {
        if (IsGroundLand(gridPropertiesDetail.Position.x, gridPropertiesDetail.Position.y)) return;

        UpdateTileType(gridPropertiesDetail, TileType.Land);
        groundDecoration1.SetTile(new Vector3Int(gridPropertiesDetail.Position.x, gridPropertiesDetail.Position.y, 0), landTile);
    }
    private void UpdateTileType(GridPropertiesDetail gridPropertiesDetail, TileType type)
    {
        gridPropertiesDetail.TileType = type;
        gridPropertiesDetail.DaysSinceLastModified = 0;
    }
    private bool IsGroundLand(int x, int y)
    {
        GridPropertiesDetail gridPropertiesDetail = GetGridPropertyDetail(x, y);

        if (gridPropertiesDetail == null)
        {
            return false;
        }
        else if (gridPropertiesDetail.TileType == TileType.Land)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool IsGroundDug(int x, int y)
    {
        GridPropertiesDetail gridPropertiesDetail = GetGridPropertyDetail(x, y);

        if (gridPropertiesDetail == null)
        {
            return false;
        }
        else if (gridPropertiesDetail.TileType > TileType.Land)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void AfterSceneLoad()
    {
        grid = GameObject.FindObjectOfType<Grid>();
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
}
