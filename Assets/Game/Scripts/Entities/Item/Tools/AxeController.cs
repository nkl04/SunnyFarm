using SunnyFarm.Game.Entities.Item;
using SunnyFarm.Game.Managers;
using UnityEngine;

public class AxeController : ToolController
{
    protected override void Update()
    {
        if (Input.GetMouseButton(0) & !isUseTool)
        {
            player.IsAxePressed = true; // test;
            isUseTool = true;
        }
    }
    public override void ReactivateTool()
    {
        base.ReactivateTool();
        player.IsAxePressed = false; // test;
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

        HitBox(detail.Position);
    }
}
