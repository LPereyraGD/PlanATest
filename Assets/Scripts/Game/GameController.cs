using System;
using Game.Config;
using Game.Core;
using Game.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Managers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private GridConfig gridConfig;
        [SerializeField] private Button makeMoveButton;
        
        public event Action<int> OnScoreChanged;
        public event Action<int> OnMovesChanged;
        public event Action OnGameOver;
        
        private GameState currentGameState;

        private void Awake()
        {
            if (makeMoveButton != null)
            {
                makeMoveButton.onClick.AddListener(MakeMove);
            }
        }
        private void Start()
        {
            StartGame();
        }

        private void StartGame()
        {
            currentGameState = new GameState(0, gridConfig.InitialMoves);
            
            OnScoreChanged?.Invoke(currentGameState.score);
            OnMovesChanged?.Invoke(currentGameState.moves);
        }

        private void MakeMove()
        {
            currentGameState.moves--;
            OnMovesChanged?.Invoke(currentGameState.moves);

            
            //fixed score 10
            currentGameState.score += 10;
            OnScoreChanged?.Invoke(currentGameState.score);
            
            if (currentGameState.moves > 0) 
                return;
            
            currentGameState.isGameOver = true;
            OnGameOver?.Invoke();
        }

        public void RestartGame()
        {
            StartGame();
        }
    }
}