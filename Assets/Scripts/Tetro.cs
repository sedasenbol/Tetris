using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tetro : MonoBehaviour
{
    public static event Action OnTetroGrounded;
    public static event Action OnGameOverCollision;

    private const int GAME_OVER_HEIGHT = 22;
    private const int SQUARE_COUNT = 4;
    private float GoDownWaitSeconds = 0.5f;
    private bool isGrounded = false;
    private Transform[,] grid;
    private Transform[] squares = new Transform[SQUARE_COUNT];

    private void IsGroundedCheck()
    {
        foreach (Transform xform in squares)
        {
            if (xform.position.y <= 0 || PositionToGrid(xform.position + new Vector3(0, -1, 0)) != null)
            {
                isGrounded = true;
                return;
            }
        }
    }

    private IEnumerator GoDown()
    {
        while (!isGrounded)
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
            GoDownWaitSeconds = 0.03f;
        }
        else
        {
            GoDownWaitSeconds = 1f;
        }
    }

    private void AssignTetroToGrid()
    {
        bool isGameOverCollision = false;

        foreach (Transform xform in squares)
        {
            grid[Convert.ToInt32(xform.position.x), Convert.ToInt32(xform.position.y)] = xform;

            if (xform.position.y == GAME_OVER_HEIGHT)
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

    private Transform PositionToGrid(Vector3 position)
    {
        return grid[Convert.ToInt32(position.x), Convert.ToInt32(position.y)];
    }

    private Vector3 RotatedSquarePos(Vector3 position, bool isCW)
    {
        if (isCW)
        {
            return new Vector3(-position.y, position.x, 0);
        }
        else
        {
            return new Vector3(position.y, -position.x, 0);
        }
    }

    private void RotateCW()
    {
        if (!Input.GetKeyDown(KeyCode.S))
        {
            return;
        }

        bool canRotateCW = true;

        foreach (Transform xform in squares)
        {
            Vector2 newPosition = RotatedSquarePos(xform.localPosition, true) + transform.position + new Vector3(0, -1, 0);

            if (newPosition.x < 0 || newPosition.x > 9 || newPosition.y < 0 || PositionToGrid(newPosition) != null)
            {
                canRotateCW = false;
            }
        }

        if (canRotateCW)
        {
            foreach (Transform xform in squares)
            {
                xform.RotateAround(squares[0].position, new Vector3(0, 0, 1), 90);
            }
        }
    }

    private void RotateCCW()
    {
        if (!Input.GetKeyDown(KeyCode.A))
        {
            return;
        }

        bool canRotateCCW = true;

        foreach (Transform xform in squares)
        {
            Vector2 newSquarePos = RotatedSquarePos(xform.localPosition, false) + transform.position + new Vector3(0, -1, 0);
            if (newSquarePos.x < 0 || newSquarePos.x > 9 || newSquarePos.y < 0 || PositionToGrid(newSquarePos) != null)
            {
                canRotateCCW = false;
            }
        }

        if (canRotateCCW)
        {
            foreach (Transform xform in squares)
            {
                xform.RotateAround(squares[0].position, new Vector3(0, 0, 1), -90);
            }
        }
    }

    private void MoveLeft()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            bool isLeftSideAvailable = true;

            foreach (Transform xform in squares)
            {
                Vector3 newSquarePos = new Vector3(xform.position.x - 1, xform.position.y, 0);
                if (Convert.ToInt32(newSquarePos.x) == -1 || PositionToGrid(newSquarePos) != null)
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

    private void MoveRight()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            bool isRightSideAvailable = true;

            foreach (Transform xform in squares)
            {
                Vector3 newSquarePos = new Vector3(xform.position.x + 1, xform.position.y, 0);
                if (Convert.ToInt32(newSquarePos.x) == 10 || PositionToGrid(newSquarePos) != null)
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

    private void AnyChildLeft()
    {
        if (transform.childCount == 0)
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        for (int i = 0; i < SQUARE_COUNT; i++)
        {
            squares[i] = transform.GetChild(i).transform;
        }

        grid = FindObjectOfType<Board>().TetroGrid;

        StartCoroutine(GoDown());
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
        else
        {
            AnyChildLeft();
        }
    }
}
