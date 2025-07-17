using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Manages scene transitions
    /// </summary>
    public class SceneTransitionManager : MonoBehaviour
    {
        public Animator transitionAnimator;

        // Number of transitions that can be performed, we choose a random one each time
        public int numberOfTransitions;

        // Fading Image, used to cover loading
        public UnityEngine.UI.Image fadeImage;

        // Duration of the fade effect
        public float fadeOutDuration;

        // Duration of the fade effect
        public float fadeInDuration;

        // Singleton instance
        public static SceneTransitionManager Instance { get; private set; }

        private bool isTransitioning = false;
        private bool canLoadNextScene = false;

        private void Awake()
        {
            // Ensure only one instance exists
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void StartSceneTransition(string sceneName)
        {
            if (isTransitioning)
            {
                Debug.LogWarning("A transition is already in progress.");
                return;
            }
            StartCoroutine(TransitionToScene(sceneName));
        }

        /// <summary>
        /// To be called by the animation event when we can load the next scene. (For some transitions we load when the screen is entirely covered by the animation)
        /// </summary>
        public void LoadNextScene()
        {
            canLoadNextScene = true;
        }

        private IEnumerator TransitionToScene(string sceneName)
        {
            isTransitioning = true;

            // Start fade out effect (since we want to make the black screen appear, we fade in the fadeImage)
            if (fadeImage != null)
            {
                yield return StartCoroutine(new Utils.FadeController().FadeIn(fadeImage, fadeOutDuration));
            }

            if (Random.value < 0.33f)
            {
                yield return new WaitForSeconds(3f);
            }

            AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = false;

            // Simulating a loading delay
            if (Random.value < 0.33f)
            {
                yield return new WaitForSeconds(3f);
            }

            // Waiting for the scene to load
            while (asyncLoad.progress < 0.9f)
            {
                yield return null;
            }

            int randomIndex = Random.Range(0, numberOfTransitions);
            transitionAnimator.SetInteger("TransitionIndex", randomIndex);
            transitionAnimator.SetTrigger("StartTransition");

            // Wait for the transition animation to finish
            while (!canLoadNextScene)
            {
                yield return null;
            }
            canLoadNextScene = false; // Reset for the next transition

            asyncLoad.allowSceneActivation = true;

            // Wait for the scene to activate
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            // Start fade in effect (instantly start the transition)
            if (fadeImage != null)
            {
                yield return StartCoroutine(new Utils.FadeController().FadeOut(fadeImage, fadeInDuration));
            }

            isTransitioning = false;
        }
    }
}