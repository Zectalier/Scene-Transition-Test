using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Handles button actions for scene transitions.
    /// </summary>
    public class TransitionButtonAction : MonoBehaviour
    {
        public void OnClick(string sceneName)
        {
            // Check if the SceneTransitionManager is available
            if (Transitions.SceneTransitionManager.Instance != null)
            {
                Transitions.SceneTransitionManager.Instance.StartSceneTransition(sceneName);
            }
            else
            {
                Debug.LogError("SceneTransitionManager instance not found. Ensure it is initialized before calling this method.");
            }
        }
    }
}