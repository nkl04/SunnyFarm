using System;
using UnityEngine;

public class FishingGameManager : MonoBehaviour
{
    public event Action<bool> OnGameFinish;

    private bool _isGameActive;
    private float _gameDuration;
    private float _elapsedTime;

    public void StartGame()
    {
        _isGameActive = true;
        _elapsedTime = 0f;
        _gameDuration = UnityEngine.Random.Range(3f, 10f); // Example duration for game
    }

    public void EndGame()
    {
        _isGameActive = false;
    }

    public void UpdateGame(float deltaTime)
    {
        if (!_isGameActive) return;

        _elapsedTime += deltaTime;

        if (_elapsedTime >= _gameDuration)
        {
            FinishGame(true); // Assuming the player catches a fish on completion
        }
    }

    private void FinishGame(bool fishCaught)
    {
        _isGameActive = false;
        OnGameFinish?.Invoke(fishCaught);
    }
}
