using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Board : MonoBehaviour
{
    public static event Action OnClearLines;

    [SerializeField]
    private Transform emptyCell;
    private Transform[,] grid = new Transform[BOARD_WIDTH, BOARD_HEIGHT+8];
    private const int BOARD_HEIGHT = 22;
    private const int BOARD_WIDTH = 10;

    private void OnEnable()
    {
        Tetro.OnTetroGrounded += CheckLines;
    }

    private void OnDisable()
    {
        Tetro.OnTetroGrounded -= CheckLines;
    }

    private void DrawEmptyCells()
    {
        for (int y = 0; y < BOARD_HEIGHT; y++)
        {
            for (int x = 0; x < BOARD_WIDTH; x++)
            {
                var clone = Instantiate<Transform>(emptyCell, new Vector3(x, y, 0), Quaternion.identity, transform);
                clone.name = "x = " + x + ", y = " + y;
            }
        }
    }

    private void CheckLines()
    {
        List<int> linesToClear = new List<int>();
        for (int i = 0; i < BOARD_HEIGHT; i++)
        {
            bool isLineFull = true;
            for (int j = 0; j < BOARD_WIDTH; j++)
            {
                if (grid[j, i] == null)
                {
                    isLineFull = false;
                }
            }
            if (isLineFull)
            {
                linesToClear.Add(i);
                OnClearLines?.Invoke();
            }
        }
        ClearLines(linesToClear);
    }

    private void ClearLines(List<int> linesToClear)
    {
        if (!linesToClear.Any())
        {
            return;
        }

        for (int k = 0; k < linesToClear.Count; k++)
        {
            for (int i = 0; i < BOARD_WIDTH; i++)
            {
                Destroy(grid[i, linesToClear[k]].gameObject);
            }
            ShiftLines(linesToClear[k]);

            for (int i = 0; i < linesToClear.Count; i++)
            {
                linesToClear[i]--;
            }
        }
        linesToClear.Clear();
    }

    private void ShiftLines(int startLine)
    {
        for (int j = startLine + 1; j < BOARD_HEIGHT; j++)
        {
            for (int i = 0; i < BOARD_WIDTH; i++)
            {
                if (grid[i, j] != null)
                {
                    grid[i, j].position = new Vector2(grid[i, j].position.x, grid[i, j].position.y - 1);

                    grid[i, j - 1] = grid[i, j];
                    grid[i, j] = null;
                }
            }
        }
    }

    private void Start()
    {
        DrawEmptyCells();
    }

    public Transform[,] TetroGrid { get { return grid; } set { grid = value; } }

}
