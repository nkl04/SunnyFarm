namespace SunnyFarm.Game.Managers
{
    using SunnyFarm.Game.Entities.Item;
    using UnityEngine;

    public abstract class ToolController : ItemController
    {
        protected ToolDetail toolDetail;

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
                    toolHit.Hit(player);
                    break;
                }

                // if object is damagable

            }
        }


    }
}
