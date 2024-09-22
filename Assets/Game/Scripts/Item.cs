namespace SunnyFarm.Game.Entities.Item
{
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
            spriteRenderer = GetComponent<SpriteRenderer>();
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

        }
    }
}

