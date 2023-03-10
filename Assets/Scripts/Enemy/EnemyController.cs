using System;
using System.Collections;
using Animations;
using Gameplay;
using Main_Character;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Enemy
{
    public enum EnemyState
    {
        Walk,
        Attack,
        Death,
        GetHit,
        Stop
    }
    
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private Vector2 speedInterval;
        [SerializeField] private float damageAmount;
        [SerializeField] private float attackCooldown;
        [SerializeField] private int health;
        [SerializeField] private int reward;
        
        private Transform _current;
        private Transform _target;
        private NavMeshAgent _agent;
        private DieAnimation _dieAnimation;
        private EnemyAttackAnimation _attackAnimation;
        private TakeHitAnimation _hitAnimation;
        private bool _canAttack = true;
        private YieldInstruction _attackCooldown;
        private float _health;
        private bool _isAttackable = true;

        public int Reward => reward;
        public float DamageAmount => damageAmount;
        public EnemyState CurrentState { get; private set; } = EnemyState.Walk;

        private void Awake()
        {
            _current = transform;
            _attackAnimation = GetComponent<EnemyAttackAnimation>();
            _hitAnimation = GetComponent<TakeHitAnimation>();
            _dieAnimation = GetComponent<DieAnimation>();
            _target = MainCharacterController.Instance.transform;
            _agent = GetComponent<NavMeshAgent>();
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
            _agent.speed = Random.Range(speedInterval.x, speedInterval.y);
            _agent.acceleration = _agent.speed * 3;
            _health = health;
            var speedAlpha = _agent.speed / speedInterval.y;
            transform.localScale = Vector3.one * Mathf.Lerp(1.6F, 1F, speedAlpha);
            _attackCooldown = new WaitForSeconds(Random.Range(attackCooldown / 2F, attackCooldown));
        }

        private void Update()
        {
            switch (CurrentState)
            {
                case EnemyState.Death or EnemyState.Attack or EnemyState.GetHit or EnemyState.Stop:
                    break;
                case EnemyState.Walk:
                {
                    if (Vector3.Distance(transform.position, _target.position) > _agent.stoppingDistance * 2F || !_canAttack)
                    {
                        var targetPosition = _target.position;
                        _agent.SetDestination(new Vector3(targetPosition.x, targetPosition.y, 0F));
                        CurrentState = EnemyState.Walk;
                    }

                    else
                    {
                        CurrentState = EnemyState.Attack;
                        _agent.isStopped = true;
                        _attackAnimation.AttackTo(_target.position, () =>
                        {
                            if (_agent.isOnNavMesh && _agent.enabled)
                            {
                                _agent.isStopped = false;
                                CurrentState = EnemyState.Walk;
                            }
                        });
                        
                        StartCoroutine(AttackCooldown());
                    }
                    break;
                }
            }

            var position = _current.position;
            position.z = 0;
            _current.position = position;
        }

        private IEnumerator AttackCooldown()
        {
            _canAttack = false;
            yield return _attackCooldown;
            _canAttack = true;
        }

        public void TakeHit(Vector3 hitPoint, float damage = 0, bool shouldAddScore = true)
        {
            if (_health == 0) return;
            if (_isAttackable == false) return;

            damage = damage == 0 ? Random.Range(15, 25) : damage;
            _health = Mathf.Clamp(_health - damage, 0F, health);
            CurrentState = EnemyState.GetHit;

            _isAttackable = false;
            _attackAnimation.Clear(true);
            AudioManager.Instance.Play($"Hit Type{Random.Range(1, 4)}");
            _hitAnimation.TakeHit((int)damage, hitPoint, () =>
            {
                _isAttackable = _health != 0;;
                CurrentState = EnemyState.Walk;

            }, null, true);
            
            if (_health == 0)
            {
                if (shouldAddScore)
                {
                    MainCharacterController.Instance.AddScore(reward);
                }
                _hitAnimation.Clear();;
                _dieAnimation.Die(Destroy);
                CurrentState = EnemyState.Death;
            }
        }

        private void StopEnemy()
        {
            TakeHit(transform.position, health, false);
        }
        
        private void OnEnable()
        {
            GameManager.OnComplete += StopEnemy;
        }

        private void OnDisable()
        {
            GameManager.OnComplete -= StopEnemy;
        }
    }
}