using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]
    private Transform emptyCell;
    private const int boardHeight = 22;
    private const int boardWidth = 10;
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
}
