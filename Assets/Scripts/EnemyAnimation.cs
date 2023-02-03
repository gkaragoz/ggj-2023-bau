using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] private TakeHitAnimation TakeHitAnimation;
    [SerializeField] private DieAnimation DieAnimation;
    
    public void TakeHit()
    {
        TakeHitAnimation.TakeHit(() =>
        {
            Die();
        }, null);
    }

    public void Die()
    {
        DieAnimation.Die();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeHit();
        }
    }
}