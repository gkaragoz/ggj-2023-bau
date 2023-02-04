using System.Collections;
using Main_Character;
using UnityEngine;
using UnityEngine.AI;

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
        [SerializeField] private float attackCooldown;
        [SerializeField] private Vector2 speedInterval;
        
        private Transform _target;
        private NavMeshAgent _agent;
        private EnemyAttackAnimation _attackAnimation;
        private EnemyState _currentState = EnemyState.Walk;
        private bool _canAttack = true;
        private YieldInstruction _attackCooldown;

        private void Awake()
        {
            _attackAnimation = GetComponent<EnemyAttackAnimation>();
            _target = MainCharacterController.Instance.transform;
            _agent = GetComponent<NavMeshAgent>();
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
            _agent.speed = Random.Range(speedInterval.x, speedInterval.y);
            _agent.acceleration = _agent.speed * 3;
            _attackCooldown = new WaitForSeconds(Random.Range(attackCooldown / 2F, attackCooldown));
        }

        private void Update()
        {
            switch (_currentState)
            {
                case EnemyState.Death or EnemyState.Attack:
                    return;
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
                        _agent.enabled = false;
                        _attackAnimation.AttackTo(_target.position, () =>
                        {
                            _agent.enabled = true;
                            _currentState = EnemyState.Walk;
                        });
                        
                        StartCoroutine(AttackCooldown());
                    }
                    return;
                }
            }
        }

        private IEnumerator AttackCooldown()
        {
            _canAttack = false;
            yield return _attackCooldown;
            _canAttack = true;
        }
    }
}