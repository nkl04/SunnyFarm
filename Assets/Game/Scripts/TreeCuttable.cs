using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SunnyFarm.Game.Entities.Player;
public class TreeCuttable : ToolHit
{
    [Header("Tree Parts")]
    [SerializeField] private GameObject bodyTree;
    [SerializeField] private GameObject stoolTree;

    [Space(5)]

    [Header("Tree Drops")]
    [SerializeField] private GameObject dropPrefab;
    [SerializeField] private int bodyDropAmount = 5;
    [SerializeField] private float stoolDropAmount = 5f;
    [SerializeField] private float spread = 2f;

    private float fallDegreeZ = 60f;
    private float fallDuration = 1f;

    //TODO: Remove
    public int hp = 10;

    public override void Hit(Player player)
    {
        if (hp <= 0) return;

        hp--;

        if (hp == 5)
        {
            BodyFallAnim(player.LastMovementInput);
        }

        if (bodyTree != null)
        {
            ShakeAnim(bodyTree);
        }
        else
        {
            ShakeAnim(stoolTree);
        }

        if (hp <= 0)
        {
            while (bodyDropAmount > 0)
            {
                bodyDropAmount--;

                Vector3 pos = transform.position;
                pos.x += spread * Random.value - spread / 2;
                pos.y += spread * Random.value - spread / 2;

                Instantiate(dropPrefab, pos, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }

    public void ShakeAnim(GameObject gameObject)
    {
        gameObject.transform.DOShakePosition(0.5f, 0.5f, 10, 90, false, true);
    }

    public void BodyFallAnim(Vector2 directionImpact)
    {
        float fallDirectionZ = directionImpact.x != 0 ? -directionImpact.x * fallDegreeZ : Random.Range(-1f, 1f) * fallDegreeZ;
        //body tree fall
        bodyTree.transform.DORotate(new Vector3(0f, 0f, fallDirectionZ), fallDuration).OnComplete(() =>
        {
            //drop log
            while (bodyDropAmount > 0)
            {
                bodyDropAmount--;

                Vector3 pos = bodyTree.transform.position;
                pos.x += spread * Random.value - spread / 2;
                pos.y += spread * Random.value - spread / 2;

                Instantiate(dropPrefab, pos, Quaternion.identity);
            }

            Destroy(bodyTree);
        });
    }
}
