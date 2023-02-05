using Animations;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private TakeHitAnimation TakeHitAnimation;
        [SerializeField] private RectTransform ParentRectTransform;
        [SerializeField] private Image HealthBarFillImage;
        [SerializeField] private Image HealthBarDamageIndicatorImage;
        [SerializeField] private CanvasGroup CanvasGroup;
        
        [SerializeField] private float ShakeStrength = 1.05f;
        [SerializeField] private float ShakeDuration = 0.25f;
        [SerializeField] private float DamageIndicatorDuration = 0.5f;

        private Tween _shakeTween;
        private Tween _healthIndicatorTween;

        private Vector3 _parentScale;
        
        private void Awake()
        {
            _parentScale = ParentRectTransform.localScale;
        }

        public void Hide()
        {
            CanvasGroup.DOFade(0F, .5F);
        }

        public void SetHealth(float currentHealth, float maxHealth)
        {
            var healthPercentage = currentHealth / maxHealth;
            HealthBarFillImage.fillAmount = healthPercentage;
            HealthBarDamageIndicatorImage.fillAmount = healthPercentage;
        }

        public void TakeDamage(int damageAmount, Vector3 hitPoint, float currentHealth, float maxHealth, bool pushBack = false)
        {
            _shakeTween?.Kill();
            ParentRectTransform.localScale = _parentScale;
            _shakeTween = ParentRectTransform.DOPunchScale(_parentScale * ShakeStrength, ShakeDuration);

            var healthPercentage = currentHealth / maxHealth;
            HealthBarFillImage.fillAmount = healthPercentage;

            _healthIndicatorTween?.Kill();
            _healthIndicatorTween = HealthBarDamageIndicatorImage.DOFillAmount(healthPercentage, DamageIndicatorDuration);
            
            TakeHitAnimation.TakeHit(damageAmount, hitPoint, null, null, pushBack);
        }
    }
}
