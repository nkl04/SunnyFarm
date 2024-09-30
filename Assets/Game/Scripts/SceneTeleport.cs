using System.Collections;
using System.Collections.Generic;
using SunnyFarm.Game;
using SunnyFarm.Game.Entities.Player;
using UnityEngine;
using static SunnyFarm.Game.Constant.Enums;

[RequireComponent(typeof(Collider2D))]
public class SceneTeleport : MonoBehaviour
{
    [SerializeField] private SceneName sceneGoTo = SceneName.Scene1_Farm;

    [SerializeField] private Vector3 sceneGoToPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponentInParent<Player>();
        if (player != null)
        {
            float xPos = Mathf.Approximately(sceneGoToPosition.x, 0) ? player.transform.position.x : sceneGoToPosition.x;

            float yPos = Mathf.Approximately(sceneGoToPosition.y, 0) ? player.transform.position.y : sceneGoToPosition.y;

            float zPos = 0f;

            SceneController.Instance.FadeAndLoadScene(sceneGoTo, new Vector3(xPos, yPos, zPos));
        }
    }
}
