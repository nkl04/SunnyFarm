using SunnyFarm.Game.Entities.Item;
using SunnyFarm.Game.Managers;
using SunnyFarm.Game.Tilemap;
using System.Collections.Generic;
using UnityEngine;

public class ToolsManager : MonoBehaviour
{
    [SerializeField] private List<ToolController> toolPrefabs; // problems!!!
    [SerializeField] private GridCursor gridCursor;

    private List<ToolController> toolControllers = new List<ToolController>();
    private ToolController toolUsing;

    // test
    [SerializeField] protected ToolDetail toolDetail;
    private void Start()
    {
        foreach (var controller in toolPrefabs)
        {
            ToolController tool = Instantiate(controller, transform);
            toolControllers.Add(tool);
        }
        ChangeTool(toolDetail);
    }
    public void ChangeTool(ToolDetail tool)
    {
        if (toolUsing != null && toolUsing.ItemType == tool.ItemType) return;
        foreach (var controller in toolControllers)
        {
            if (controller.ItemType == tool.ItemType)
            {
                controller.enabled = true;
                controller.SetUpDetail(tool);
                controller.SetUpCursor(gridCursor);
                toolUsing = controller;
            }
            else
                controller.enabled = false;
        }
    }
    public void ResetTool()
    {
        toolUsing = null;
    }
}
