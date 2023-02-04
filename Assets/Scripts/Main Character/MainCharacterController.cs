using System;
using Enemy;
using Gameplay;
using Samples.Basic.Scripts;
using UI;
using UnityEngine;

namespace Main_Character
{
    public class MainCharacterController : Singleton<MainCharacterController>
    {
        [SerializeField] private float health;
        [SerializeField] private float speed;
        [SerializeField] private Rigidbody2D rigidbody2d;
        [SerializeField] private CharacterSpriteSymmetry characterSpriteSymmetry;
        [SerializeField] private HealthBar healthBar;

        public static Action<float> OnReceiveHit;
        public static Action OnDeath;
        
        private Vector3 _refVel = Vector3.zero;
        private float _currentHealth;
        private bool _isDead;

        private void Awake()
        {
            _currentHealth = health;
            healthBar.SetHealth(health, health);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if(_isDead) return;
            
            if (col.TryGetComponent(out EnemyController enemy))
            {
                _currentHealth = Mathf.Clamp(_currentHealth - enemy.DamageAmount, 0F, health);

                if (_currentHealth == 0)
                {
                    GameManager.OnComplete?.Invoke();
                    OnDeath?.Invoke();
                    _isDead = true;
                    return;
                }
                
                OnReceiveHit?.Invoke(enemy.DamageAmount);
                healthBar.TakeDamage(col.transform.position, _currentHealth, health);
            }
        }

        private void Update()
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");
            var direction = new Vector2(horizontal, vertical);
            Vector2 targetVelocity;
            
            if (Mathf.Abs(horizontal) >= .95F && Mathf.Abs(vertical) >= .95F)
            {
                targetVelocity = direction * speed / 1.5F;
            }

            else
            {
                targetVelocity = direction * speed;
            }

            rigidbody2d.velocity = Vector3.SmoothDamp(rigidbody2d.velocity, targetVelocity, ref _refVel, .05F);
        }
    }
}
