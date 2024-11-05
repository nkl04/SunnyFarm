using System;
using UnityEngine;
using UnityEngine.UI;

public class FishingManager : MonoBehaviour
{
    [SerializeField] private FishingGameManager fishingGameManager;
    [SerializeField] private Slider fishingMeter;

    private bool isFishing;


    public event Action OnFishingStart;
    public event Action OnFishingEnd;
    public event Action<bool> OnFishingComplete;

    private FishingManager() { }

    public void StartFishing()
    {
        isFishing = true;
        fishingMeter.gameObject.SetActive(true); // Show meter
        fishingMeter.value = 0; // Reset meter
        OnFishingStart?.Invoke();
    }

    public void EndFishing()
    {
        if (isFishing)
        {
            isFishing = false;

            OnFishingEnd?.Invoke();
        }
    }
    public void HideMeter()
    {
        fishingMeter.gameObject.SetActive(false); // Hide meter
    }
    public void CompleteFishing(bool caughtFish)
    {
        OnFishingComplete?.Invoke(caughtFish);
    }

    public void UpdateMeter(float progress)
    {
        if (isFishing)
        {
            fishingMeter.value = progress; // Update meter based on fishing progress
        }
    }
}
