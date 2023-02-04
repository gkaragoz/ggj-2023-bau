using UI;
using UnityEngine;

namespace Main_Character
{
    public class CharacterHealthTestAnimation : MonoBehaviour
    {
        [SerializeField] private float CurrentHealth = 100;
        [SerializeField] private float MaxHealth = 100;
        [SerializeField] private HealthBar HealthBar;

        private void Awake()
        {
            HealthBar.SetHealth(CurrentHealth, MaxHealth);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                CurrentHealth -= UnityEngine.Random.Range(5f, 25f);
                HealthBar.TakeDamage(10, (Vector2)transform.position + Vector2.right, CurrentHealth, MaxHealth);
            }
        }
    }
}