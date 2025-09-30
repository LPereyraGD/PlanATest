using UnityEngine;
using UnityEngine.UI;

namespace Game.Managers
{
    public class GridView : MonoBehaviour
    {
        public GridLayoutGroup gridLayoutGroup;
        public GameObject tilePrefab;
        public GameController gameController;

        private TileView[,] tileViews;
        private int gridWidth;
        private int gridHeight;
        
        private bool isInitialized = false;

        private void Start()
        {
            if (gameController == null)
            {
                Debug.LogError("GameController not assigned! Please assign it in the inspector.");
                return;
            }

            gameController.OnGridChanged += RefreshGrid;
        }

        private void OnDestroy()
        {
            if (gameController != null)
                gameController.OnGridChanged -= RefreshGrid;
        }

        private void SetupGrid()
        {
            if (gameController == null || gridLayoutGroup == null) 
            {
                Debug.LogError("Missing references!");
                return;
            }

            // Get grid dimensions from GameController instead of ServiceLocator
            if (gameController.GridConfig == null)
            {
                Debug.LogError("GameController has no grid config!");
                return;
            }

            gridWidth = gameController.GridConfig.Width;
            gridHeight = gameController.GridConfig.Height;

            // Configure GridLayoutGroup with the width from config
            // I tried to add a little extra making the grid adapt to the size of the config
            ConfigureGridLayout();

            ClearGrid();
            tileViews = new TileView[gridWidth, gridHeight];
            
            for (int y = 0; y < gridHeight; y++)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    CreateTile(x, y);
                }
            }
            
            isInitialized = true;
        }

        private void ConfigureGridLayout()
        {
            if (gridLayoutGroup == null) return;

            gridLayoutGroup.constraintCount = gridWidth;
        }

        private void CreateTile(int x, int y)
        {
            if (tilePrefab == null)
            {
                Debug.LogError("No tile prefab!");
                return;
            }

            var tileObj = Instantiate(tilePrefab, gridLayoutGroup.transform);
            tileObj.name = $"Tile_{x}_{y}";

            var tileView = tileObj.GetComponent<TileView>();
            if (tileView == null)
                tileView = tileObj.AddComponent<TileView>();

            tileView.Initialize(x, y, this);
            tileViews[x, y] = tileView;
            UpdateTile(x, y);
        }

        private void RefreshGrid()
        {
            if (!isInitialized)
            {
                SetupGrid();
                return;
            }

            // Just refresh existing tiles
            if (tileViews == null) return;

            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    UpdateTile(x, y);
                }
            }
        }

        private void UpdateTile(int x, int y)
        {
            if (tileViews == null || x < 0 || x >= gridWidth || y < 0 || y >= gridHeight)
                return;

            var tileView = tileViews[x, y];
            if (tileView == null) return;

            if (gameController != null)
            {
                var tileColor = gameController.GetTileColor(x, y);
                var isValid = gameController.IsValidPosition(x, y);
                tileView.UpdateVisual(tileColor, isValid);
            }
        }

        private void ClearGrid()
        {
            if (tileViews == null) return;

            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    if (tileViews[x, y] != null)
                        DestroyImmediate(tileViews[x, y].gameObject);
                }
            }

            tileViews = null;
            isInitialized = false;
        }

        public void OnTileClicked(int x, int y)
        {
            if (gameController != null)
                gameController.OnTileClicked(x, y);
        }
    }
}