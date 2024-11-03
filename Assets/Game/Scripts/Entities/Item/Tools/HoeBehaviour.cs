using SunnyFarm.Game.Entities.Item.Data;
using SunnyFarm.Game.Entities.Player;
using System.Collections.Generic;
using UnityEngine;


public class HoeBehaviour : ToolBehaviour
{
    private float holdTime;

    public HoeBehaviour(ConfigItemTool _toolDetail, Player _player) : base(_toolDetail, _player)
    {
    }

    public override void OnHold(ref bool isUsing)
    {
        // for basic
        if (!toolDetail.CanPowerUp)
        {
            if (!isUsing)
            {
                isUsing = true;
                player.IsDigPressed = true;
            }
        }

        // for the upgraded
        else
        {
            holdTime += Time.deltaTime;
            if (holdTime < 1f)
            {
                Debug.Log("Upgraded Hoe - Tilling 1 tiles ahead");
            }
            else if (holdTime >= 1f && holdTime < 2f)
            {
                Debug.Log("Upgraded Hoe - Tilling 3 tiles ahead");
            }
            else if (holdTime >= 2f && holdTime < 3f)
            {
                Debug.Log("Upgraded Hoe - Tilling 5 tiles ahead");
            }
            else if (holdTime >= 3f && holdTime < 4f)
            {
                Debug.Log("Upgraded Hoe - Tilling a 3x3 area");
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
            player.IsDigPressed = true;
        }
    }

    public override void Use(List<GridPropertiesDetail> tileDetails)
    {
        GridPropertiesController.Instance.SetDugGround(tileDetails);
    }

    public override void Reactivate()
    {
        player.IsDigPressed = false;
    }
}
