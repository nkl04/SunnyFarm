namespace SunnyFarm.Game.Inventory.UI
{
    using TMPro;
    using UnityEngine;
    using static SunnyFarm.Game.Constant.Enums;

    public class UIInventoryDescription : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textTop1;
        [SerializeField] private TextMeshProUGUI textTop2;
        [SerializeField] private TextMeshProUGUI textTop3;

        [SerializeField] private TextMeshProUGUI textBottom1;
        [SerializeField] private TextMeshProUGUI textBottom2;
        [SerializeField] private TextMeshProUGUI textBottom3;

        /// <summary>
        /// Reset UI elements
        /// </summary>
        public void ResetTextBox()
        {
            this.textTop1.text = "";
            this.textTop2.text = "";
            this.textTop3.text = "";
            this.textBottom1.text = "";
            this.textBottom2.text = "";
            this.textBottom3.text = "";
        }

        /// <summary>
        /// Set text to the description text box
        /// </summary>
        /// <param name="textTop1"></param>
        /// <param name="textTop2"></param>
        /// <param name="textTop3"></param>
        /// <param name="textBottom1"></param>
        /// <param name="textBottom2"></param>
        /// <param name="textBottom3"></param>
        public void SetTextboxText(string textTop1, string textTop2, string textTop3, string textBottom1, string textBottom2, string textBottom3)
        {
            this.textTop1.text = textTop1;
            this.textTop2.text = textTop2;
            this.textTop3.text = textTop3;
            this.textBottom1.text = textBottom1;
            this.textBottom2.text = textBottom2;
            this.textBottom3.text = textBottom3;
        }
    }
}