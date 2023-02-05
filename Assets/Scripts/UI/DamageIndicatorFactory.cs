using Samples.Basic.Scripts;
using UnityEngine;
using UnityEngine.Pool;

namespace UI
{
    public class DamageIndicatorFactory : Singleton<DamageIndicatorFactory>
    {
        [SerializeField] private DamageIndicator DamageIndicatorPrefab;
        
        private readonly int _maxPoolSize = 50;
        private IObjectPool<DamageIndicator> _pool;
        
        public RectTransform ParentCanvas;

        private void Awake()
        {
            _pool = new ObjectPool<DamageIndicator>(CreatePooledItem, OnTakeFromPool, OnReturnToPool,
                OnDestroyPoolObject, true, 10, _maxPoolSize);
        }

        private DamageIndicator CreatePooledItem()
        {
            return Instantiate(DamageIndicatorPrefab);
        }

        private void OnTakeFromPool(DamageIndicator obj)
        {
            obj.OnTakeFromPool();
        }

        private void OnReturnToPool(DamageIndicator obj)
        {
            obj.gameObject.SetActive(false);
        }

        private void OnDestroyPoolObject(DamageIndicator obj)
        {
            Destroy(obj.gameObject);
        }

        public void Create(int damage, Transform targetTransform)
        {
            var damageIndicator = _pool.Get();
            damageIndicator.transform.SetParent(ParentCanvas);
            damageIndicator.Animate(damage.ToString(), targetTransform, ParentCanvas, (element) =>
            {
                _pool.Release(element);
            });
        }
    }
}