using UnityEngine;

namespace Animations
{
    public class EnemyDieTestAnimation : MonoBehaviour
    {
        [SerializeField] private DieAnimation DieAnimation;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Die();
            }
        }

        public void Die()
        {
            DieAnimation.Die(Destroy);
        }
    }
}