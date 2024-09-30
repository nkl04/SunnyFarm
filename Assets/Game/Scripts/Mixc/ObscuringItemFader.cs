namespace SunnyFarm.Game
{
    using UnityEngine;
    using System.Collections;

    public class ObscuringItemFader : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;

        private void Start()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(Constant.Tag.Player))
            {
                FadeOut();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(Constant.Tag.Player))
            {
                FadeIn();
            }
        }

        public void FadeOut()
        {
            StartCoroutine(FadeOutRoutine());
        }

        public void FadeIn()
        {
            StartCoroutine(FadeInRoutine());
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
        }
    }
}
