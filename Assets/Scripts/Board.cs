using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Board : MonoBehaviour
{
    [SerializeField]
    private Transform emptyCell;
    private Transform[,] grid = new Transform[boardWidth, boardHeight+5];
    private const int boardHeight = 22;
    private const int boardWidth = 10;

    public static event Action OnClearLines;

    private void OnEnable()
    {
        Tetro.OnTetroGrounded += CheckLine;
    }

    private void OnDisable()
    {
        Tetro.OnTetroGrounded -= CheckLine;
    }

    private void Start()
    {
        DrawEmptyCells();
    }

    private void Update()
    {
        
    }
    private void DrawEmptyCells()
    {
        for (int y=0; y<boardHeight; y++)
        {
            for (int x=0; x<boardWidth; x++)
            {
                var clone = Instantiate<Transform>(emptyCell,new Vector3(x,y,0),Quaternion.identity,transform);
                clone.name = "x = " + x + ", y = " + y;
            }
        }
    }

    private void CheckLine()
    {
        for (int i = 0; i< boardHeight; i++)
        {
            bool isLineFull = true;
            for (int j = 0; j < boardWidth; j++)
            {
                if (grid[j, i] == null)
                {
                    isLineFull = false;
                }
            }
            if (isLineFull)
            {
                ClearLines(i);
                OnClearLines?.Invoke();
                isLineFull = true;
            }
        }
    }

    private void ClearLines(int lineToClear)
    {
        for (int i = 0; i < boardWidth; i++ )
        {
            Destroy(grid[i, lineToClear].gameObject);
            for (int j = lineToClear + 1; j < boardHeight; j++)
            {
                if(grid[i,j] != null)
                {
                    grid[i, j].position = new Vector2(grid[i, j].position.x, grid[i, j].position.y - 1);

                    grid[i, j - 1] = grid[i, j];
                    grid[i, j] = null;
                }
            }
        }
    }

    public Transform[,] TetroGrid { get { return grid; } set { TetroGrid = value; } }

}
