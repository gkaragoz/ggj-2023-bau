using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using Samples.Basic.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class TimerManager : Singleton<TimerManager>
    {
        
    }

    [Serializable]
    public class SpawnData
    {
        public string waveName;
        public float duration;
        public SpriteRenderer spawnArea;
        public AnimationCurve difficultyCurve;
        public List<EnemyController> enemies;
    }
    
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private List<SpawnData> waves;

        private bool _shouldSpawn;
        private Coroutine _coroutine;
        private float _interval;
        private int _spawnerIndex;

        private void StartSpawner()
        {
            _shouldSpawn = true;
            _coroutine = StartCoroutine(Spawner());
        }
        private void StopSpawner()
        {
            _shouldSpawn = false;
            StopCoroutine(_coroutine);
        }
        private IEnumerator Spawner()
        {
            while (_shouldSpawn)
            {
                // TODO : Change seconds
                yield return new WaitForSeconds(1F);
            }
        }

        private Vector2 GetRandomSpawnPosition(SpawnData spawnData)
        {
            var bounds = spawnData.spawnArea.bounds;
            var rectPos = spawnData. spawnArea.transform.position;
            var rectHeight = bounds.extents.y;
            var rectWidth = bounds.extents.x;
            var xPos = rectPos.x + Random.Range(-rectWidth, rectWidth);
            var yPos = rectPos.y + Random.Range(-rectHeight, rectHeight);
            return new Vector2(xPos, yPos);
        }

        private void OnEnable()
        {
            GameManager.OnStart += StartSpawner;
            GameManager.OnComplete += StopSpawner;
        }

        private void OnDisable()
        {
            GameManager.OnStart -= StartSpawner;
            GameManager.OnComplete -= StopSpawner;
        }
    }
}