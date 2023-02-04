using System.Collections;
using Animations;
using DG.Tweening;
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
        private bool _canAttack = true;
        private YieldInstruction _attackCooldown;
        private int _health;
        private bool _isAttackable = true;

        public int Reward => reward;
        public float DamageAmount => damageAmount;
        public EnemyState CurrentState { get; private set; } = EnemyState.Walk;

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

        private void Update()
        {
            switch (CurrentState)
            {
                case EnemyState.Death or EnemyState.Attack:
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
                            _agent.isStopped = false;
                            CurrentState = EnemyState.Walk;
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

        public void TakeHit(Vector3 hitPoint)
        {
            if (_health == 0) return;
            if (_isAttackable == false) return;
            _isAttackable = false;
            DOTween.Kill(transform);
            CurrentState = EnemyState.Walk;
            _health--;
            
            if (_health == 0)
            {
                hitAnimation.Clear();;
                dieAnimation.Die(Destroy);
                CurrentState = EnemyState.Death;
                return;
            }

            hitAnimation.TakeHit(hitPoint, () =>
            {
                _isAttackable = true;
            }, null, true);
        }
    }
}