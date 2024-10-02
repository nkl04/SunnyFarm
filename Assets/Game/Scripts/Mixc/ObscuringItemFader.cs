namespace SunnyFarm.Game
{
    using System.Collections;
    using UnityEngine;

    public class ObscuringItemFader : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private Coroutine activeFadeRoutine = null; // Track the currently active fade coroutine

        private void Start()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(Constant.Tag.Player))
            {
                StartFadeOut();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(Constant.Tag.Player))
            {
                StartFadeIn();
            }
        }

        public void StartFadeOut()
        {
            if (activeFadeRoutine != null)
            {
                StopCoroutine(activeFadeRoutine); // Stop the current coroutine if it exists
            }
            activeFadeRoutine = StartCoroutine(FadeOutRoutine()); // Start the new fade out coroutine
        }

        public void StartFadeIn()
        {
            if (activeFadeRoutine != null)
            {
                StopCoroutine(activeFadeRoutine); // Stop the current coroutine if it exists
            }
            activeFadeRoutine = StartCoroutine(FadeInRoutine()); // Start the new fade in coroutine
        }

        private IEnumerator FadeOutRoutine()
        {
            float currentAlpha = spriteRenderer.color.a;
            float distance = currentAlpha - Constant.ColorStat.TargetAlpha;

            while (spriteRenderer.color.a > Constant.ColorStat.TargetAlpha)
            {
                currentAlpha -= distance * Time.deltaTime / Constant.ColorStat.FadeOutSeconds;
                spriteRenderer.color = new Color(1, 1, 1, currentAlpha);
                yield return null;
            }

            spriteRenderer.color = new Color(1, 1, 1, Constant.ColorStat.TargetAlpha);
            activeFadeRoutine = null; // Reset the coroutine tracker
            Debug.Log("Faded out");
        }

        private IEnumerator FadeInRoutine()
        {
            float currentAlpha = spriteRenderer.color.a;
            float distance = 1 - currentAlpha;

            while (spriteRenderer.color.a < 1)
            {
                currentAlpha += distance * Time.deltaTime / Constant.ColorStat.FadeInSeconds;
                spriteRenderer.color = new Color(1, 1, 1, currentAlpha);
                yield return null;
            }

            activeFadeRoutine = null; // Reset the coroutine tracker
            Debug.Log("Faded in");
        }
    }
}
