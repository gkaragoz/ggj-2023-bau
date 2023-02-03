using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(SimpleFlash))]
public class TakeHitAnimation : MonoBehaviour
{
    private Tween _bounceTween;
    private Tween _colorTween;

    [SerializeField] private Vector3 BounceScale = Vector3.one * 0.1f;
    [SerializeField] private SimpleFlash SimpleFlash;
    
    public void TakeHit()
    {
        _bounceTween?.Kill();
        _bounceTween = transform.DOPunchScale(BounceScale, SimpleFlash.Duration);
        
        SimpleFlash.Flash();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            TakeHit();
    }
}
