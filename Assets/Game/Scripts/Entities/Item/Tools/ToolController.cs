namespace SunnyFarm.Game.Managers
{
    using SunnyFarm.Game.Entities.Item;
    using SunnyFarm.Game.Entities.Item.Data;
    using System.Collections.Generic;
    using UnityEngine;

    public class ToolController : ItemController
    {
        protected GridPropertiesDetail tileDetail;

        private ToolBehaviour toolBehaviour;

        private ToolBehaviourMap toolBehaviourMap;

        protected override void Awake()
        {
            base.Awake();

            toolBehaviourMap = new ToolBehaviourMap();
        }
        protected override void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                toolBehaviour.OnPress();
            }
            if (Input.GetMouseButton(0) && !isUseTool)
            {
                tileDetail = TileActionCheck();
                toolBehaviour.OnHold(ref isUseTool);
            }
            if (Input.GetMouseButtonUp(0))
            {
                toolBehaviour.OnRelease();
            }
        }
        public override void SetUpDetail(ConfigItem _itemDetail)
        {
            // before set up data, reactivate tool
            toolBehaviour?.Reactivate();

            base.SetUpDetail(_itemDetail);

            itemDetail = _itemDetail;

            toolBehaviour = toolBehaviourMap.GetToolBehaviour((ConfigItemTool)itemDetail, player);
        }

        public GridPropertiesDetail TileActionCheck()
        {
            Vector3Int cursorGridPosition = gridCursor.GetGridPositionForCursor();

            Vector3Int playerGridPosition = gridCursor.GetGridPositionForPlayer();

            var distance = Vector2.Distance(new Vector2(cursorGridPosition.x, cursorGridPosition.y), new Vector2(playerGridPosition.x, playerGridPosition.y));
            if (distance == ((ConfigItemTool)itemDetail).OffsetDistance || distance == ((ConfigItemTool)itemDetail).OffsetDistance * Mathf.Sqrt(2))
            {
                // change the last movement based on cursor position
                player.LastMovementInput = new Vector2(cursorGridPosition.x - playerGridPosition.x, cursorGridPosition.y - playerGridPosition.y);

                // flip the player 
                if (this.player.LastMovementInput.x > 0 && !this.player.IsFacingRight)
                {
                    this.player.Flip();

                    this.player.IsFacingRight = true;
                }
                else if (this.player.LastMovementInput.x < 0 && this.player.IsFacingRight)
                {
                    this.player.Flip();

                    this.player.IsFacingRight = false;
                }

                return GridPropertiesController.Instance.GetGridPropertyDetail(cursorGridPosition.x, cursorGridPosition.y);
            }
            else
            {
                var playerDirection = player.GetPlayerDirection();

                var position = playerGridPosition + new Vector3Int(playerDirection.x, playerDirection.y, 0);

                return GridPropertiesController.Instance.GetGridPropertyDetail(position.x, position.y);
            }
        }

        protected virtual void HitBox(Vector2 position, out bool havingObj)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(position, ((ConfigItemTool)itemDetail).InteractableAreaSize);

            havingObj = false;

            foreach (Collider2D collider in colliders)
            {
                // if object is not damagable
                IToolHittable toolHit = collider.GetComponentInParent<IToolHittable>();

                if (toolHit != null)
                {
                    havingObj = true;

                    if (toolHit.CanBeHit(((ConfigItemTool)itemDetail).ResourceCanBeHit))
                    {
                        toolHit.Hit(player);
                    }
                }
                // if object is damagable

            }
        }

        public override void UseItem()
        {
            HitBox(tileDetail.Position, out var havingObj);

            if (havingObj) return;

            toolBehaviour.Use(new List<GridPropertiesDetail>() { tileDetail }); // need to modify
        }

        public override void ReactivateTool()
        {
            toolBehaviour.Reactivate();

            base.ReactivateTool();
        }
    }
}
