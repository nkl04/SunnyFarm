using SunnyFarm.Game.Entities.Item;
using SunnyFarm.Game.Entities.Player;
using System.Collections.Generic;
using UnityEngine;


public class WateringCanBehaviour : IToolBehaviour
{
    private float holdTime;
    private ToolDetail toolDetail;
    private Player player;

    private int charges;
    public WateringCanBehaviour(ToolDetail _toolDetail, Player _player)
    {
        toolDetail = _toolDetail;
        player = _player;

        charges = toolDetail.ChargeCapacity;
    }
    public void OnHold(ref bool isUsing)
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

    public void OnPress()
    {
        holdTime = 0f;  // Reset hold time when pressed
    }

    public void OnRelease()
    {
        // for the upgraded
        if (toolDetail.CanPowerUp)
        {
            holdTime = 0f;
            player.IsWaterPressed = true;
        }
    }

    public void Use(List<GridPropertiesDetail> tileDetails)
    {
        ConsumeCharge(toolDetail.ChargeConsume);

        GridPropertiesController.Instance.SetWaterGround(tileDetails);
    }

    public void Reactivate()
    {
        player.IsWaterPressed = false;
    }

    private void ConsumeCharge(int charge)
    {
        charges -= charge;
    }
}
