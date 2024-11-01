using SunnyFarm.Game.Entities.Item;
using SunnyFarm.Game.Managers;
using UnityEngine;

public class PickaxeController : ToolController
{
    protected override void Update()
    {
        if (Input.GetMouseButton(0) & !isUseTool)
        {
            isUseTool = true;
        }
    }
    public override void ReactivateTool()
    {
        base.ReactivateTool();
    }

    public override void EnableController()
    {
        base.EnableController();

        toolDetail = itemDetail as ToolDetail;
    }
    public override void UseItem()
    {
        // Get grid property detail that make action
        GridPropertiesDetail detail = TileActionCheck();

        if (!detail.HasCrop)
        {
            GridPropertiesController.Instance.SetLandGround(detail);
        }

        HitBox(detail.Position, out var havingObj);
    }
}
