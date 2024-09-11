namespace SunnyFarm.Game.Inventory.Data
{
    using SunnyFarm.Game.DesignPattern.Singleton;

    public class InventoryDataController : Singleton<InventoryDataController>
    {
        public int InventoryLevel { get; private set; } = 1;
        // Start is called before the first frame update
        void Start()
        {
            GetData();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void SaveData()
        {

        }

        void GetData()
        {

        }
    }
}