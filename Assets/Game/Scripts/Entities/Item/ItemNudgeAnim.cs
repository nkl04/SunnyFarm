namespace SunnyFarm.Game.Entities.Item
{
    using System;
    using System.Collections;
    using UnityEngine;

    public class ItemNudgeAnim : MonoBehaviour
    {
        private WaitForSeconds pause;
        private bool isAnimating = false;

        private void Awake()
        {
            pause = new WaitForSeconds(0.04f);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!isAnimating)
            {
                if (gameObject.transform.position.x < other.transform.position.x)
                {
                    StartCoroutine(RotateAntiClock());
                }
                else
                {
                    StartCoroutine(RotateClock());
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!isAnimating)
            {
                if (gameObject.transform.position.x > other.transform.position.x)
                {
                    StartCoroutine(RotateAntiClock());
                }
                else
                {
                    StartCoroutine(RotateClock());
                }
            }
        }

        private IEnumerator RotateClock()
        {
            isAnimating = true;

            for (int i = 0; i < 4; i++)
            {
                transform.GetChild(0).transform.Rotate(0, 0, -2f);
                yield return pause;
            }

            for (int i = 0; i < 5; i++)
            {
                transform.GetChild(0).transform.Rotate(0, 0, 2f);
                yield return pause;
            }

            transform.GetChild(0).Rotate(0, 0, -2);

            yield return pause;

            isAnimating = false;
        }

        private IEnumerator RotateAntiClock()
        {
            isAnimating = true;

            for (int i = 0; i < 4; i++)
            {
                transform.GetChild(0).transform.Rotate(0, 0, 2f);
                yield return pause;
            }

            for (int i = 0; i < 5; i++)
            {
                transform.GetChild(0).transform.Rotate(0, 0, -2f);
                yield return pause;
            }

            transform.GetChild(0).Rotate(0, 0, 2);

            yield return pause;

            isAnimating = false;
        }
    }
}