using System;
using DG.Tweening;
using Enemy;
using Gameplay;
using Samples.Basic.Scripts;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Main_Character
{
    public enum CharacterState
    {
        Idle,
        Attack,
        Dash,
        Walk,
        Death
    }
    
    public class MainCharacterController : Singleton<MainCharacterController>
    {
        [SerializeField] private float health;
        [SerializeField] private float speed;
        [SerializeField] private float dashSpeed;
        [SerializeField] private float dashDistance;
        [SerializeField] private float dashCooldown;
        [SerializeField] private Rigidbody2D rigidbody2d;
        [SerializeField] private CharacterSpriteSymmetry characterSpriteSymmetry;
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private CharacterWeaponController weaponController;

        public int Score { get; private set; }
        public CharacterState CurrentState { get; private set; } = CharacterState.Idle;
        public static Action<float> OnReceiveHit;
        public static Action OnDeath;
        
        private Vector3 _refVel = Vector3.zero;
        private float _currentHealth;

        private void Awake()
        {
            _currentHealth = health;
            healthBar.SetHealth(health, health);
        }

        public void AddScore(int score)
        {
            Score += score;
            ScoreIndicatorFactory.Instance.Create(score);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if(CurrentState is CharacterState.Death or CharacterState.Dash) return;
            
            if (col.TryGetComponent(out EnemyController enemy))
            {
                if(enemy.CurrentState != EnemyState.Attack) return; 
                
                _currentHealth = Mathf.Clamp(_currentHealth - enemy.DamageAmount, 0F, health);
                AudioManager.Instance.Play($"Get Hit Type{Random.Range(1, 4)}");

                if (_currentHealth == 0)
                {
                    GameManager.OnComplete?.Invoke();
                    OnDeath?.Invoke();
                    CurrentState = CharacterState.Death;
                    return;
                }
                
                OnReceiveHit?.Invoke(enemy.DamageAmount);
                healthBar.TakeDamage((int)enemy.DamageAmount, col.transform.position, _currentHealth, health);
            }
        }

        private void HandleMovement()
        {
            if(!CanMove()) return;
            
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

        private void HandleDash()
        {
            if (CanDash())
            {
                if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    CurrentState = CharacterState.Dash;
                    var timer = 0F;
                    DOTween.To(() => timer, x => timer = x, 1F, 1F / dashSpeed)
                        .OnUpdate(()=> rigidbody2d.velocity += CharacterDirectionController.Instance.CurrentDirectionVector * dashDistance)
                        .SetEase(Ease.OutExpo)
                        .OnComplete(() => { DOVirtual.DelayedCall(dashCooldown, () => CurrentState = CharacterState.Idle); });
                }
            }
        }

        private bool CanDash()
        {
            return CurrentState != CharacterState.Death && 
                   CurrentState != CharacterState.Attack && 
                   CurrentState != CharacterState.Dash; 
        }

        private bool CanAttack()
        {
            return CurrentState != CharacterState.Death && 
                   CurrentState != CharacterState.Attack && 
                   CurrentState != CharacterState.Dash;
        }

        private bool CanMove()
        {
            return CurrentState != CharacterState.Death && 
                   CurrentState != CharacterState.Dash;
        }

        private void HandleAttack()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (CanAttack())
                {
                    weaponController.SwingWeapon();
                    CurrentState = CharacterState.Attack;
                }
            }
        }

        private void OnCompleteAttackAnimation()
        {
            CurrentState = CharacterState.Idle;
        }

        private void Update()
        {
            HandleMovement();
            HandleAttack();
            HandleDash();
        }

        private void OnEnable()
        {
            weaponController.OnCompleteAttackAnimation += OnCompleteAttackAnimation;
        }

        private void OnDisable()
        {
            weaponController.OnCompleteAttackAnimation -= OnCompleteAttackAnimation;
        }
    }
}
