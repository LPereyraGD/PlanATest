using UnityEngine;

namespace Game.Config
{
    [CreateAssetMenu(fileName = "GridConfig", menuName = "Game/GridController Config")]
    public class GridConfig : ScriptableObject
    {
        [Header("GridController Settings")] public int width = 6;
        public int height = 5;
        public int initialMoves = 5;
        public int? seed = null;

        public int Width => width;
        public int Height => height;
        public int InitialMoves => initialMoves;
        
        public int TileCount => tileColor.Length;
        
        [Header("Tile Sprites")]
        public Sprite[] tileColor = new Sprite[]{};

        public Sprite GetColor(int colorIndex)
        {
            if (colorIndex < 0 || colorIndex >= tileColor.Length)
                return null;
                
            return tileColor[colorIndex];
        }
    }
}