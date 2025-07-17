using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Transitions
{
    [System.Serializable]
    public struct TransitionConfig
    {
        public int transitionIndex;
        public float fadeOutDuration;
        public float fadeInDuration;
        public bool needsFade;
    }

    /// <summary>
    /// Manages scene transitions
    /// </summary>
    public class SceneTransitionManager : MonoBehaviour
    {
        public Animator transitionAnimator;

        [Header("Number of transitions that can be performed, we choose a random one each time")]
        public List<TransitionConfig> transitions = new List<TransitionConfig>();

        [Header("Fading Image, used to cover loading")]
        public UnityEngine.UI.Image fadeImage;

        [Header("Canvas Group Transition, used for special fading effects (e.g. we need to fade the transition in and out)")]
        public CanvasGroup transitionCanvasGroup;

        // Singleton instance
        public static SceneTransitionManager Instance { get; private set; }

        public GameObject loadingAnimation;

        public TransitionSoundEvents TransitionSoundEvents;

        private FadeController fadeController = new FadeController();

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

        public void PlayTransitionSound()
        {
            TransitionSoundEvents?.PlayTransitionSound();
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

            TransitionConfig selectedTransition = transitions[Random.Range(0, transitions.Count)];

            // Start fade out effect (since we want to make the black screen appear, we fade in the fadeImage)
            yield return FadeImageIn(selectedTransition.fadeOutDuration);

            AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = false;

            // Simulating a loading delay
            if (Random.value < 0.33f)
            {
                if (loadingAnimation != null)
                {
                    loadingAnimation.SetActive(true);
                }
                yield return new WaitForSeconds(3f);
            }

            // Waiting for the scene to load
            while (asyncLoad.progress < 0.9f)
            {
                yield return null;
            }

            SetupCanvasGroup(selectedTransition);

            if (loadingAnimation != null)
            {
                loadingAnimation.SetActive(false); // Hide loading animation if it was shown
            }

            if (transitionAnimator != null)
            {
                transitionAnimator.SetInteger("TransitionIndex", selectedTransition.transitionIndex);
                transitionAnimator.SetTrigger("StartTransition");
            }

            yield return FadeCanvasGroupIn(selectedTransition);

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


            yield return FadeCanvasGroupOut(selectedTransition);

            yield return FadeImageOut(selectedTransition.fadeInDuration);

            isTransitioning = false;
        }

        private void SetupCanvasGroup(TransitionConfig config)
        {
            if (transitionCanvasGroup != null)
                transitionCanvasGroup.alpha = config.needsFade ? 0f : 1f;
        }

        private IEnumerator FadeImageIn(float duration)
        {
            if (fadeImage != null)
                yield return StartCoroutine(fadeController.FadeIn(fadeImage, duration));
        }

        private IEnumerator FadeImageOut(float duration)
        {
            if (fadeImage != null)
                yield return StartCoroutine(fadeController.FadeOut(fadeImage, duration));
        }

        private IEnumerator FadeCanvasGroupIn(TransitionConfig config)
        {
            if (config.needsFade && transitionCanvasGroup != null)
                yield return StartCoroutine(fadeController.FadeIn(transitionCanvasGroup, config.fadeInDuration));
        }

        private IEnumerator FadeCanvasGroupOut(TransitionConfig config)
        {
            if (config.needsFade && transitionCanvasGroup != null)
                yield return StartCoroutine(fadeController.FadeOut(transitionCanvasGroup, config.fadeOutDuration));
        }
    }
}