namespace SunnyFarm.Game.Managers
{
    using SunnyFarm.Game.Entities.Player;
    using UnityEngine;
    public class ToolsController : MonoBehaviour
    {
        [SerializeField] private float offsetDistance = 1f;
        [SerializeField] private float interactableAreaSize = 1f;
        private Player player;
        private Rigidbody2D rb2d;

        private void Start()
        {
            player = GetComponent<Player>();
            rb2d = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (Input.GetMouseButton(1))
            {
                UseTool();
            }
        }

        private void UseTool()
        {
            Vector2 position = rb2d.position + player.LastMovementInput * offsetDistance;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(position, interactableAreaSize);

            foreach (Collider2D collider in colliders)
            {
                ToolHit toolHit = collider.GetComponentInParent<ToolHit>();

                if (toolHit != null)
                {
                    toolHit.Hit(this.player);
                    break;
                }
            }
        }
    }
}
