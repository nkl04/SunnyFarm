using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeCuttable : ToolHit
{
    [SerializeField] private GameObject dropPrefab;
    [SerializeField] private int dropAmount = 5;
    [SerializeField] private float spread = 2f;

    public override void Hit()
    {
        while (dropAmount > 0)
        {
            dropAmount--;

            Vector3 pos = transform.position;
            pos.x += spread * Random.value - spread / 2;
            pos.y += spread * Random.value - spread / 2;

            Instantiate(dropPrefab, pos, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
