using System;
using DG.Tweening;
using Gameplay;
using Samples.Basic.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [Serializable]
    public class ScreenData
    {
        public CanvasGroup canvasGroup;
        public GraphicRaycaster raycaster;
    }
    
    public class ScreenManager : Singleton<ScreenManager>
    {
        [SerializeField] private ScreenData splashScreen;
        [SerializeField] private ScreenData mainScreen;
        [SerializeField] private ScreenData creditScreen;
        [SerializeField] private ScreenData storyScreen;

        private void Start()
        {
            AudioManager.Instance.Play("Menu SFX");
        }

        public void NavigateSplashToMain()
        {
            splashScreen.raycaster.enabled = false;
            splashScreen.canvasGroup.DOFade(0F, .5F).SetEase(Ease.OutExpo);
            mainScreen.raycaster.enabled = true;
            mainScreen.canvasGroup.DOFade(1F, .5F).SetEase(Ease.OutExpo);
        }
        
        public void NavigateMainToCredits()
        {
            mainScreen.raycaster.enabled = false;
            mainScreen.canvasGroup.DOFade(0F, .5F).SetEase(Ease.OutExpo);
            creditScreen.raycaster.enabled = true;
            creditScreen.canvasGroup.DOFade(1F, .5F).SetEase(Ease.OutExpo);
        }
        
        public void NavigateCreditsToMain()
        {
            creditScreen.raycaster.enabled = false;
            creditScreen.canvasGroup.DOFade(0F, .5F).SetEase(Ease.OutExpo);
            mainScreen.raycaster.enabled = true;
            mainScreen.canvasGroup.DOFade(1F, .5F).SetEase(Ease.OutExpo);
        }

        public void NavigateMainToStory()
        {
            AudioManager.Instance.Stop("Menu SFX");
            AudioManager.Instance.Play("Story SFX");
            mainScreen.raycaster.enabled = false;
            mainScreen.canvasGroup.DOFade(0F, .5F).SetEase(Ease.OutExpo);
            storyScreen.raycaster.enabled = true;
            storyScreen.canvasGroup.DOFade(1F, .5F).SetEase(Ease.OutExpo);
            storyScreen.canvasGroup.transform.GetComponent<StoryCanvasManager>().StartStory();
        }

        public void Quit()
        {
            #if !UNITY_WEBGL
            Application.Quit();
            #endif
        }
    }
}