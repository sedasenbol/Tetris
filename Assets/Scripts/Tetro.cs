using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tetro : MonoBehaviour
{
    public static event Action OnTetroGrounded;
    public static event Action OnGameOverCollision;

    private const float SQUARE_LENGTH =0.5f;
    private const float GAME_OVER_HEIGHT = 22 * SQUARE_LENGTH;
    private const int SQUARE_COUNT = 4;
    private float lastDescensionTime;
    private float currentDescensionDelay = 0.5f;
    private bool isGrounded = false;

    private Transform[,] grid;
    private Transform[] squares = new Transform[SQUARE_COUNT];


    private void GoDown()
    {
        if (Time.time - lastDescensionTime > currentDescensionDelay)
        {
            foreach (Transform xform in squares)
            {
                if (Mathf.RoundToInt(xform.position.y / SQUARE_LENGTH) == 0 || PositionToGrid(xform.position + new Vector3(0, -SQUARE_LENGTH, 0)) != null)
                {
                    isGrounded = true;
                    AssignTetroToGrid();
                    return;
                }
            }

            transform.position = new Vector2(transform.position.x, transform.position.y - SQUARE_LENGTH);

            lastDescensionTime = Time.time;
        }
    }

    private void CheckForFasterDownMovementCommands()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            currentDescensionDelay = 0.03f;
        }
        else
        {
            currentDescensionDelay = 0.5f;
        }
    }

    private void AssignTetroToGrid()
    {
        bool isGameOverCollision = false;

        foreach (Transform xform in squares)
        {
            grid[Mathf.RoundToInt(xform.position.x / SQUARE_LENGTH), Mathf.RoundToInt(xform.position.y / SQUARE_LENGTH)] = xform;

            if (Mathf.RoundToInt(xform.position.y) == GAME_OVER_HEIGHT)
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
        return grid[Mathf.RoundToInt(position.x / SQUARE_LENGTH), Mathf.RoundToInt(position.y / SQUARE_LENGTH)];
    }

    private Vector3 RotateSquare(Vector3 position, bool isCW)
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

    private void CheckForCWRotationCommand()
    {
        if (!Input.GetKeyDown(KeyCode.S))
        {
            return;
        }

        bool canRotateCW = true;

        foreach (Transform xform in squares)
        {
            Vector2 newPosition = RotateSquare(xform.localPosition, true) + transform.position;

            if (newPosition.x < 0 || newPosition.x > 9 * SQUARE_LENGTH || newPosition.y < 0 || PositionToGrid(newPosition) != null)
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

    private void CheckForCCWRotationCommand()
{
        if (!Input.GetKeyDown(KeyCode.A))
        {
            return;
        }

        bool canRotateCCW = true;

        foreach (Transform xform in squares)
        {
            Vector2 newSquarePos = RotateSquare(xform.localPosition, false) + transform.position;
            if (newSquarePos.x < 0 || newSquarePos.x > 9 * SQUARE_LENGTH || newSquarePos.y < 0 || PositionToGrid(newSquarePos) != null)
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

    private void CheckForStrafeLeftCommand()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            bool isLeftSideAvailable = true;

            foreach (Transform xform in squares)
            {
                Vector3 newSquarePos = new Vector3(xform.position.x - SQUARE_LENGTH, xform.position.y, 0);
                if (Mathf.RoundToInt(newSquarePos.x / SQUARE_LENGTH) == -1 || PositionToGrid(newSquarePos) != null)
                {
                    isLeftSideAvailable = false;
                }
            }
            if (isLeftSideAvailable)
            {
                transform.position = new Vector2(transform.position.x - SQUARE_LENGTH, transform.position.y);
            }
        }
    }

    private void CheckForStrafeRightCommand()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            bool isRightSideAvailable = true;

            foreach (Transform xform in squares)
            {
                Vector3 newSquarePos = new Vector3(xform.position.x + SQUARE_LENGTH, xform.position.y, 0);
                if (Mathf.RoundToInt(newSquarePos.x / SQUARE_LENGTH) == 10 || PositionToGrid(newSquarePos) != null)
                {
                    isRightSideAvailable = false;
                }
            }
            if (isRightSideAvailable)
            {
                transform.position = new Vector2(transform.position.x + SQUARE_LENGTH, transform.position.y);
            }
        }
    }

    private void CheckForRemainingChildren()
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

        lastDescensionTime = Time.deltaTime;
    }

    private void Update()
    {
        if (!isGrounded)
        {
            CheckForFasterDownMovementCommands();
            CheckForCWRotationCommand();
            CheckForCCWRotationCommand();
            CheckForStrafeLeftCommand();
            CheckForStrafeRightCommand();
            
            GoDown();
        }
        else
        {
            CheckForRemainingChildren();
        }
    }
}
