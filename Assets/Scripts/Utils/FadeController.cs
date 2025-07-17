using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    /// <summary>
    /// FadeController Utility for managing fade effects for images in Unity.
    /// </summary>
    public class FadeController
    {
        public IEnumerator FadeOut(UnityEngine.UI.Image image, float duration = 1f)
        {
            float elapsedTime = 0f;
            Color startColor = image.color;
            Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                image.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
                yield return null;
            }

            image.color = endColor;
        }

        public IEnumerator FadeIn(UnityEngine.UI.Image image, float duration = 1f)
        {
            float elapsedTime = 0f;
            Color startColor = image.color;
            Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1f);

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                image.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
                yield return null;
            }

            image.color = endColor;
        }
    }
}
