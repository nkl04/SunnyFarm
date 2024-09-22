namespace SunnyFarm.Game.Utilities.PropertyDrawer.Editor
{
    using UnityEngine;
    using UnityEditor;
    using System;
    using SunnyFarm.Game.Configs;
    using SunnyFarm.Game.Entities.Item.Data;

    [CustomPropertyDrawer(typeof(ItemAttribute))]
    public class ItemDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property) * 2;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that 
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            if (property.propertyType == SerializedPropertyType.String)
            {
                EditorGUI.BeginChangeCheck(); // Start checking for changes

                //Draw item id field
                string itemID = EditorGUI.TextField(new Rect(position.x, position.y, position.width, position.height / 2), label, property.stringValue);

                //Draw item name field
                EditorGUI.LabelField(new Rect(position.x, position.y + position.height / 2, position.width, position.height / 2), "Item Name", GetItemName(property.stringValue));

                if (EditorGUI.EndChangeCheck())
                {
                    property.stringValue = itemID;
                }
            }

            EditorGUI.EndProperty();
        }

        private string GetItemName(string itemID)
        {
            ConfigItemList configItemList = AssetDatabase.LoadAssetAtPath<ConfigItemList>("Assets/Game/Configs/ItemList.asset") as ConfigItemList;

            if (configItemList == null)
            {
                Debug.LogError("Item List Data could not be loaded");
                return "";
            }

            ItemDetail itemDetail = Array.Find(configItemList.itemDetails, item => item.ID == itemID);

            if (itemDetail != null)
            {
                return itemDetail.Name;
            }
            else return "(not found)";
        }
    }
}

