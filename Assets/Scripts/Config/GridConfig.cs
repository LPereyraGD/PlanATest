using UnityEngine;

namespace Game.Config
{
    [CreateAssetMenu(fileName = "GridConfig", menuName = "Game/Grid Config")]
    public class GridConfig : ScriptableObject
    {
        [Header("Grid Settings")] public int width = 6;
        public int height = 5;
        public int initialMoves = 5;
        public int colorCount = 4;

        public int Width => width;
        public int Height => height;
        public int InitialMoves => initialMoves;
    }
}