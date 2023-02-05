using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAttackAnimation : MonoBehaviour
{
    public float FlyHeight = 1f;
    public float FlyDuration = 0.5f;
    public Ease FlyEaseType = Ease.InOutQuad;

    [SerializeField] private SpriteRenderer ShadowSpriteRenderer;

    private Transform _transform;
    private Tween _jumpTween;
    private Tween _shadowTween;
    private Action OnCompleteAttackAnimation;

    private void Awake()
    {
        _transform = transform;
    }
    
    public void AttackTo(Vector3 targetPosition, Action onComplete = null)
    {
        UpdateShadow();
        
        _jumpTween?.Kill();
        
        Vector2 startPosition = _transform.position;
        Vector2 endPosition = targetPosition;
        Vector2 peekPosition = ((endPosition + startPosition) * 0.5f) + Vector2.up * FlyHeight;
        OnCompleteAttackAnimation = onComplete;
        
        var path = new Vector3[] { startPosition, peekPosition, endPosition };

        _jumpTween = _transform.DOPath(path, FlyDuration, PathType.CatmullRom, PathMode.TopDown2D)
            .SetDelay(Random.Range(0.25f, 1f))
            .SetEase(FlyEaseType)
            .OnComplete(()=> OnCompleteAttackAnimation?.Invoke());
    }

    private void UpdateShadow()
    {
        _shadowTween?.Kill();

        _shadowTween = ShadowSpriteRenderer.DOFade(0f, FlyDuration * 0.5f)
            .OnComplete(() =>
            {
                _shadowTween = ShadowSpriteRenderer.DOFade(1f, FlyDuration * 0.5f);
            });
    }

    public void Clear(bool callBack)
    {
        _jumpTween?.Kill();
        _shadowTween?.Kill();
        OnCompleteAttackAnimation?.Invoke();
    }

    private void OnDestroy()
    {
        Clear(false);
    }
}