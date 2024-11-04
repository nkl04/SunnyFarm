using SunnyFarm.Game;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
public class DropToWorldArea : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        EventHandlers.CallOnLeftPointerClick(this);
    }

}
