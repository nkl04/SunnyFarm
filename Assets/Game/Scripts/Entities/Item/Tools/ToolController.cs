namespace SunnyFarm.Game.Managers
{
    using SunnyFarm.Game.Entities.Item;
    using SunnyFarm.Game.Entities.Player;
    using SunnyFarm.Game.Tilemap;
    using UnityEngine;
    using static SunnyFarm.Game.Constant.Enums;

    public abstract class ToolController : MonoBehaviour
    {
        protected Player player;
        protected Rigidbody2D rb2d;
        protected ToolDetail toolDetail;
        protected GridCursor gridCursor;
        //TODO: Remove
        protected bool isUseTool = false;

        public ToolType ToolType;
        protected virtual void Start()
        {
            player = GetComponentInParent<Player>();
            rb2d = GetComponentInParent<Rigidbody2D>();
        }

        protected virtual void Update()
        {
            if (Input.GetMouseButton(1) & !isUseTool)
            {
                UseTool();
                isUseTool = true;
            }
            else if (Input.GetMouseButton(0) & isUseTool)
            {
                isUseTool = false;
            }
        }

        protected abstract void UseTool();

        public void SetUpCursor(GridCursor _gridCursor)
        {
            gridCursor = _gridCursor;
        }
        public void SetUpDetail(ToolDetail _toolDetail)
        {
            toolDetail = _toolDetail;
        }
    }
}
