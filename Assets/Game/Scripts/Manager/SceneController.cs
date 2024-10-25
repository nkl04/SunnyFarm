namespace SunnyFarm.Game
{
    using System;
    using System.Collections;
    using SunnyFarm.Game.DesignPattern;
    using SunnyFarm.Game.Entities.Player;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;
    using static SunnyFarm.Game.Constant.Enums;

    public class SceneController : Singleton<SceneController>
    {
        public SceneName sceneName;
        [SerializeField] private Image fadeImage;
        [SerializeField] private float fadeDuration = 1f;
        [SerializeField] private CanvasGroup fadeCanvasGroup = null;
        private bool isFading;


        private IEnumerator Start()
        {
            fadeImage.color = new Color(0f, 0f, 0f, 1f);
            fadeCanvasGroup.alpha = 1f;

            // Start the first scene and wait until it's finished loading
            yield return StartCoroutine(LoadAndActiveScene(sceneName));

            // If this event has any subscribers, call it
            EventHandlers.CallOnAfterSceneLoad();

            // Start fading back in and wait until fade is finished
            yield return StartCoroutine(Fade(0f));
        }

        // This will be called when the player wants to switch scenes
        public void FadeAndLoadScene(SceneName sceneName, Vector3 spawnPosition)
        {
            // If a fade isn't happening, start fading and switching scenes
            if (!isFading)
            {
                StartCoroutine(FadeAndSwitchScenes(sceneName, spawnPosition));
            }
        }


        private IEnumerator FadeAndSwitchScenes(SceneName sceneName, Vector3 spawnPosition)
        {
            // Call before scene unload fade out event
            EventHandlers.CallOnBeforeSceneUnloadFadeOut();

            // Start fading to black and wait until fade is finished
            yield return StartCoroutine(Fade(1f));

            // Set player position
            Player.Instance.transform.position = spawnPosition;

            // Call before scene unload event
            EventHandlers.CallOnBeforeSceneUnload();

            // Unload the current active scene
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

            // Start loading the given scene and wait until it's finished loading  
            yield return StartCoroutine(LoadAndActiveScene(sceneName));

            // Call after scene load event
            EventHandlers.CallOnAfterSceneLoad();

            // Start fading back in and wait until fade is finished
            yield return StartCoroutine(Fade(0f));

            // Call after scene load fade in event
            EventHandlers.CallOnAfterSceneLoadFadeIn();
        }

        private IEnumerator LoadAndActiveScene(SceneName sceneName)
        {
            // Allow the given scene to be active
            yield return SceneManager.LoadSceneAsync(sceneName.ToString(), LoadSceneMode.Additive);

            // Get the newly loaded scene
            Scene newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

            // Set the newly loaded scene as the active scene
            SceneManager.SetActiveScene(newlyLoadedScene);
        }

        private IEnumerator Fade(float alpha)
        {
            isFading = true;

            // Make sure the CanvasGroup blocks raycasts so player can't interact with the UI
            fadeCanvasGroup.blocksRaycasts = true;

            // Calculate how fast the CanvasGroup should fade based on its current alpha, its final alpha, and how long it has to change between the two
            float fadeSpeed = Mathf.Abs(fadeCanvasGroup.alpha - alpha) / fadeDuration;

            // While the CanvasGroup hasn't reached the final alpha yet...
            while (!Mathf.Approximately(fadeCanvasGroup.alpha, alpha))
            {
                // ... move the alpha towards its target alpha
                fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, alpha, fadeSpeed * Time.deltaTime);

                // Wait for a frame then continue
                yield return null;
            }

            isFading = false;

            // Stop blocking raycasts so player can interact with the UI
            fadeCanvasGroup.blocksRaycasts = false;
        }


    }
}
