using System;
using System.Collections;
using System.Collections.Generic;
using Game.Config;
using Game.Core;
using UnityEngine;

namespace Game.Managers
{
    // There is a lot of references to the game controller. There is better ways to solve this.
    // But for lack of time, I prioritize the functionality over the perfect architecture.
    public class GameController : MonoBehaviour
    {
        [SerializeField] private GridConfig gridConfig;
        
        public GridConfig GridConfig => gridConfig;
        public event Action<int> OnScoreChanged;
        public event Action<int> OnMovesChanged;
        public event Action OnGameOver;
        public event Action OnGridChanged;
        
        private GameState currentGameState;
        private bool isResolving;

        private GridController gridService;
        private void Awake()
        {
            gridService = new GridController();
        }
        private void Start()
        {
            StartGame();
        }

        private void StartGame()
        { 
            gridService.Initialize(gridConfig);
            currentGameState = new GameState(0, gridConfig.InitialMoves);
            
            OnScoreChanged?.Invoke(currentGameState.score);
            OnMovesChanged?.Invoke(currentGameState.moves);
            OnGridChanged?.Invoke();
        }

        public void OnTileClicked(int x, int y)
        {
            if (!currentGameState.CanMakeMove || isResolving)
                return;

            var group = gridService.GetConnectedGroup(x, y);
            if (group.Count < 2)
                return;

            ProcessGroup(group);
        }
        
        private void ProcessGroup(IReadOnlyList<Position> group)
        {
            isResolving = true;
            currentGameState.isResolving = true;

            int score = group.Count;
            currentGameState.score += score;
            OnScoreChanged?.Invoke(currentGameState.score);

            currentGameState.moves--;
            OnMovesChanged?.Invoke(currentGameState.moves);

            if (currentGameState.moves <= 0)
            {
                currentGameState.isGameOver = true;
                isResolving = false;
                OnGameOver?.Invoke();
                return;
            }

            StartCoroutine(ResolveGroup(group));
        }

        private IEnumerator ResolveGroup(IReadOnlyList<Position> group)
        {
            gridService.Remove(group);
            OnGridChanged?.Invoke();

            yield return new WaitForSeconds(1f); // TODO: make this configurable

            gridService.CollapseColumns();
            OnGridChanged?.Invoke();

            gridService.Refill();
            OnGridChanged?.Invoke();

            isResolving = false;
            currentGameState.isResolving = false;
        }

        public void RestartGame()
        {
            StopAllCoroutines();
            StartGame();
        }
        public Sprite GetTileColor(int x, int y)
        {
            var colorIndex = gridService.Get(x, y);
            return gridConfig.GetColor(colorIndex);
        }

        public bool IsValidPosition(int x, int y)
        {
            return gridService.IsValidPosition(x, y);
        }
    }
}