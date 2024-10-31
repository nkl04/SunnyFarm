namespace SunnyFarm.Game.Entities.Item
{
    using SunnyFarm.Game.Managers;
    using UnityEngine;

    public class HoeController : ToolController
    {
        protected override void Update()
        {
            if (Input.GetMouseButton(0) && !isUseTool)
            {
                tileDetail = TileActionCheck();

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
            HitBox(tileDetail.Position, out var havingObj);

            if (havingObj) return;
            // Get grid property detail that make action
            GridPropertiesController.Instance.SetDugGround(tileDetail);


        }
    }
}