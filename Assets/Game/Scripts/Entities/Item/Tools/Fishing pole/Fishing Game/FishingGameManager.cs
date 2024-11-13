using SunnyFarm.Game;
using System;
using UnityEngine;

public class FishingGameManager : MonoBehaviour
{
    [SerializeField] Transform topPivot;
    [SerializeField] Transform bottomPivot;

    [SerializeField] Transform fishMarker;

    float fishPosition;
    float fishDestination;

    float fishTimer;
    [SerializeField] float timerMultiplicator = 3f;

    float fishSpeed;
    [SerializeField] float smoothMotion = 1f;

    [SerializeField] Transform hook;
    float hookPosition;
    [SerializeField] float hookSize = 0.1f;
    [SerializeField] float hookPower = 0.5f;
    float hookProgress;
    float hookPullVelocity;
    [SerializeField] float hookPullPower = 0.01f;
    [SerializeField] float hookGravityPower = 0.005f;
    [SerializeField] float hookProgressDegradationPower = 0.1f;

    [SerializeField] SpriteRenderer hookSpriteRenderer;

    [SerializeField] Transform progressBarContainer;

    bool pause = false;

    [SerializeField] float failTimer = 10f;

    private enum FishBehavior { Mixed, Smooth, Sinker, Floater, Dart }
    [SerializeField] private FishBehavior fishBehavior;
    [SerializeField] private float difficulty = 20; // Set difficulty, e.g., 80

    public event Action<bool> OnGameFinish;

    private void OnEnable()
    {
        Resize();
    }
    private void OnDisable()
    {
        Reset();
    }

    private void Reset()
    {
        fishPosition = 0;
        fishDestination = 0;
        fishTimer = 0;
        fishSpeed = 0;
        hookPosition = 0;
        hookPullVelocity = 0;
        pause = false;
    }

    private void Resize()
    {
        Bounds b = hookSpriteRenderer.bounds;
        float ySize = b.size.y;
        Vector3 ls = hook.localScale;
        float distance = Vector3.Distance(topPivot.position, bottomPivot.position);
        ls.y = ySize / distance * hookSize;
        hook.localScale = ls;
    }
    private void Update()
    {
        if (pause) return;
        Fish();
        Hook();
        ProgressCheck();
    }
    private void ProgressCheck()
    {
        Vector3 ls = progressBarContainer.localScale;
        ls.y = hookProgress;
        progressBarContainer.localScale = ls;

        float min = hookPosition - hook.localScale.y / 2;
        float max = hookPosition + hook.localScale.y / 2;

        if (min < fishPosition && fishPosition < max)
        {
            hookProgress += hookPower * Time.deltaTime;
        }
        else
        {
            hookProgress -= hookProgressDegradationPower * Time.deltaTime;

            failTimer -= Time.deltaTime;
            if (failTimer < 0)
            {
                Lose();
            }
        }

        if (hookProgress >= 1f)
        {
            Win();
        }

        hookProgress = Mathf.Clamp(hookProgress, 0, 1);
    }

    private void Lose()
    {
        pause = true;
        Debug.Log("Lose");

        EventHandlers.CallOnFishingEnd();
    }

    private void Win()
    {
        pause = true;
        Debug.Log("Catch the fish");
        EventHandlers.CallOnFishingEnd();

    }

    private void Hook()
    {
        if (Input.GetMouseButton(0))
        {
            hookPullVelocity += hookPullPower * Time.deltaTime;
        }

        hookPullVelocity -= hookGravityPower * Time.deltaTime;

        hookPosition += hookPullVelocity;

        if (hookPosition - hook.localScale.y / 2 <= 0f && hookPullVelocity < 0f)
        {
            hookPullVelocity = 0f;
        }
        if (hookPosition + hook.localScale.y / 2 >= 1f && hookPullVelocity > 0f)
        {
            hookPullVelocity = 0f;
        }

        hookPosition = Mathf.Clamp(hookPosition, hook.localScale.y / 2, 1 - hook.localScale.y / 2);
        hook.position = Vector3.Lerp(bottomPivot.position, topPivot.position, hookPosition);
    }
    private void Fish()
    {
        fishTimer -= Time.deltaTime;

        if (fishTimer < 0)
        {
            fishTimer = UnityEngine.Random.value * timerMultiplicator / (1 + difficulty * 0.01f); // Faster movement with higher difficulty

            // Set target position based on behavior and difficulty
            switch (fishBehavior)
            {
                case FishBehavior.Sinker:
                    fishDestination = Mathf.Clamp(UnityEngine.Random.value - (0.2f * difficulty / 100), 0, 1); // Bias lower with difficulty scaling
                    break;
                case FishBehavior.Floater:
                    fishDestination = Mathf.Clamp(UnityEngine.Random.value + (0.2f * difficulty / 100), 0, 1); // Bias higher with difficulty scaling
                    break;
                case FishBehavior.Dart:
                    float difficultyFactor = 1.0f + (difficulty * 0.02f);
                    fishDestination = Mathf.Clamp(fishDestination + (UnityEngine.Random.Range(-0.5f, 0.5f) * difficultyFactor), 0, 1);
                    break;
                default: // Mixed and Smooth
                    fishDestination = UnityEngine.Random.value;
                    break;
            }
        }

        // Adjust smoothness based on behavior and difficulty
        float currentSmoothness = smoothMotion / (1 + (fishBehavior == FishBehavior.Smooth ? 0.5f : 0) + difficulty * 0.01f);

        // Move the fish with adjusted smoothness
        fishPosition = Mathf.SmoothDamp(fishPosition, fishDestination, ref fishSpeed, currentSmoothness);

        // Set the position of the fish marker
        fishMarker.position = Vector3.Lerp(bottomPivot.position, topPivot.position, fishPosition);
    }


}
