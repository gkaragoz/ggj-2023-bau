using DG.Tweening;
using UnityEngine;

namespace Animations
{
    public class DieAnimation : MonoBehaviour
    {
        [SerializeField] private ParticleSystem DieParticles;
        [SerializeField] private float ScaleDownDuration = 0.5F;
        [SerializeField] private SpriteRenderer SpriteRenderer;
    
        private Tween _scaleDownTween;

        public void Die()
        {
            var dieParticlesTransform = DieParticles.transform;
        
            dieParticlesTransform.SetParent(null);
            dieParticlesTransform.localScale = Vector3.one;
            DieParticles.Play();
        
            _scaleDownTween?.Kill();
            _scaleDownTween = transform.DOScale(Vector3.zero, ScaleDownDuration)
                .OnComplete(() =>
                {
                    SpriteRenderer.enabled = false;
                });
        }
    }
}
