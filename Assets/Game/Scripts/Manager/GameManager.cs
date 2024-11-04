namespace SunnyFarm.Game.Managers
{
    using SunnyFarm.Game.DesignPattern;
    using SunnyFarm.Game.Entities.Player;
    using SunnyFarm.Game.Inventory.Data;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    public class GameManager : Singleton<GameManager>
    {
        public Dictionary<InventoryKey, Player> players;

        protected override void Awake()
        {
            base.Awake();
            players = new Dictionary<InventoryKey, Player>();
        }

        public void RegisterPlayer(InventoryKey inventoryKey, Player player)
        {
            players.Add(inventoryKey, player);
        }

        public Player GetPlayer(InventoryKey inventoryKey)
        {
            if (players.ContainsKey(inventoryKey))
            {
                return players[inventoryKey];
            }
            return null;
        }
    }
}
