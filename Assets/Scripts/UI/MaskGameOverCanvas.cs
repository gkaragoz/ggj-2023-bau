using DG.Tweening;
using Gameplay;
using Main_Character;
using UnityEngine;

namespace UI
{
    public class MaskGameOverCanvas : MonoBehaviour
    {
        [SerializeField] private RectTransform ParentRectTransform;
        [SerializeField] private CanvasGroup CanvasGroup;
        [SerializeField] private RectTransform MaskRectTransform;

        private Camera _camera;
        
        private void Awake()
        {
            _camera = Camera.main;
            CanvasGroup.alpha = 0;
        }

        private void OnEnable()
        {
            GameManager.OnComplete += OnGameOver;
        }

        private void OnDisable()
        {
            GameManager.OnComplete -= OnGameOver;
        }

        private void OnGameOver()
        {
            var targetPosition = MainCharacterController.Instance.transform.position;
            var parentCanvasSizeDelta = ParentRectTransform.sizeDelta;
            var viewportPosition = _camera.WorldToViewportPoint(targetPosition);
            var worldObjectScreenPosition = new Vector2(
                viewportPosition.x * parentCanvasSizeDelta.x - parentCanvasSizeDelta.x * 0.5f,
                viewportPosition.y * parentCanvasSizeDelta.y - parentCanvasSizeDelta.y * 0.5f);

            MaskRectTransform.anchoredPosition = worldObjectScreenPosition + (Vector2.up * 50);
            
            CanvasGroup.DOFade(1, 0.25f);
        }
    }
}