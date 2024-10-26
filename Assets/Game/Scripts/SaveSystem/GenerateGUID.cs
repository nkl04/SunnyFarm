using UnityEngine;

[ExecuteAlways]
public class GenerateGUID : MonoBehaviour
{
    [SerializeField] private string gUID = "";

    public string GUID { get => gUID; set => gUID = value; }
    private void Awake()
    {
        // only populate in the editor
        if (!Application.IsPlaying(gameObject))
        {
            // Ensure the object has a guaranteed unique id
            if (gUID == "")
            {
                gUID = System.Guid.NewGuid().ToString();
            }
        }
    }
}
