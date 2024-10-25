namespace SunnyFarm.Game.Managers
{
    using SunnyFarm.Game.Entities.Item;
    using System.Collections.Generic;
    using UnityEngine;
    using static SunnyFarm.Game.Constant.Enums;

    public abstract class ToolController : ItemController
    {
        protected ToolDetail toolDetail;

        [SerializeField] private List<ResourceType> resourceCanHit = new List<ResourceType>();

        protected GridPropertiesDetail TileActionCheck()
        {
            Vector3Int cursorGridPosition = gridCursor.GetGridPositionForCursor();
            Vector3Int playerGridPosition = gridCursor.GetGridPositionForPlayer();

            if (Mathf.Abs(cursorGridPosition.x - playerGridPosition.x) > toolDetail.OffsetDistance
                || Mathf.Abs(cursorGridPosition.y - playerGridPosition.y) > toolDetail.OffsetDistance)
            {
                var playerDirection = player.GetPlayerDirection();
                var position = playerGridPosition + new Vector3Int(playerDirection.x, playerDirection.y, 0);
                return GridPropertiesController.Instance.GetGridPropertyDetail(position.x, position.y);
            }
            else
            {
                return GridPropertiesController.Instance.GetGridPropertyDetail(cursorGridPosition.x, cursorGridPosition.y);
            }
        }

        protected virtual void HitBox(Vector2 position)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(position, toolDetail.InteractableAreaSize);

            foreach (Collider2D collider in colliders)
            {
                // if object is not damagable
                IToolHittable toolHit = collider.GetComponentInParent<IToolHittable>();

                if (toolHit != null)
                {
                    if (toolHit.CanBeHit(resourceCanHit))
                    {
                        toolHit.Hit(player);
                    }
                }
                // if object is damagable

            }
        }


    }
}
