using SunnyFarm.Game;
using System;
using UnityEngine;

public class FishingManager : MonoBehaviour
{
    public float baseMinBiteTime = 0.6f;
    public float baseMaxBiteTime = 30f;
    public int fishingLevel = 0;
    public bool isFirstBite = true;

    [SerializeField] private FishingGameManager fishingGameManager;

    private bool isFishing;
    private bool isFishBite;

    private float biteTimer;
    private bool isStartFishGame = false;

    public event Action OnFishingEnd;
    public event Action<bool> OnFishingComplete;

    private void Start()
    {
        EventHandlers.OnFishingStart += StartFishing;
        EventHandlers.OnOpenFishingGame += OpenFishingGame;
        EventHandlers.OnFishingEnd += EndFishing;
    }

    private void Update()
    {
        if (isFishing && !isStartFishGame)
        {
            biteTimer -= Time.deltaTime;

            //Debug.Log(biteTimer);
            if (biteTimer < 0)
            {
                // wait for player click to start the fishing game
                isFirstBite = false;
                Debug.Log("Fish bite");
                isFishBite = true;
            }
            if (biteTimer < -1.5f)
            {
                // if the player not click on time, reset the bite time
                isFishBite = false;
                Debug.Log("Reset timer");
                biteTimer = CalculateBiteTime();
            }
        }
    }
    public float CalculateBiteTime()
    {
        float minTime = baseMinBiteTime;
        float maxTime = baseMaxBiteTime;

        // Adjust max time based on fishing level
        maxTime -= fishingLevel * 0.25f;

        // Ensure max time doesn't drop below min time
        maxTime = Mathf.Max(minTime, maxTime);

        // Apply first bite reduction
        if (isFirstBite)
        {
            minTime *= 0.75f;
            maxTime *= 0.75f;
        }

        // Ensure minTime is no less than the allowed minimum
        minTime = Mathf.Max(0.5f, minTime);

        // Randomize bite time between min and max
        return UnityEngine.Random.Range(minTime, maxTime);
    }
    public void StartFishing()
    {
        isFishing = true;

        isFirstBite = true;
        biteTimer = CalculateBiteTime();

    }

    public void OpenFishingGame()
    {
        if (isFishBite)
        {
            fishingGameManager.gameObject.SetActive(true);
            isStartFishGame = true;
        }
        else
        {
            EventHandlers.CallOnFishingEnd();
        }
    }
    public void EndFishing()
    {

        Reset();
    }

    private void Reset()
    {
        isFishing = false;

        isFirstBite = true;

        isFishBite = false;

        biteTimer = 0;

        isStartFishGame = false;
    }

    public void CompleteFishing(bool caughtFish)
    {
        OnFishingComplete?.Invoke(caughtFish);
    }


}
