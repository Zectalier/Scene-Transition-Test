using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Transitions
{
    /// <summary>
    /// FadeController Utility for managing fade effects for images in Unity.
    /// </summary>
    public class FadeController
    {
        /// <summary>
        /// Fades out a UI Image over the specified duration.
        /// </summary>
        public IEnumerator FadeOut(Image image, float duration = 1f)
        {
            yield return FadeImage(image, image.color.a, 0f, duration);
        }

        /// <summary>
        /// Fades in a UI Image over the specified duration.
        /// </summary>
        public IEnumerator FadeIn(Image image, float duration = 1f)
        {
            yield return FadeImage(image, image.color.a, 1f, duration);
        }

        /// <summary>
        /// Fades out a CanvasGroup over the specified duration.
        /// </summary>
        public IEnumerator FadeOut(CanvasGroup canvasGroup, float duration = 1f)
        {
            yield return FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 0f, duration);
        }

        /// <summary>
        /// Fades in a CanvasGroup over the specified duration.
        /// </summary>
        public IEnumerator FadeIn(CanvasGroup canvasGroup, float duration = 1f)
        {
            yield return FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 1f, duration);
        }

        private IEnumerator FadeImage(Image image, float startAlpha, float endAlpha, float duration)
        {
            float elapsedTime = 0f;
            Color startColor = image.color;
            Color endColor = new Color(startColor.r, startColor.g, startColor.b, endAlpha);

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                image.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
                yield return null;
            }
            image.color = endColor;
        }

        private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
                yield return null;
            }
            canvasGroup.alpha = endAlpha;
        }
    }
}
