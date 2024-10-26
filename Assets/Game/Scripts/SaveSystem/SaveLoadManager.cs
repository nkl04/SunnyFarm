using SunnyFarm.Game.DesignPattern;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SaveLoadManager : Singleton<SaveLoadManager>
{
    public List<ISavable> ISavableObjectList;

    protected override void Awake()
    {
        base.Awake();

        ISavableObjectList = new List<ISavable>();
    }

    public void StoreCurrentSceneData()
    {
        foreach (ISavable iSavableObj in ISavableObjectList)
        {
            iSavableObj.StoreScene(SceneManager.GetActiveScene().name);
        }
    }
    public void RestoreCurrentSceneData()
    {
        foreach (ISavable iSavableObj in ISavableObjectList)
        {
            iSavableObj.RestoreScene(SceneManager.GetActiveScene().name);
        }
    }
}
