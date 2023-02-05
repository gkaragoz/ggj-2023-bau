using Main_Character;
using Samples.Basic.Scripts;
using UnityEngine;
using UnityEngine.Pool;

namespace UI
{
    public class ScoreIndicatorFactory : Singleton<ScoreIndicatorFactory>
    {
        [SerializeField] private ScoreIndicator ScoreIndicatorPrefab;
        
        private readonly int _maxPoolSize = 50;
        private IObjectPool<ScoreIndicator> _pool;
        
        public RectTransform StartRectTransform;
        public RectTransform EndRectTransform;

        private void Awake()
        {
            _pool = new ObjectPool<ScoreIndicator>(CreatePooledItem, OnTakeFromPool, OnReturnToPool,
                OnDestroyPoolObject, true, 10, _maxPoolSize);
        }

        private ScoreIndicator CreatePooledItem()
        {
            return Instantiate(ScoreIndicatorPrefab);
        }

        private void OnTakeFromPool(ScoreIndicator obj)
        {
            obj.OnTakeFromPool();
        }

        private void OnReturnToPool(ScoreIndicator obj)
        {
            obj.transform.localScale = Vector3.one;
            obj.gameObject.SetActive(false);
        }

        private void OnDestroyPoolObject(ScoreIndicator obj)
        {
            Destroy(obj.gameObject);
        }

        // private void Update()
        // {
        //     if (Input.GetKeyDown(KeyCode.KeypadPlus))
        //     {
        //         Create(10);
        //     }
        // }

        public void Create(int score)
        {
            var damageIndicator = _pool.Get();
            damageIndicator.transform.SetParent(StartRectTransform);
            damageIndicator.Animate(score, EndRectTransform, StartRectTransform, (element) =>
            {
                _pool.Release(element);
                GameplayTimerScore.Instance.SetScore(MainCharacterController.Instance.Score);
            });
        }
    }
}