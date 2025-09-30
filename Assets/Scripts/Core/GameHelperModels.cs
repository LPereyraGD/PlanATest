using System;
using UnityEngine;

namespace Game.Core
{
    public static class ScreenIds
    {
        public static string GAME_OVER_SCREEN = "GAME_OVER_SCREEN";
    }
    [Serializable]
    public struct ScreenEntry
    {
        public string key;
        public GameObject prefab;
    }
    [Serializable]
    public struct GameState
    {
        public int score;
        public int moves;
        public bool isGameOver;
  
        public GameState(int score, int moves, bool isGameOver = false)
        {
            this.score = score;
            this.moves = moves;
            this.isGameOver = isGameOver;
        }
    }
}
