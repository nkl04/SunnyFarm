[System.Serializable]
public class SceneItem
{
    public string ItemCode;
    public Vector3Serializable Position;
    public string ItemName;

    public SceneItem()
    {
        Position = new Vector3Serializable();
    }
}
