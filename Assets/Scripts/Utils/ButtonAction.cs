using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    /// <summary>
    /// Handles button actions for scene transitions.
    /// </summary>
    public class TransitionButtonAction : MonoBehaviour
    {
        public void OnClick(string sceneName)
        {
            // Check if the SceneTransitionManager is available
            if (Managers.SceneTransitionManager.Instance != null)
            {
                Managers.SceneTransitionManager.Instance.StartSceneTransition(sceneName);
            }
            else
            {
                Debug.LogError("SceneTransitionManager instance not found. Ensure it is initialized before calling this method.");
            }
        }
    }
}