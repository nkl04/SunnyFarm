namespace SunnyFarm.Game.Inventory.UI
{
    using TMPro;
    using UnityEngine;
    using static SunnyFarm.Game.Constant.Enums;

    public class UIInventoryDescription : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text title;
        [SerializeField]
        private TMP_Text type;
        [SerializeField]
        private TMP_Text description;

        public Vector2 InitPivot { get; private set; }
        public void Awake()
        {
            InitPivot = GetComponent<RectTransform>().pivot;
            ResetDescription();
        }

        public void ResetDescription()
        {
            title.text = "";
            type.text = "";
            description.text = "";
        }

        public void SetDescription(string itemName, ItemType itemType,
            string itemDescription)
        {
            title.text = itemName;
            //type.text = itemType.ToString();
            description.text = itemDescription;
        }
        public void ResetPivot()
        {
            GetComponent<RectTransform>().pivot = InitPivot;
        }
    }
}