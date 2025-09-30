using System.Collections.Generic;
using Game.Config;
using Game.Core;

namespace Game.Managers
{
    public class GridController
    {
        private int[,] grid;
        private int width;
        private int height;
        private int tileCount;
        private int? seed;
        
        // Random instance for this, we should make a service for the random
        private System.Random random;

        public int Width => width;
        public int Height => height;

        public void Initialize(GridConfig  config)
        {
            width = config.Width;
            height = config.Height;
            tileCount = config.TileCount;
            grid = new int[width, height];
            seed = config.seed;
            
            random = new System.Random(seed ?? System.DateTime.Now.Millisecond);
            FillGrid();
        }

        public int Get(int x, int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
                return -1;
                
            return grid[x, y];
        }

        public IReadOnlyList<Position> GetConnectedGroup(int x, int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
                return new List<Position>();

            int color = grid[x, y];
            if (color == -1)
                return new List<Position>();

            var visited = new bool[width, height];
            var group = new List<Position>();
            var queue = new Queue<Position>();
            
            queue.Enqueue(new Position(x, y));
            visited[x, y] = true;

            while (queue.Count > 0)
            {
                var pos = queue.Dequeue();
                group.Add(pos);

                var neighbors = new[]
                {
                    new Position(pos.x - 1, pos.y),
                    new Position(pos.x + 1, pos.y),
                    new Position(pos.x, pos.y - 1),
                    new Position(pos.x, pos.y + 1)
                };

                foreach (var neighbor in neighbors)
                {
                    if (IsValidPosition(neighbor.x, neighbor.y) && 
                        !visited[neighbor.x, neighbor.y] && 
                        grid[neighbor.x, neighbor.y] == color)
                    {
                        visited[neighbor.x, neighbor.y] = true;
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return group;
        }

        public void Remove(IReadOnlyList<Position> cells)
        {
            foreach (var cell in cells)
            {
                if (cell.x >= 0 && cell.x < width && cell.y >= 0 && cell.y < height)
                {
                    grid[cell.x, cell.y] = -1;
                }
            }
        }

        public void CollapseColumns()
        {
            for (int x = 0; x < width; x++)
            {
                CollapseColumn(x);
            }
        }

        public void Refill()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (grid[x, y] == -1)
                        grid[x, y] = random.Next(0, tileCount);
                }
            }
        }

        public bool IsValidPosition(int x, int y)
        {
            return x >= 0 && x < width && y >= 0 && y < height;
        }
        private void FillGrid()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grid[x, y] = random.Next(0, tileCount);
                }
            }
        }

        private void CollapseColumn(int col)
        {
            var values = new List<int>();
            
            for (int y = 0; y < height; y++)
            {
                if (grid[col, y] != -1)
                    values.Add(grid[col, y]);
            }

            for (int y = 0; y < height; y++)
                grid[col, y] = -1;

            for (int i = 0; i < values.Count; i++)
                grid[col, height - 1 - i] = values[values.Count - 1 - i];
        }
    }
}