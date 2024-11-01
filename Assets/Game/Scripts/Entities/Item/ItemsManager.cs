using SunnyFarm.Game.Entities.Item.Data;
using SunnyFarm.Game.Entities.Player;
using SunnyFarm.Game.Tilemap;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    [SerializeField] private GridCursor gridCursor;

    [SerializeField] private List<ItemController> itemControllers = new List<ItemController>();
    private ItemController itemUsing;

    private Player player;

    // test
    [SerializeField] protected ItemDetail itemDetail;

    [SerializeField] protected ItemDetail itemChange;

    private void Start()
    {
        player = GetComponentInParent<Player>();

        SetupToolAnimationEvents(player.GetComponentInChildren<AnimationEventReceiver>());

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeItem(itemDetail);
        }
    }
    public void ChangeItem(ItemDetail tool)
    {
        if (itemUsing != null && itemUsing.ItemType == tool.ItemType) return;
        foreach (var controller in itemControllers)
        {
            if (controller.ItemType == tool.ItemType)
            {
                controller.enabled = true;
                controller.SetUpDetail(tool);
                controller.SetUpCursor(gridCursor);
                controller.EnableController();
                itemUsing?.DisableController();
                itemUsing = controller;
            }
            else
                controller.enabled = false;
        }
    }
    public void ResetTool()
    {
        itemUsing = null;
    }
    private void SetupToolAnimationEvents(AnimationEventReceiver receiver)
    {
        AnimationEvent onUseEvent = new();
        onUseEvent.EventName = "OnUse";
        onUseEvent.OnAnimationEvent += () => itemUsing?.UseItem();

        receiver.AddAnimationEvent(onUseEvent);

        AnimationEvent onFinishEvent = new();
        onFinishEvent.EventName = "OnFinish";
        onFinishEvent.OnAnimationEvent += () => itemUsing?.ReactivateTool();

        receiver.AddAnimationEvent(onFinishEvent);
    }
}
