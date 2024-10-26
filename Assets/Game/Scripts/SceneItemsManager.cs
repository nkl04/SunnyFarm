using SunnyFarm.Game;
using SunnyFarm.Game.DesignPattern;
using SunnyFarm.Game.Entities.Item;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GenerateGUID))]
public class SceneItemsManager : Singleton<SceneItemsManager>, ISavable
{
    private Transform parentItem;
    [SerializeField] private GameObject itemPrefab = null;

    private string iSavableUniqueID;
    public string ISavableUniqueID { get => iSavableUniqueID; set => iSavableUniqueID = value; }

    private GameObjectSave gameObjectSave;
    public GameObjectSave GameObjectSave { get => gameObjectSave; set => gameObjectSave = value; }


    private void AfterSceneLoad()
    {
        parentItem = GameObject.FindGameObjectWithTag(Constant.Tag.ItemsParentTransform).transform;
    }

    protected override void Awake()
    {
        base.Awake();

        ISavableUniqueID = GetComponent<GenerateGUID>().GUID;
        GameObjectSave = new GameObjectSave();
    }

    private void DestroySceneItems()
    {
        // Get all items in the scene
        Item[] itemsInScene = GameObject.FindObjectsOfType<Item>();

        for (int i = 0; i < itemsInScene.Length; i++)
        {
            Destroy(itemsInScene[i].gameObject);
        }
    }
    public void InstantiateSceneItem(string itemID, Vector3 itemPosition)
    {
        GameObject itemGameObject = Instantiate(itemPrefab, itemPosition, Quaternion.identity);
        Item item = itemGameObject.GetComponent<Item>();
        item.Init(itemID);
    }
    private void InstantiateSceneItems(List<SceneItem> sceneItemList)
    {
        GameObject itemGameObject;

        foreach (var sceneItem in sceneItemList)
        {
            itemGameObject = Instantiate(itemPrefab, new Vector3(sceneItem.Position.x, sceneItem.Position.y, sceneItem.Position.z), Quaternion.identity, parentItem);

            Item item = itemGameObject.GetComponent<Item>();
            item.ItemID = sceneItem.ItemCode;
            item.name = sceneItem.ItemName;
        }
    }
    private void OnDisable()
    {
        ISavableUnregister();
        EventHandlers.OnAfterSceneLoad -= AfterSceneLoad;
    }
    private void OnEnable()
    {
        ISavableRegister();
        EventHandlers.OnAfterSceneLoad += AfterSceneLoad;
    }


    public void ISavableRegister()
    {
        SaveLoadManager.Instance.ISavableObjectList.Add(this);
    }

    public void RestoreScene(string sceneName)
    {
        if (GameObjectSave.sceneData.TryGetValue(sceneName, out SceneSave sceneSave))
        {
            if (sceneSave.listSceneItemDictionary != null
                && sceneSave.listSceneItemDictionary.TryGetValue("sceneItemList", out List<SceneItem> sceneItemList))
            {
                // scene list items found - destroy existing items in scene
                DestroySceneItems();

                // now instanitiate the list of scene items
                InstantiateSceneItems(sceneItemList);
            }
        }
    }

    public void StoreScene(string sceneName)
    {
        // Remove old scene save for gameObject if exists
        GameObjectSave.sceneData.Remove(sceneName);

        // Get all items in the scene
        List<SceneItem> sceneItemList = new List<SceneItem>();
        Item[] itemsInScene = FindObjectsOfType<Item>();

        foreach (Item item in itemsInScene)
        {
            SceneItem sceneItem = new SceneItem();
            sceneItem.ItemCode = item.ItemID;
            sceneItem.Position = new Vector3Serializable(item.transform.position.x,
                item.transform.position.y, item.transform.position.z);
            sceneItem.ItemName = item.name;

            sceneItemList.Add(sceneItem);
        }

        // create list scene items dictionary in scene save and add to it
        SceneSave sceneSave = new SceneSave();
        sceneSave.listSceneItemDictionary = new Dictionary<string, List<SceneItem>>
        {
            { "sceneItemList", sceneItemList }
        };

        // add scene save to gameobject
        GameObjectSave.sceneData.Add(sceneName, sceneSave);
    }

    public void ISavableUnregister()
    {
        SaveLoadManager.Instance.ISavableObjectList.Remove(this);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
