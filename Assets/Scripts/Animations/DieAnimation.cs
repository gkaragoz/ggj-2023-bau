using System;
using DG.Tweening;
using UnityEngine;

namespace Animations
{
    public class DieAnimation : MonoBehaviour
    {
        [SerializeField] private ParticleSystem DieParticles;
        [SerializeField] private float ScaleUpDuration = 0.5F;
        [SerializeField] private SpriteRenderer CharacterSpriteRenderer;
        [SerializeField] private SpriteRenderer CharacterShadowSpriteRenderer;
    
        private Tween _scaleDownTween;
        private Tween _characterAlphaTween;
        private Tween _characterShadowAlphaTween;

        public void Die(Action<GameObject> onCompleted)
        {
            var dieParticlesTransform = DieParticles.transform;
        
            dieParticlesTransform.SetParent(null);
            dieParticlesTransform.localScale = Vector3.one;
            Destroy(DieParticles.gameObject, DieParticles.main.duration);
            DieParticles.Play();
        
            _characterAlphaTween?.Kill();
            _characterAlphaTween = CharacterSpriteRenderer.DOFade(0f, ScaleUpDuration);
            
            _characterShadowAlphaTween?.Kill();
            _characterShadowAlphaTween = CharacterShadowSpriteRenderer.DOFade(0f, ScaleUpDuration);
            
            _scaleDownTween?.Kill();
            _scaleDownTween = transform.DOScale(Vector3.one * 2f, ScaleUpDuration)
                .OnComplete(() =>
                {
                    CharacterSpriteRenderer.enabled = false;
                    onCompleted?.Invoke(gameObject);
                });
        }
    }
}
