using System;
using UnityEngine;

namespace Animations
{
    public class TakeHitAnimationTest : MonoBehaviour
    {
        [SerializeField] private Transform KnifeTransform;
        [SerializeField] private TakeHitAnimation TakeHitAnimation;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                TakeHitAnimation.TakeHit(KnifeTransform.position, null, null);
            }
        }
    }
}