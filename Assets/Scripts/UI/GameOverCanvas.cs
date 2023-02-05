using System;
using System.Collections;
using DG.Tweening;
using Gameplay;
using Main_Character;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameOverCanvas : MonoBehaviour
    {
        [SerializeField] private RectTransform ParentRectTransform;
        [SerializeField] private CanvasGroup CanvasGroup;
        [SerializeField] private RectTransform MaskRectTransform;
        
        [Header("Elements")]
        [SerializeField] private CanvasGroup MenuCanvasGroup;
        [SerializeField] private TextMeshProUGUI MessageText;
        [SerializeField] private TextMeshProUGUI ScoreText;
        [SerializeField] private TextMeshProUGUI TimeAliveText;

        [SerializeField] private RectTransform FoodsTargetRect;
        [SerializeField] private CanvasGroup KidCanvasGroup;
        [SerializeField] private RectTransform KidHappyFace;
        [SerializeField] private RectTransform KidSadFace;
        [SerializeField] private RectTransform[] FoodsToFall;
        [SerializeField] private RectTransform FullSoup;
        [SerializeField] private RectTransform FullHotSoup;
        [SerializeField] private RectTransform HalfSoup;
        [SerializeField] private RectTransform EmptySoup;

        private Camera _camera;
        
        private void Awake()
        {
            _camera = Camera.main;
            CanvasGroup.alpha = 0;
        }

        private void OnEnable()
        {
            GameManager.OnComplete += OnGameOver;
        }

        private void OnDisable()
        {
            GameManager.OnComplete -= OnGameOver;
        }

        private void OnGameOver()
        {
            var targetPosition = MainCharacterController.Instance.transform.position;
            var parentCanvasSizeDelta = ParentRectTransform.sizeDelta;
            var viewportPosition = _camera.WorldToViewportPoint(targetPosition);
            var worldObjectScreenPosition = new Vector2(
                viewportPosition.x * parentCanvasSizeDelta.x - parentCanvasSizeDelta.x * 0.5f,
                viewportPosition.y * parentCanvasSizeDelta.y - parentCanvasSizeDelta.y * 0.5f);

            MaskRectTransform.anchoredPosition = worldObjectScreenPosition + (Vector2.up * 50);
            
            CanvasGroup.DOFade(1, 0.25f)
                .SetDelay(0.5f)
                .OnComplete(StartSequence);
        }

        private void StartSequence()
        {
            GetComponent<GraphicRaycaster>().enabled = true;
            MenuCanvasGroup.alpha = 0;
            SetScoreObjects();

            KidCanvasGroup.DOFade(1, 0.5f)
                .OnComplete(() =>
                {
                    ShowPrepareMeal(() =>
                    {
                        KidCanvasGroup.transform.DOScale(Vector3.one * 0.5f, 0.5f)
                            .OnComplete(() =>
                            {
                                MenuCanvasGroup.DOFade(1, 0.25f);
                            });
                    });
                });
        }

        private void SetScoreObjects()
        {
            var score = MainCharacterController.Instance.Score;

            EmptySoup.gameObject.SetActive(false);
            FullSoup.gameObject.SetActive(false);
            FullHotSoup.gameObject.SetActive(false);
            HalfSoup.gameObject.SetActive(false);
            
            KidSadFace.gameObject.SetActive(false);
            KidHappyFace.gameObject.SetActive(false);
            
            if (score <= 25)
            {
                MessageText.text = "YOU.. BASICALLY JUST FAINTED";
                KidSadFace.gameObject.SetActive(true);
                EmptySoup.gameObject.SetActive(true);
            }
            else if (score <= 50)
            {
                MessageText.text = "BUT MADE THE SOUP..KINDA?";
                KidHappyFace.gameObject.SetActive(true);
                HalfSoup.gameObject.SetActive(true);
            }
            else
            {
                MessageText.text = "BUT MADE THE SOUP...YAY?";
                KidHappyFace.gameObject.SetActive(true);
                FullHotSoup.gameObject.SetActive(true);
            }
            
            ScoreText.text = score.ToString();
            TimeAliveText.text = TimerManager.Instance.TimeText;
            
            KidCanvasGroup.gameObject.SetActive(true);
        }

        private void ShowPrepareMeal(Action onCompleted)
        {
            StartCoroutine(Do());
            
            IEnumerator Do()
            {
                foreach (var foodsToFall in FoodsToFall)
                {
                    foodsToFall.DOMove(FoodsTargetRect.transform.position, 0.85f)
                        .OnComplete(() =>
                        {
                            foodsToFall.gameObject.SetActive(false);
                        });
                    
                    yield return new WaitForSeconds(0.85f);
                }
                
                onCompleted?.Invoke();
            }
        }
    }
}