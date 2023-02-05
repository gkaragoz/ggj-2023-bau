using System;
using System.Collections;
using Main_Character;
using Samples.Basic.Scripts;
using UI;
using UnityEngine;

namespace Gameplay
{
    public class TimerManager : Singleton<TimerManager>
    {
        public int PassingSeconds { get; private set; }
        public string TimeText { get; private set; }

        public static Action<int> OnTimeChange;
        public static Action<string> OnTimeChangeAsFormattedString;
        
        private bool _timerIsRunning;
        private Coroutine _coroutine;

        private void StartTimer()
        {
            _timerIsRunning = true;
            _coroutine = StartCoroutine(Timer());
        }
        
        private void StopTimer()
        {
            _timerIsRunning = false;
            StopCoroutine(_coroutine);
        }

        private IEnumerator Timer()
        {
            while (_timerIsRunning)
            {
                yield return new WaitForSeconds(1F);
                PassingSeconds++;
                CountTime(PassingSeconds);

                if (PassingSeconds % 2 == 0)
                {
                    MainCharacterController.Instance.AddScore(1);
                }
            }
        }
        
        private void CountTime(float timeToDisplay)
        {
            float hours = Mathf.FloorToInt(timeToDisplay / 3600);
            float minutes = Mathf.FloorToInt(timeToDisplay / 60) % 60;
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);
            TimeText = $"{hours:00}:{minutes:00}:{seconds:00}";
            OnTimeChange?.Invoke(PassingSeconds);
            OnTimeChangeAsFormattedString?.Invoke(TimeText);
        }

        private void OnEnable()
        {
            GameManager.OnStart += StartTimer;
            GameManager.OnPause += StopTimer;
            GameManager.OnComplete += StopTimer;
        }

        private void OnDisable()
        {
            GameManager.OnStart -= StartTimer;
            GameManager.OnPause -= StopTimer;
            GameManager.OnComplete -= StopTimer;
        }
    }
}