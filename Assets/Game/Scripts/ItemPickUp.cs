namespace SunnyFarm.Game
{
    using System;
    using System.Collections;
    using SunnyFarm.Game.Entities.Item;
    using SunnyFarm.Game.Entities.Item.Data;
    using SunnyFarm.Game.Managers;
    using Unity.VisualScripting;
    using UnityEngine;

    public class ItemPickUp : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private float pickUpDistance = 2f;
        [SerializeField] private CircleCollider2D lootingArea;

        private void Start()
        {
            lootingArea.radius = pickUpDistance;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Item item = collision.GetComponent<Item>();
            if (item != null)
            {
                ItemDetail itemDetail = ItemSystemManager.Instance.GetItemDetail(item.ItemID);
                if (itemDetail.CanBePickUp)
                {
                    StartCoroutine(MoveItemToPlayer(item));
                }
            }
        }

        private IEnumerator MoveItemToPlayer(Item item)
        {
            Vector3 playerPos = transform.position;

            Vector3 itemPos = item.transform.position;

            float distance = Vector3.Distance(itemPos, playerPos);

            while (item != null && distance > 0.1f)
            {
                playerPos = transform.position;

                itemPos = item.transform.position;

                distance = Vector3.Distance(item.transform.position, playerPos);

                item.transform.position = Vector3.MoveTowards(item.transform.position, playerPos, speed * Time.deltaTime);

                yield return null;
            }

            CollectItem(item);
        }

        /// <summary>
        /// Collects the item and destroys it
        /// </summary>
        /// <param name="item"></param>
        private void CollectItem(Item item)
        {
            if (item != null)
            {
                Destroy(item.gameObject);
            }
        }
    }
}
