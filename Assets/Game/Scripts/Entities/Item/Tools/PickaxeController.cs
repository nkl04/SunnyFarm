using SunnyFarm.Game.Entities.Item;
using SunnyFarm.Game.Managers;
using UnityEngine;

public class PickaxeController : ToolController
{
    protected override void Update()
    {
        if (Input.GetMouseButton(0) & !isUseTool)
        {
            player.IsPickaxePressed = true; // test;
            isUseTool = true;
        }
    }
    public override void ReactivateTool()
    {
        base.ReactivateTool();
        player.IsPickaxePressed = false; // test;
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
            GridPropertiesController.Instance.DisplayLandGround(detail);
        }

        HitBox(detail.Position);
    }
}
