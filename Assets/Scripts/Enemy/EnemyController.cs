using Main_Character;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private Vector2 speedInterval;
        
        private Transform _target;
        private NavMeshAgent _agent;

        private void Awake()
        {
            _target = MainCharacterController.Instance.transform;
            _agent = GetComponent<NavMeshAgent>();
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
            _agent.speed = Random.Range(speedInterval.x, speedInterval.y);
            _agent.acceleration = _agent.speed * 3;
        }

        private void Update()
        {
            var targetPosition = _target.position;
            _agent.SetDestination(new Vector3(targetPosition.x, targetPosition.y, transform.position.z));
        }
    }
}