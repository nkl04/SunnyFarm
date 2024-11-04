namespace SunnyFarm.Game.Entities.Item
{
    using SunnyFarm.Game.Entities.Item.Data;
    using SunnyFarm.Game.Managers;
    using SunnyFarm.Game.Utilities.PropertyDrawer;
    using UnityEngine;

    public class Item : MonoBehaviour
    {
        public string ItemID { get => itemID; set => itemID = value; }

        public int Quantity { get => quantity; set => quantity = value; }

        [ItemAttribute]
        [SerializeField] private string itemID;

        [SerializeField] private int quantity;

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

        public void SetUp(string itemID, int quantity = 1)
        {
            this.itemID = itemID;
            this.quantity = quantity;
        }

        public void Init(string itemID)
        {
            if (itemID != null)
            {
                ConfigItem itemDetail = ItemSystemManager.Instance.GetItemDetail(itemID);

                spriteRenderer.sprite = itemDetail.ItemImage;

                if (itemDetail.ItemType == Constant.Enums.ItemType.Reapable_Scenary)
                {
                    gameObject.AddComponent<ItemNudgeAnim>();
                }
            }
        }
    }
}

