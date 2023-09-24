using UnityEngine;

public class CellularAutomaton : MonoBehaviour
{
    private int _gridSizeX;
    private int _gridSizeY;
    private int _randomFillPercent;
    private int _numIterations;

    private int[,] _grid;

    public int[,] GenerateMap(int width, int height, int randomFillPercent, int numIterations)
    {
        _gridSizeX = width;
        _gridSizeY = height;
        _randomFillPercent = randomFillPercent;
        _numIterations = numIterations;
        _grid = new int[width, height];
        RandomFillGrid();
        SmoothGrid();
        return _grid;
    }
    

    private void RandomFillGrid() {
        var seed = Time.time.ToString();
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());
        for (int x = 0; x < _gridSizeX; x ++) {
            for (int y = 0; y < _gridSizeY; y ++) {
                if (x == 0 || x == _gridSizeX-1 || y == 0 || y == _gridSizeY -1) {
                    _grid[x,y] = 1;
                }
                else {
                    _grid[x,y] = (pseudoRandom.Next(0,100) < _randomFillPercent)? 1: 0;
                }
            }
        }
    }

    private void SmoothGrid()
    {
        for (int iteration = 0; iteration < _numIterations; iteration++)
        {
            int[,] newGrid = new int[_gridSizeX, _gridSizeY];

            for (int x = 0; x < _gridSizeX; x++)
            {
                for (int y = 0; y < _gridSizeY; y++)
                {
                    newGrid[x, y] = ApplyCellularAutomaton(x, y);
                }
            }

            _grid = newGrid;
        }
    }

    private int ApplyCellularAutomaton(int x, int y)
    {
        int countNeighbors = CountNeighbors(x, y);
        return countNeighbors >= 5 ? 1 : 0;
    }

    private int CountNeighbors(int x, int y)
    {
        int count = 0;

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                int neighborX = x + dx;
                int neighborY = y + dy;

                if (neighborX >= 0 && neighborX < _gridSizeX && neighborY >= 0 && neighborY < _gridSizeY)
                {
                    count += _grid[neighborX, neighborY];
                }
            }
        }

        count -= _grid[x, y];

        return count;
    }
        
    //void OnDrawGizmos() {
    //    if (_grid != null) {
    //        for (int x = 0; x < gridSizeX; x ++) {
    //            for (int y = 0; y < gridSizeY; y ++) {
    //                Gizmos.color = (_grid[x,y] == 1)?Color.black:Color.white;
    //                Vector3 pos = new Vector3(-gridSizeX/2 + x + .5f,0, -gridSizeY/2 + y+.5f);
    //                Gizmos.DrawCube(pos,Vector3.one);
    //            }
    //        }
    //    }
    //}

}
