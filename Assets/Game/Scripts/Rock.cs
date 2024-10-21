using DG.Tweening;
using SunnyFarm.Game.Entities.Player;
using UnityEngine;

public class Rock : MonoBehaviour, IToolHittable
{
    [Header("Tree Data")]
    [SerializeField] private GameObject dropPrefab;
    [SerializeField] private float rockDropAmount = 1f;
    [SerializeField] private float spread = 3f;

    [SerializeField] public float duration = 1f;      // Duration of the shake
    [SerializeField] public float strength = 1f;      // Strength of the shake
    [SerializeField] public int vibrato = 10;         // Number of shakes
    [SerializeField] public float randomness = 90f;   // Randomness of the shake

    //TODO: Remove
    public int hp = 1;

    public void Hit(Player player)
    {
        hp--;


        if (hp <= 0)
        {
            DropStone(transform.position, (int)rockDropAmount);

            Destroy(gameObject);
        }

        Shake();
    }

    // Call this function to shake the object
    public void Shake()
    {
        // Shake the position of the object
        transform.DOShakePosition(duration, strength, vibrato, randomness);
    }

    /// <summary>
    /// Drop logs
    /// </summary>
    /// <param name="position"></param>
    /// <param name="amount"></param>
    private void DropStone(Vector2 position, int amount)
    {
        while (amount > 0)
        {
            amount--;

            Vector3 pos = position;

            pos.x += spread * Random.value - spread / 2;

            pos.y += spread * Random.value - spread / 2;

            GameObject rockObject = Instantiate(dropPrefab, pos, Quaternion.identity);

            // log jump anim
            rockObject.transform.DOJump(
            pos,
            jumpPower: 1f,
            numJumps: 1,
            duration: 0.5f
            ).OnComplete(() =>
            {

                rockObject.transform.DOJump(
                    pos,
                    jumpPower: 0.2f,
                    numJumps: 1,
                    duration: 0.3f
                );
            });
        }
    }

}
