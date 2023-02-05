using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ScoreIndicator : MonoBehaviour
    {
        [SerializeField] private RectTransform SelfRectTransform;
        [SerializeField] private TextMeshProUGUI Text;

        private Camera _camera;
        private Color _originalColor;
        
        private void Awake()
        {
            _camera = Camera.main;
            _originalColor = Text.color;
        }

        public void Animate(int score, RectTransform endTransform, RectTransform startTransform, Action<ScoreIndicator> onCompleted)
        {
            Text.text = $"+{score}";

            SelfRectTransform.anchoredPosition = startTransform.anchoredPosition;
            
            transform.SetParent(endTransform);
            var endPosition = SelfRectTransform.anchoredPosition;
            endPosition.y = 0;
            
            gameObject.SetActive(true);

            Text.DOFade(0f, 0.5f).SetDelay(0.25f);
            SelfRectTransform.DOAnchorPos(endPosition, 0.75f)
                 .OnComplete(() => onCompleted?.Invoke(this));
        }

        public void OnTakeFromPool()
        {
            Text.DOKill();
            SelfRectTransform.DOKill();

            Text.color = _originalColor;
            
            gameObject.SetActive(false);
        }
    }
}