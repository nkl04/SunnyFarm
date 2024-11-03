using SunnyFarm.Game.Entities.Item.Data;
using SunnyFarm.Game.Entities.Player;
using System.Collections.Generic;
using UnityEngine;


public class WateringCanBehaviour : ToolBehaviour
{
    private float holdTime;

    private int charges;

    public WateringCanBehaviour(ConfigItemTool _toolDetail, Player _player) : base(_toolDetail, _player)
    {
        charges = toolDetail.ChargeCapacity;
    }

    public override void OnHold(ref bool isUsing)
    {
        // for basic
        if (!toolDetail.CanPowerUp)
        {
            if (!isUsing)
            {
                isUsing = true;
                player.IsWaterPressed = true;
            }
        }

        // for the upgraded
        else
        {
            holdTime += Time.deltaTime;
            if (holdTime < 1f)
            {
                Debug.Log("Upgraded Water - Tilling 1 tiles ahead");
            }
            else if (holdTime >= 1f && holdTime < 2f)
            {
                Debug.Log("Upgraded Water - Tilling 3 tiles ahead");
            }
            else if (holdTime >= 2f && holdTime < 3f)
            {
                Debug.Log("Upgraded Water - Tilling 5 tiles ahead");
            }
            else if (holdTime >= 3f && holdTime < 4f)
            {
                Debug.Log("Upgraded Water - Tilling a 3x3 area");
            }
        }
    }

    public override void OnPress()
    {
        holdTime = 0f;  // Reset hold time when pressed
    }

    public override void OnRelease()
    {
        // for the upgraded
        if (toolDetail.CanPowerUp)
        {
            holdTime = 0f;
            player.IsWaterPressed = true;
        }
    }

    public override void Use(List<GridPropertiesDetail> tileDetails)
    {
        ConsumeCharge(toolDetail.ChargeConsume);

        GridPropertiesController.Instance.SetWaterGround(tileDetails);
    }

    public override void Reactivate()
    {
        player.IsWaterPressed = false;
    }

    private void ConsumeCharge(int charge)
    {
        charges -= charge;
    }
}
