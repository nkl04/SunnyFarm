namespace SunnyFarm.Game
{
    using DG.Tweening;
    using SunnyFarm.Game.DesignPattern;
    using SunnyFarm.Game.Entities.Item;
    using SunnyFarm.Game.Inventory.Data;
    using System;
    using UnityEngine;

    public class ItemWorldSpawner : Singleton<ItemWorldSpawner>
    {
        [SerializeField] private GameObject itemWorldPrefab;

        public Item SpawnItemWorld(Vector3 position, string itemId, int quantity)
        {
            GameObject itemWorldGameObject = Instantiate(itemWorldPrefab, position, Quaternion.identity);

            Item item = itemWorldGameObject.GetComponent<Item>();

            item.SetUp(itemId, quantity);

            return item;
        }

        public Item SpawnItemWorld(Vector3 from, Vector3 to, string itemId, int quantity, float dropHeight, float duration)
        {
            Item item = SpawnItemWorld(from, itemId, quantity);

            item.GetComponent<BoxCollider2D>().enabled = false;

            Vector3 targetPosition = to + new Vector3(0, dropHeight, 0);

            item.transform.DOJump(targetPosition, dropHeight, 1, duration)
                .OnComplete(() =>
                {
                    item.transform.DOMoveY(targetPosition.y - 0.1f, 0.2f)
                     .SetEase(Ease.OutBounce)
                     .OnComplete(() =>
                     {
                         item.GetComponent<BoxCollider2D>().enabled = true;
                     });

                });

            item.transform.DOScale(Vector3.one * 1.2f, duration / 2)
                .SetLoops(2, LoopType.Yoyo);

            return item;
        }
    }
}
