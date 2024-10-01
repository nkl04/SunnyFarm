using System.Collections;
using System.Collections.Generic;
using SunnyFarm.Game.Entities.Player;
using UnityEngine;

public abstract class ToolHit : MonoBehaviour
{
    public virtual void Hit(Player player)
    {
        Debug.Log("Hit");
    }
}
