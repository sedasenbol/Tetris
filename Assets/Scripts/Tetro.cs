using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Tetro : MonoBehaviour
{
    public static event Action OnTetroGrounded;
    public static event Action OnGameOverCollision;

    private const int GAME_OVER_HEIGHT = 22;
    private float GoDownWaitSeconds = 1f;
    private bool isGrounded = false;
    private BoxCollider2D[] colliders;
    private Transform[,] grid;
    private Transform[] squares = new Transform[4];

    private void Start()
    {
        for (int i= 0; i < 4; i++)
        {
            squares[i] = transform.GetChild(i).transform;
        }

        colliders = GetComponentsInChildren<BoxCollider2D>();
        grid = FindObjectOfType<Board>().TetroGrid;
        
        GoDown();
    }

    private void Update()
    {
        if (!isGrounded)
        {
            IsGroundedCheck();
            GoDownFast();
            RotateCW();
            RotateCCW();
            MoveLeft();
            MoveRight();
        }
    }

    private void IsGroundedCheck()
    {
        for (int i = 0; i < 4; i++)
        {
            if (squares[i].position.y <= 0 || grid[Convert.ToInt32(squares[i].position.x), Convert.ToInt32(squares[i].position.y - 1)] != null)
            {
                isGrounded = true;
            }
        }
    }

    public void GoDown()
    {
         StartCoroutine(GoDownSlowly());
    }
    private IEnumerator GoDownSlowly()
    {
        while(!isGrounded)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - 1);
            yield return new WaitForSecondsRealtime(GoDownWaitSeconds);
        }
        AssignTetroToGrid();
    }

    private void GoDownFast()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            GoDownWaitSeconds = 0.05f;
        }
        else
        {
            GoDownWaitSeconds = 1f;
        }
    }

    private void AssignTetroToGrid()
    {
        bool isGameOverCollision = false;
        for (int i = 0; i < 4; i++)
        {
            grid[Convert.ToInt32(squares[i].position.x), Convert.ToInt32(squares[i].position.y)] = squares[i];
            if (squares[i].position.y == GAME_OVER_HEIGHT)
            {
                isGameOverCollision = true;
            }
        }
        if (isGameOverCollision)
        {
            OnGameOverCollision?.Invoke();
            return;
        }
        OnTetroGrounded?.Invoke();
    }

    public void RotateCW()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            transform.Rotate(0f, 0f, 90f, Space.Self);
        }
    }

    public void RotateCCW()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.Rotate(0f, 0f, -90f, Space.Self);
        }
    }

    public void MoveLeft()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            bool isLeftSideAvailable = true;
            for (int i = 0; i < 4; i++)
            {
                if (Convert.ToInt32(squares[i].position.x) == 0)
                {
                    isLeftSideAvailable = false;
                }
            }
            if (isLeftSideAvailable)
            {
                transform.position = new Vector2(transform.position.x - 1, transform.position.y);
            }
        }
    }

    public void MoveRight()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            bool isRightSideAvailable = true;
            for (int i = 0; i < 4; i++)
            {
                if (Convert.ToInt32(squares[i].position.x) == 9)
                {
                    isRightSideAvailable = false;
                }
            }
            if (isRightSideAvailable)
            {
                transform.position = new Vector2(transform.position.x + 1, transform.position.y);
            }
        }
    }
}
