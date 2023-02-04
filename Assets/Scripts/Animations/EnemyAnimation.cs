using UnityEngine;

namespace Animations
{
    public class EnemyAnimation : MonoBehaviour
    {
        [SerializeField] private TakeHitAnimation TakeHitAnimation;
    
        public void TakeHit()
        {
            TakeHitAnimation.TakeHit(transform.position, null, null);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TakeHit();
            }
        }
    }
}