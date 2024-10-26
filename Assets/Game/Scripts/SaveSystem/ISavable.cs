public interface ISavable
{
    string ISavableUniqueID { get; set; }
    GameObjectSave GameObjectSave { get; set; }
    void ISavableRegister();
    void ISavableUnregister();
    void StoreScene(string sceneName);
    void RestoreScene(string sceneName);
}
