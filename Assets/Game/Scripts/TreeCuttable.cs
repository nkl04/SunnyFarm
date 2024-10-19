using DG.Tweening;
using SunnyFarm.Game.Entities.Player;
using UnityEngine;
public class TreeCuttable : MonoBehaviour, IToolHittable
{
    [Header("Tree Parts")]
    [SerializeField] private GameObject bodyTree;
    [SerializeField] private GameObject stoolTree;

    [Space(5)]

    [Header("Tree Data")]
    [SerializeField] private GameObject dropPrefab;
    [SerializeField] private int bodyDropAmount = 5;
    [SerializeField] private float stoolDropAmount = 5f;
    [SerializeField] private float spread = 3f;

    // Fall animation
    private float fallDegreeZ = 80f;
    private float fallDuration = 1f;
    private float baseTreeLength = 2f;
    private bool isBodyFall = false;

    //TODO: Remove
    public int hp = 10;

    public void Hit(Player player)
    {
        if (hp <= 0) return;

        hp--;

        if (hp == 5)
        {
            BodyFallAnim(player.LastMovementInput);

            return;
        }


        if (bodyTree != null && !isBodyFall)
        {
            RotateBodyAnim();
        }
        else if (stoolTree != null && isBodyFall)
        {
            VibrateStoolAnim();
        }


        if (hp <= 0)
        {
            DropLog(stoolTree.transform.position, (int)stoolDropAmount);

            Destroy(gameObject);
        }
    }


    public void RotateBodyAnim()
    {
        bodyTree.transform.DORotate(new Vector3(0, 0, -3f), 0.1f, RotateMode.LocalAxisAdd)
        .OnComplete(() =>
        {
            bodyTree.transform.DORotate(new Vector3(0, 0, 4f), 0.15f, RotateMode.LocalAxisAdd)
            .OnComplete(() =>
            {
                bodyTree.transform.DORotate(new Vector3(0, 0, -1f), 0.05f, RotateMode.LocalAxisAdd)
                .OnComplete(() =>
                {
                    // do something 
                });
            });
        });
    }

    private void VibrateStoolAnim()
    {
        stoolTree.transform.DOShakePosition(0.5f, new Vector3(0.05f, 0, 0), 10, 0, false, true)
        .OnComplete(() =>
        {
            // do something
        });
    }

    public void BodyFallAnim(Vector2 directionImpact)
    {
        isBodyFall = true;

        float fallDirectionZ = directionImpact.x != 0 ? -directionImpact.x * fallDegreeZ : Mathf.Sign(Random.Range(-1f, 1f)) * fallDegreeZ;

        bodyTree.transform.DORotate(new Vector3(0f, 0f, fallDirectionZ), fallDuration)
            .SetEase(Ease.InCubic)
            .OnComplete(() =>
            {
                Vector3 finalPosition = bodyTree.transform.position;

                Vector3 direction = new Vector3(-fallDirectionZ, 0, 0).normalized;

                // Drop log position when body tree fall
                Vector3 adjustedPosition = finalPosition + direction * baseTreeLength;

                DropLog(adjustedPosition, bodyDropAmount);

                Destroy(bodyTree);
            });
    }


    /// <summary>
    /// Drop logs
    /// </summary>
    /// <param name="position"></param>
    /// <param name="amount"></param>
    private void DropLog(Vector2 position, int amount)
    {
        while (amount > 0)
        {
            amount--;

            Vector3 pos = position;

            pos.x += spread * Random.value - spread / 2;

            pos.y += spread * Random.value - spread / 2;

            GameObject logObject = Instantiate(dropPrefab, pos, Quaternion.identity);

            // log jump anim
            logObject.transform.DOJump(
            pos,
            jumpPower: 1f,
            numJumps: 1,
            duration: 0.5f
            ).OnComplete(() =>
            {

                logObject.transform.DOJump(
                    pos,
                    jumpPower: 0.2f,
                    numJumps: 1,
                    duration: 0.3f
                );
            });
        }
    }
}
