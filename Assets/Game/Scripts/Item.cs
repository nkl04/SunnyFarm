namespace SunnyFarm.Game.Entities.Item
{
    using SunnyFarm.Game.Entities.Item.Data;
    using SunnyFarm.Game.Managers;
    using SunnyFarm.Game.Utilities.PropertyDrawer;
    using UnityEngine;

    public class Item : MonoBehaviour
    {
        public string ItemID { get => itemID; set => itemID = value; }

        [ItemAttribute]
        [SerializeField] private string itemID;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void Start()
        {
            if (itemID != null)
            {
                Init(itemID);
            }
        }

        private void Init(string itemID)
        {
            if (itemID != null)
            {
                ItemDetail itemDetail = ItemSystemManager.Instance.GetItemDetail(itemID);

                spriteRenderer.sprite = itemDetail.ItemImage;

                if (itemDetail.ItemType == Constant.Enums.ItemType.Reapable_Scenary)
                {
                    gameObject.AddComponent<ItemNudgeAnim>();
                }
            }
        }
    }
}

