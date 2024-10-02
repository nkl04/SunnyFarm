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

        public Vector2 InitPivot { get; private set; } = new Vector2(-0.1f, 1.1f);
        public void Awake()
        {
            ResetDescription();
        }
        /// <summary>
        /// Reset UI elements
        /// </summary>
        public void ResetDescription()
        {
            title.text = "";
            type.text = "";
            description.text = "";
        }
        /// <summary>
        /// Set data to the UI elements
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="itemType"></param>
        /// <param name="itemDescription"></param>
        public void SetDescription(string itemName, ItemType itemType,
            string itemDescription)
        {
            title.text = itemName;
            //type.text = itemType.ToString();
            description.text = itemDescription;
        }
        /// <summary>
        /// Change pivot of the description
        /// </summary>
        public void ChangePivot()
        {
            Vector2 newPivot = new Vector2(InitPivot.x, -InitPivot.y + 1f);
            GetComponent<RectTransform>().pivot = newPivot;
        }
        /// <summary>
        /// Reset the pivot value
        /// </summary>
        public void ResetPivot()
        {
            GetComponent<RectTransform>().pivot = InitPivot;
        }
    }
}