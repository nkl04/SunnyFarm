using SunnyFarm.Game.Entities.Item;
using SunnyFarm.Game.Managers;
using UnityEngine;

public class WateringController : ToolController
{
    private int charges;
    protected override void Start()
    {
        base.Start();

        charges = toolDetail.chargeCapacity;
    }
    protected override void Update()
    {
        if (Input.GetMouseButton(0) & !isUseTool)
        {
            player.IsWaterPressed = true; // test;
            isUseTool = true;
        }
    }
    public override void ReactivateTool()
    {
        base.ReactivateTool();
        player.IsWaterPressed = false; // test;
    }

    public override void EnableController()
    {
        base.EnableController();

        toolDetail = itemDetail as ToolDetail;
    }
    public override void UseItem()
    {
        ConsumeCharge(toolDetail.chargeConsume);

        // Get grid property detail that make action
        GridPropertiesDetail detail = TileActionCheck();
        GridPropertiesController.Instance.SetWaterGround(detail);
    }

    private void ConsumeCharge(int charge)
    {
        charges -= charge;
    }
}
