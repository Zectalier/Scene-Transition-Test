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

        // Singleton instance
        public static SceneTransitionManager Instance { get; private set; }

        private bool isTransitioning = false;

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

        private IEnumerator TransitionToScene(string sceneName)
        {
            isTransitioning = true;

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
            yield return new WaitForSeconds(transitionAnimator.GetCurrentAnimatorStateInfo(0).length);

            asyncLoad.allowSceneActivation = true;

            isTransitioning = false;
        }
    }
}