using SunnyFarm.Game.Entities.Item.Data;
using SunnyFarm.Game.Entities.Player;
using SunnyFarm.Game.Tilemap;
using UnityEngine;
using static SunnyFarm.Game.Constant.Enums;

public abstract class ItemController : MonoBehaviour
{
    protected Player player;
    protected Rigidbody2D rb2d;
    protected ItemDetail itemDetail;
    protected GridCursor gridCursor;

    protected bool isUseTool = false;

    public ItemType ItemType;

    protected virtual void Start()
    {
        player = GetComponentInParent<Player>();
        rb2d = GetComponentInParent<Rigidbody2D>();
    }

    protected abstract void Update();

    public virtual void ReactivateTool()
    {
        isUseTool = false;
    }

    public void SetUpCursor(GridCursor _gridCursor)
    {
        gridCursor = _gridCursor;
    }

    public void SetUpDetail(ItemDetail _itemDetail)
    {
        itemDetail = _itemDetail;
    }

    public virtual void EnableController()
    {
        this.enabled = true;
    }

    public void DisableController()
    {
        this.enabled = false;
    }

    public abstract void UseItem();
}
