using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class SteadyScale : MonoBehaviour
    {
        [SerializeField] private RectTransform TargetTransform;
        [SerializeField] private Vector3 TargetScale = Vector3.one * 1.5f;
        [SerializeField] private float Duration = 1f;
        [SerializeField] private Ease EaseType = Ease.InOutBack;

        private void OnEnable()
        {
            TargetTransform.DOScale(TargetScale, Duration)
                .SetEase(EaseType)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void OnDisable()
        {
            TargetTransform.DOKill();
        }
    }
}