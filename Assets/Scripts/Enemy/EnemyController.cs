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
        Death
    }
    
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private TakeHitAnimation hitAnimation;
        [SerializeField] private DieAnimation dieAnimation;
        [SerializeField] private Vector2 speedInterval;
        [SerializeField] private float damageAmount;
        [SerializeField] private float attackCooldown;
        [SerializeField] private int hitLimit;
        [SerializeField] private int reward;
        
        private Transform _current;
        private Transform _target;
        private NavMeshAgent _agent;
        private EnemyAttackAnimation _attackAnimation;
        private EnemyState _currentState = EnemyState.Walk;
        private bool _canAttack = true;
        private YieldInstruction _attackCooldown;
        private int _health;

        public int Reward => reward;
        public float DamageAmount => _currentState == EnemyState.Attack ? damageAmount : damageAmount / 3F;
        
        private void Awake()
        {
            _current = transform;
            _attackAnimation = GetComponent<EnemyAttackAnimation>();
            _target = MainCharacterController.Instance.transform;
            _agent = GetComponent<NavMeshAgent>();
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
            _agent.speed = Random.Range(speedInterval.x, speedInterval.y);
            _agent.acceleration = _agent.speed * 3;
            _health = hitLimit;
            var speedAlpha = _agent.speed / speedInterval.y;
            transform.localScale = Vector3.one * Mathf.Lerp(1.6F, 1F, speedAlpha);
            _attackCooldown = new WaitForSeconds(Random.Range(attackCooldown / 2F, attackCooldown));
        }

        private void OnTriggerEnter2DHandler(Collider2D col)
        {
            if (col.TryGetComponent(out CharacterWeaponController playerWeapon))
            {
                _health--;
                
                if (_health == 0)
                {
                    dieAnimation.Die(Destroy);
                    
                    // TODO : Send score to ScoreManager
                    
                    return;
                }

                hitAnimation.TakeHit(playerWeapon.transform.position, null, null);
            }
        }

        private void Update()
        {
            switch (_currentState)
            {
                case EnemyState.Death or EnemyState.Attack:
                    break;
                case EnemyState.Walk:
                {
                    if (Vector3.Distance(transform.position, _target.position) > _agent.stoppingDistance * 2F || !_canAttack)
                    {
                        var targetPosition = _target.position;
                        _agent.SetDestination(new Vector3(targetPosition.x, targetPosition.y, 0F));
                        _currentState = EnemyState.Walk;
                    }

                    else
                    {
                        _currentState = EnemyState.Attack;
                        _agent.isStopped = true;
                        _attackAnimation.AttackTo(_target.position, () =>
                        {
                            _agent.isStopped = false;
                            _currentState = EnemyState.Walk;
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

        private void OnEnable()
        {
            TriggerTransmitter.
        }

        private void OnDisable()
        {
            throw new NotImplementedException();
        }
    }
}