using System.Collections.Generic;
using Game.Core;
using Game.Managers;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Game.Services
{
    public class Navigator : MonoBehaviour
    {
        [SerializeField] private GameController gameController;
        [SerializeField] private List<ScreenEntry> screens;
        [SerializeField] private Transform UICanvasGroup;
        
        private static Navigator instance;
        public static Navigator Instance
        {
            get { return instance; }
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            gameController.OnGameOver += () =>
            {
                ShowScreen(ScreenIds.GAME_OVER_SCREEN);
            };
        }

        public void ShowScreen(string screenName)
        {
            var screenToInstanciate = screens.Find(s => s.key == screenName).prefab;
            var go = Instantiate(screenToInstanciate, UICanvasGroup);
        }

        public void RestartGame()
        {
            gameController.RestartGame();
        }

        // use this a service to handle all the logic of showing screens
    }
}