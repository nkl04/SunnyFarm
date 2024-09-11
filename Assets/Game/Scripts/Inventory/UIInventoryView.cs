namespace SunnyFarm.Game.Inventory.UI
{
    using UnityEngine;
    using UnityEngine.InputSystem;
    public class UIInventoryView : MonoBehaviour
    {
        // field to store the quick-access items
        //[SerializeField] private 

        // field to store not quick-access items
        //

        [SerializeField]
        private UIInventoryDescription uiInventoryDescription;

        /// <summary>
        /// Init item slot ui for the whole inventory 
        /// </summary>
        /// <param name="capacity"></param>
        public void InitializeInventoryUI(int capacity)
        {
            for (int i = 1; i <= capacity; i++)
            {
                // init for level 1 of inventory
                if (i <= 10)
                {

                }
                // init for level 2 of inventory
                else if (i > 10 && i <= 20)
                {

                }
                // init for level 3 of inventory
                else if (i > 20 && i <= 30)
                {

                }
            }
        }


        /// <summary>
        /// Open or close the inventory UI based on is E pressed?
        /// </summary>
        /// <param name="context"></param>
        public void OnOpenOrCloseInventory(InputAction.CallbackContext context)
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);
            else gameObject.SetActive(false);
        }

    }
}