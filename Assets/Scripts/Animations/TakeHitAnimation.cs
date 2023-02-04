using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Animations
{
    [RequireComponent(typeof(SimpleFlash))]
    public class TakeHitAnimation : MonoBehaviour
    {
        private Tween _bounceTween;
        private Tween _colorTween;
        private Tween _takeHitTween;

        // Push Backwards
        [SerializeField] private float FlyHeight = 1f;
        [SerializeField] private float FlyDuration = 0.5f;
        [SerializeField] private Ease FlyEaseType = Ease.InOutQuad;
        [SerializeField] private float PushBackMultiplier = 1;
        
        // Bounce and flash 
        [SerializeField] private Vector3 BounceScale = Vector3.one * 0.1f;
        [SerializeField] private SimpleFlash SimpleFlash;

        private Transform _transform;
        private Vector3 _originalScale;

        private void Awake()
        {
            _transform = transform;
            _originalScale = _transform.localScale;
        }

        public void TakeHit(Vector2 hitPosition, Action onHalfwayCompleted, Action onCompleted)
        {
            IEnumerator HalfwayCallback()
            {
                yield return new WaitForSeconds(SimpleFlash.Duration * 0.5f);
                onHalfwayCompleted?.Invoke();
            }
            
            StopAllCoroutines();
            StartCoroutine(HalfwayCallback());

            BounceIt(onCompleted);
            FlashAnimation();
            PushTo(hitPosition);
        }

        private void BounceIt(Action onCompleted)
        {
            _bounceTween?.Kill();
            _transform.localScale = _originalScale;
            _bounceTween = transform.DOPunchScale(BounceScale, SimpleFlash.Duration)
                .OnComplete(() => onCompleted?.Invoke());
        }

        private void FlashAnimation()
        {
            SimpleFlash.Flash();
        }
        private void PushTo(Vector2 hitPosition)
        {
            _takeHitTween?.Kill();
            
            Vector2 startPosition = _transform.position;
            var direction = (hitPosition - startPosition).normalized;
            Vector2 endPosition = startPosition - direction * PushBackMultiplier;
            Vector2 peekPosition = ((endPosition + startPosition) * 0.5f) + Vector2.up * FlyHeight;

            var path = new Vector3[] { startPosition, peekPosition, endPosition };

            _takeHitTween = _transform.DOPath(path, FlyDuration, PathType.CatmullRom, PathMode.TopDown2D)
                .SetEase(FlyEaseType);
        }
    }
}
