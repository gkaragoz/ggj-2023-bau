using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(SimpleFlash))]
public class TakeHitAnimation : MonoBehaviour
{
    private Tween _bounceTween;
    private Tween _colorTween;

    [SerializeField] private Vector3 BounceScale = Vector3.one * 0.1f;
    [SerializeField] private SimpleFlash SimpleFlash;
    
    public void TakeHit(Action onHalfwayCompleted, Action onCompleted)
    {
        IEnumerator HalfwayCallback()
        {
            yield return new WaitForSeconds(SimpleFlash.Duration * 0.5f);
            onHalfwayCompleted?.Invoke();
        }
            
        StopAllCoroutines();
        StartCoroutine(HalfwayCallback());
        
        _bounceTween?.Kill();
        _bounceTween = transform.DOPunchScale(BounceScale, SimpleFlash.Duration)
            .OnComplete(() => onCompleted?.Invoke());
        
        SimpleFlash.Flash();
    }
}
