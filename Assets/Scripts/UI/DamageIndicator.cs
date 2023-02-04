using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI
{
    public class DamageIndicator : MonoBehaviour
    {
        [SerializeField] private RectTransform SelfRectTransform;
        [SerializeField] private TextMeshProUGUI Text;

        private Camera _camera;
        private Tween _animationTween;
        
        private void Awake()
        {
            _camera = Camera.main;
        }

        public void Animate(string text, Transform targetTransform, RectTransform parentCanvas)
        {
            Text.text = text;

            var targetPosition = targetTransform.position;
            var parentCanvasSizeDelta = parentCanvas.sizeDelta;
            var viewportPosition = _camera.WorldToViewportPoint(targetPosition);
            var worldObjectScreenPosition = new Vector2(
                viewportPosition.x * parentCanvasSizeDelta.x - parentCanvasSizeDelta.x * 0.5f,
                viewportPosition.y * parentCanvasSizeDelta.y - parentCanvasSizeDelta.y * 0.5f);
        
            SelfRectTransform.anchoredPosition = worldObjectScreenPosition;

            var startPosition = (Vector2)SelfRectTransform.transform.position + Vector2.up * 20f;
            var peekPosition = startPosition + Vector2.one * 20f;
            var endPosition = peekPosition + Vector2.one * 50f;
            var movementPath = new Vector3[]
            {
                startPosition,
                peekPosition,
                endPosition
            };
            
            _animationTween?.Kill();
            _animationTween = SelfRectTransform.DOPath(movementPath, 0.5f, PathType.CatmullRom)
                .OnComplete(() => gameObject.SetActive(false));
        }
    }
}