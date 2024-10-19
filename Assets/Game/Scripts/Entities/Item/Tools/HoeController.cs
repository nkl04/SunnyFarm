namespace SunnyFarm.Game.Entities.Item
{
    using SunnyFarm.Game.Managers;
    using UnityEngine;

    public class HoeController : ToolController
    {
        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            if (Input.GetMouseButton(0))
            {
                UseTool();
            }
        }
        private void OnEnable()
        {

        }
        private void OnDisable()
        {

        }
        protected override void UseTool()
        {
            // use grid cursor
            GridPropertiesController.Instance.DisplayDugGround(TileActionCheck());

            Vector2 position = rb2d.position + player.LastMovementInput * toolDetail.OffsetDistance;

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
        private GridPropertiesDetail TileActionCheck()
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
    }
}