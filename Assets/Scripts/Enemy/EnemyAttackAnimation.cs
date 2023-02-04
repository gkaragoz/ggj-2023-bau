using DG.Tweening;
using UnityEngine;

public class EnemyAttackAnimation : MonoBehaviour
{
    public float FlyHeight = 1f;
    public float FlyDuration = 0.5f;
    public Ease FlyEaseType = Ease.InOutQuad;

    private Transform _transform;
    private Tween _jumpTween;

    private void Awake()
    {
        _transform = transform;
    }
    
    public void AttackTo(Vector3 targetPosition)
    {
        _jumpTween?.Kill();
        
        Vector2 startPosition = _transform.position;
        Vector2 endPosition = targetPosition;
        Vector2 peekPosition = ((endPosition + startPosition) * 0.5f) + Vector2.up * FlyHeight;

        var path = new Vector3[] { startPosition, peekPosition, endPosition };

        _jumpTween = _transform.DOPath(path, FlyDuration, PathType.CatmullRom, PathMode.TopDown2D)
            .SetEase(FlyEaseType);
    }
}
