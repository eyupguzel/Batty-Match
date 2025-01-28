using System;
using System.Collections;
using UnityEngine;

public class GridManager : Singleton<GridManager>
{
    [SerializeField] private int rows;
    [SerializeField] private int columns;
    [SerializeField] private Vector2 tileSize;
    [SerializeField] private Vector2 startPosition;
    private MatchablePool matchablePool;

    private Matchable[,] grid;

    private void Start()
    {
        matchablePool = (MatchablePool)MatchablePool.Instance;
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        matchablePool.ObjectsPool(columns * rows * 2);
        grid = new Matchable[rows, columns];

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector2 position = startPosition + new Vector2(col * tileSize.x, -row * tileSize.y);
                Matchable newTile = matchablePool.GetObjectPooled();
                newTile.transform.position = position;
                newTile.gameObject.SetActive(true);

                int type = GetValidType(row, col);
                matchablePool.RandomizeType(newTile,type);

                grid[row, col] = newTile;
            }
        }
    }
    public int GetValidType(int row, int col)
    {
        int type;
        do
        {
            type = UnityEngine.Random.Range(0, matchablePool.howManyTypes);
        }
        while(IsHorizontalMatch(row, col,type) || IsVerticalMatch(row,col,type));

        return type;
    }
    private bool IsHorizontalMatch(int row, int col, int type)
    {
        if (col < 2) return false;

        return grid[row, col - 1] != null && grid[row, col - 2] != null &&
               grid[row, col - 1].Type == type && grid[row, col - 2].Type == type;
    }

    private bool IsVerticalMatch(int row, int col, int type)
    {
        if (row < 2) return false;

        return grid[row - 1, col] != null && grid[row - 2, col] != null &&
               grid[row - 1, col].Type == type && grid[row - 2, col].Type == type;
    }

    public void SwapTiles(Vector2Int firstPos, Vector2Int secondPos)
    {
        Matchable temp = grid[firstPos.x, firstPos.y];
        grid[firstPos.x, firstPos.y] = grid[secondPos.x, secondPos.y];
        grid[secondPos.x, secondPos.y] = temp;

        Vector3 tempPosition = grid[firstPos.x, firstPos.y].transform.position;
        grid[firstPos.x, firstPos.y].transform.position = grid[secondPos.x, secondPos.y].transform.position;
        grid[secondPos.x, secondPos.y].transform.position = tempPosition;
    }
    public bool CheckMatch(Matchable firstSelect, Matchable secondSelect)
    {
        Vector2Int firstPos = GetTilePosition(firstSelect);
        Vector2Int secondPos = GetTilePosition(secondSelect);


        return true;
    }
    private Vector2Int GetTilePosition(Matchable tile)
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (grid[row,col] == tile)
                    return new Vector2Int(row, col);
            }
        }
        Debug.LogError("Tile not foun in the grid!");
        return new Vector2Int(-1, -1);
    }
    public bool CheckMatchAfterSwap(Matchable first, Matchable second)
    {
        Vector2Int firstPos = GetTilePosition(first);
        Vector2Int secondPos = GetTilePosition(second);

        SwapTiles(firstPos, secondPos);

        bool hasMatch = HasHorizontalMatch(firstPos) || HasVerticalMatch(firstPos) ||
                        HasHorizontalMatch(secondPos) || HasVerticalMatch(secondPos);

        SwapTiles(firstPos, secondPos);

        return hasMatch;
    }


    private bool HasHorizontalMatch(Vector2Int position)
    {
        int row = position.x;
        int col = position.y;

        if (col == -1 || row == -1)
            return false;


        int currentType = grid[row,col].Type;

        if (col > 1)
        {
            int leftType1 = grid[row, col - 1]?.Type ?? -1;
            int leftType2 = grid[row, col - 2]?.Type ?? -1;

            if (currentType == leftType1 && currentType == leftType2)
                return true;
        }

        if (col < columns - 2)
        {
            int rightType1 = grid[row, col + 1]?.Type ?? -1;
            int rightType2 = grid[row, col + 2]?.Type ?? -1;

            if (currentType == rightType1 && currentType == rightType2)
                return true;
        }

        if (col > 0 && col < columns - 1)
        {
            int leftType = grid[row, col - 1]?.Type ?? -1;
            int rightType = grid[row, col + 1]?.Type ?? -1;

            if (currentType == leftType && currentType == rightType)
                return true;
        }

        return false;
    }
    private bool HasVerticalMatch(Vector2Int position)
    {
        int row = position.x;
        int col = position.y;

        if (col == -1 || row == -1)
            return false;

        int currentType = grid[row,col].Type;

        if (row > 1)
        {
            int upperType1 = grid[row - 1, col]?.Type ?? -1;
            int upperType2 = grid[row - 2, col]?.Type ?? -1;

            if (currentType == upperType1 && currentType == upperType2)
                return true;
        }

        if (row < rows - 2)
        {
            int lowerType1 = grid[row + 1, col]?.Type ?? -1;
            int lowerType2 = grid[row + 2, col]?.Type ?? -1;

            if (currentType == lowerType1 && currentType == lowerType2)
                return true;
        }

        if (row > 0 && row < rows - 1)
        {
            int upperType = grid[row - 1, col]?.Type ?? -1;
            int lowerType = grid[row + 1, col]?.Type ?? -1;

            if (currentType == upperType && currentType == lowerType)
                return true;
        }

        return false;
    }

    public IEnumerator SwapAnimation(Matchable first, Matchable second, float duration)
    {
        Debug.Log("Swap Animation Started!");

        Vector3 firstStartPosition = first.transform.position;
        Vector3 secondStartPosition = second.transform.position;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            first.transform.position = Vector3.Lerp(firstStartPosition, secondStartPosition, elapsed / duration);
            second.transform.position = Vector3.Lerp(secondStartPosition, firstStartPosition, elapsed / duration);

            yield return null;
        }

        first.transform.position = secondStartPosition;
        second.transform.position = firstStartPosition;
    }
}