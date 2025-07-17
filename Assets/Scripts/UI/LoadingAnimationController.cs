using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Loading animation controller for a rotating loading icon.
    /// </summary>
    public class LoadingAnimationController : MonoBehaviour
    {
        public float rotationSpeed = 180f; // degrees per second

        void Update()
        {
            transform.Rotate(Vector3.forward, -rotationSpeed * Time.deltaTime);
        }
    }
}
