using UnityEngine;

namespace Animations
{
    public class EnemyAttackTestAnimation : MonoBehaviour
    {
        [SerializeField] private Transform TargetTransform;
        [SerializeField] private EnemyAttackAnimation EnemyAttackAnimation;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                EnemyAttackAnimation.AttackTo(TargetTransform.position);
            }
        }
    }
}