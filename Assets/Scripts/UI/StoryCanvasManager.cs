using System;
using DG.Tweening;
using Gameplay;
using Samples.Basic.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class StoryCanvasManager : Singleton<StoryCanvasManager>
    {
        [SerializeField] private CanvasGroup SelfCanvasGroup;
        [SerializeField] private CanvasGroup[] StoryCanvasGroups;
        [SerializeField] private CanvasGroup ClickToContinueCanvasGroup;
        [SerializeField] private Button NextStoryButton;
        [SerializeField] private bool DebugStartOnAwake;
        
        private int _currentCanvasGroupIndex = 0;
        private bool _storyCompleted = false;

        private Tween _hideTween;
        private Tween _openTween;
        private Tween _openNextStoryButtonTween;
        
        public Action OnStoryCompleted { get; set; }

        private void Awake()
        {
            if (DebugStartOnAwake)
                StartStory();
        }

        public void StartStory()
        {
            _currentCanvasGroupIndex = 0;
            _storyCompleted = false;

            foreach (var storyCanvasGroup in StoryCanvasGroups)
            {
                storyCanvasGroup.alpha = 0;
            }
            
            SelfCanvasGroup.alpha = 1;
            NextStoryButton.interactable = true;
            
            OpenCanvasGroup();
        }

        public void OnClick_NextStory()
        {
            if (_storyCompleted) return;
            
            HideCanvasGroup();
            
            if (_currentCanvasGroupIndex >= StoryCanvasGroups.Length - 1)
            {
                NextStoryButton.interactable = false;
                _storyCompleted = true;
                SelfCanvasGroup.alpha = 0;
                
                OnStoryCompleted?.Invoke();
                SceneManager.LoadScene("Gameplay");
                AudioManager.Instance.Stop("Story SFX");
                AudioManager.Instance.Stop("Menu SFX");
            }
            else
            {
                _currentCanvasGroupIndex++;
                OpenCanvasGroup();   
            }
        }

        private void OpenCanvasGroup()
        {
            _openTween?.Kill(true);
            _openTween = StoryCanvasGroups[_currentCanvasGroupIndex].DOFade(1f, 1f)
                .OnComplete(() =>
                {
                    StoryCanvasGroups[_currentCanvasGroupIndex].alpha = 1;
            
                    _openNextStoryButtonTween?.Kill();
                    _openNextStoryButtonTween = ClickToContinueCanvasGroup.DOFade(1f, 1f)
                        .SetDelay(3f);
                });
        }

        private void HideCanvasGroup()
        {
            _hideTween?.Kill(true);
            _hideTween = StoryCanvasGroups[_currentCanvasGroupIndex].DOFade(0f, 1f)
                .OnComplete(() =>
                {
                    StoryCanvasGroups[_currentCanvasGroupIndex].alpha = 0;
                });
            
            _openNextStoryButtonTween?.Kill(true);
            _openNextStoryButtonTween = ClickToContinueCanvasGroup.DOFade(0f, 0.25f)
                .OnComplete(() =>
                {
                    ClickToContinueCanvasGroup.alpha = 0;
                });
        }
    }
}