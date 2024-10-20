namespace SunnyFarm.Game.Entities.Item
{
    using SunnyFarm.Game.Managers;
    using UnityEngine;

    public class HoeController : ToolController
    {
        protected override void Update()
        {
            if (Input.GetMouseButton(0) & !isUseTool)
            {
                player.IsDigPressed = true; // test;
                isUseTool = true;
            }
        }
        public override void ReactivateTool()
        {
            base.ReactivateTool();
            player.IsDigPressed = false; // test;
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
            GridPropertiesController.Instance.DisplayDugGround(detail);

            HitBox(detail.Position);
        }
    }
}