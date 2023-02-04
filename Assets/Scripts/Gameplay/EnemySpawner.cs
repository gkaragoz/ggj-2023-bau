using Enemy;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Gameplay
{
    [Serializable]
    public class SpawnData
    {
        public string waveName;
        public AnimationCurve difficultyCurve;
        public List<SpriteRenderer> spawnAreas;
        public List<EnemyController> enemies;

        public float Duration()
        {
            var keyCount = difficultyCurve.keys.Length;
            return difficultyCurve.keys[keyCount - 1].time;
        }

        public EnemyController RandomEnemy()
        {
            return enemies[Random.Range(0, enemies.Count - 1)];
        }
    }
    
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private List<SpawnData> waves;

        private bool _shouldSpawn;
        private Coroutine _coroutine;
        private float _interval;

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
                var waveData = GetSpawnData(out var remainedTime);
                if (waveData == null) yield break;
                var keyCount = waveData.difficultyCurve.keys.Length;
                var waveAlpha = remainedTime / waveData.Duration();
                var firstKey = waveData.difficultyCurve.keys[0].value;
                var lastKey = waveData.difficultyCurve.keys[keyCount - 1].value;
                var spawnRate = Mathf.Lerp(firstKey, lastKey, waveAlpha);
                yield return new WaitForSeconds(spawnRate);
                var enemy = Instantiate(waveData.RandomEnemy(), null);
                enemy.transform.position = GetRandomSpawnPosition(waveData);
            }
            
            Debug.LogWarning("All wave completed!");
        }

        private SpawnData GetSpawnData(out int remainedTime)
        {
            var passingTime = TimerManager.Instance.PassingSeconds;
            foreach (var t in waves)
            {
                if (passingTime <= t.Duration())
                {
                    remainedTime = passingTime;
                    return t;
                }
                
                passingTime -= (int)t.Duration();
                passingTime = passingTime < 0 ? 0 : passingTime;
            }
            
            remainedTime = passingTime;
            return null;
        }
        
        private Vector2 GetRandomSpawnPosition(SpawnData spawnData)
        {
            var randomSpawnArea = spawnData.spawnAreas[Random.Range(0, spawnData.spawnAreas.Count)];
            var bounds = randomSpawnArea.bounds;
            var rectPos = randomSpawnArea.transform.position;
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