using System;
using Samples.Basic.Scripts;
using UnityEngine.SceneManagement;

namespace Gameplay
{
    public class GameManager : Singleton<GameManager>
    {
        public static Action OnStart;
        public static Action OnPause;
        public static Action OnComplete;
        
        private void Start()
        {
            OnStart?.Invoke();
            AudioManager.Instance.Play("Gameplay SFX");
        }

        public void Restart()
        {
            SceneManager.LoadScene("Gameplay");
        }

        public void Menu()
        {
            AudioManager.Instance.Stop("Gameplay SFX");
            SceneManager.LoadScene("Main");
        }
    }
}