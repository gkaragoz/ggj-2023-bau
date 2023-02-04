using System;
using System.Collections;
using DG.Tweening;
using UI;
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
        [SerializeField] private Transform TargetTransform;
        [SerializeField] private Vector3 BounceScale = Vector3.one * 0.1f;
        [SerializeField] private SimpleFlash SimpleFlash;

        private Vector3 _originalScale;

        private void Awake()
        {
            _originalScale = TargetTransform.localScale;
        }

        public void TakeHit(int damageAmount, Vector2 hitPosition, Action onHalfwayCompleted, Action onCompleted, bool pushBack = false)
        {
            IEnumerator HalfwayCallback()
            {
                yield return new WaitForSeconds(SimpleFlash.Duration * 0.5f);
                onHalfwayCompleted?.Invoke();
            }
            
            StopAllCoroutines();
            StartCoroutine(HalfwayCallback());
            
            DamageIndicatorFactory.Instance.Create(damageAmount, transform);

            BounceIt(onCompleted);
            FlashAnimation();

            if (pushBack)
            {
                PushTo(hitPosition);
            }
        }

        private void BounceIt(Action onCompleted)
        {
            _bounceTween?.Kill();
            TargetTransform.localScale = _originalScale;
            _bounceTween = TargetTransform.DOPunchScale(BounceScale, SimpleFlash.Duration)
                .OnComplete(() => onCompleted?.Invoke());
        }

        private void FlashAnimation()
        {
            SimpleFlash.Flash();
        }
        
        private void PushTo(Vector2 hitPosition)
        {
            _takeHitTween?.Kill();
            
            Vector2 startPosition = transform.position;
            var direction = (hitPosition - startPosition).normalized;
            Vector2 endPosition = startPosition - direction * PushBackMultiplier;
            Vector2 peekPosition = ((endPosition + startPosition) * 0.5f) + Vector2.up * FlyHeight;

            var path = new Vector3[] { startPosition, peekPosition, endPosition };

            _takeHitTween = transform.DOPath(path, FlyDuration, PathType.CatmullRom, PathMode.TopDown2D)
                .SetEase(FlyEaseType);
        }

        public void Clear()
        {
            _bounceTween?.Kill();
            _colorTween?.Kill();
            _takeHitTween?.Kill();
        }
    }
}
