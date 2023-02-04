using DG.Tweening;
using UnityEngine;

namespace Animations
{
    public class EnemyBlood : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer SpriteRenderer;
        [SerializeField] private Sprite[] BloodSprites;
        [SerializeField] private float FadeInDuration = 0.25F;
        [SerializeField] private float ScaleInDuration = 0.5F;
        
        private Vector3 _originalScale;
        
        private void Awake()
        {
            _originalScale = transform.localScale;
        }

        private Sprite GetRandomBloodSprite()
        {
            return BloodSprites[Random.Range(0, BloodSprites.Length)];
        } 

        public void Create()
        {
            transform.SetParent(null);
            transform.localScale = Vector3.zero;
            SpriteRenderer.sprite = GetRandomBloodSprite();

            gameObject.SetActive(true);
            
            SpriteRenderer.DOFade(1f, FadeInDuration);
            transform.DOScale(_originalScale, ScaleInDuration);
        }
    }
}