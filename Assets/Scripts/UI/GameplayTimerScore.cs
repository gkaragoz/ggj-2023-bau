using Gameplay;
using Samples.Basic.Scripts;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GameplayTimerScore : Singleton<GameplayTimerScore>
    {
        [SerializeField] private TextMeshProUGUI ScoreText;
        [SerializeField] private TextMeshProUGUI TimerText;
        
        private void OnEnable()
        {
            GameManager.OnStart += OnGameStarted;
            TimerManager.OnTimeChangeAsFormattedString += OnTimeChangeAsFormattedString;
        }

        private void OnDisable()
        {
            GameManager.OnStart -= OnGameStarted;
            TimerManager.OnTimeChangeAsFormattedString -= OnTimeChangeAsFormattedString;
        }

        public void SetScore(int score)
        {
            ScoreText.text = $"SCORE: <color=#A0D235>{score}</color>";
        }
        
        private void SetTimerText(string timerText)
        {
            TimerText.text = timerText;
        }

        private void OnTimeChangeAsFormattedString(string formattedTime)
        {
            SetTimerText(formattedTime);
        }

        private void OnGameStarted()
        {
            SetScore(0);
            SetTimerText("00:00:00");
        }
    }
}