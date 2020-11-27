using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Tetro : MonoBehaviour
{
    public static event Action OnTetroGrounded;
    public static event Action OnGameOverCollision;

    private const float GO_DOWN_SPEED = 5f;
    private const float GAME_OVER_HEIGHT = 20f;
    private bool isGrounded = false;
    private BoxCollider2D[] colliders;
    private void Start()
    {
        colliders = GetComponentsInChildren<BoxCollider2D>();
    }
    private void Update()
    {
        IsGroundedCheck();
        if (!isGrounded)
        {
            GoDown();
            RotateCW();
            RotateCCW();
            MoveLeft();
            MoveRight();
        }
    }

    public void GoDown()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - 5 * GO_DOWN_SPEED * Time.deltaTime);
        }
        transform.position = new Vector2(transform.position.x, transform.position.y - GO_DOWN_SPEED * Time.deltaTime);
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
        if (colliders[0].IsTouchingLayers(LayerMask.GetMask("LeftSide"))
        || colliders[1].IsTouchingLayers(LayerMask.GetMask("LeftSide"))
        || colliders[2].IsTouchingLayers(LayerMask.GetMask("LeftSide"))
        || colliders[3].IsTouchingLayers(LayerMask.GetMask("LeftSide")))
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position = new Vector2(transform.position.x - 1, transform.position.y);
        }

    }

    public void MoveRight()
    {
        if (colliders[0].IsTouchingLayers(LayerMask.GetMask("RightSide"))
        || colliders[1].IsTouchingLayers(LayerMask.GetMask("RightSide"))
        || colliders[2].IsTouchingLayers(LayerMask.GetMask("RightSide"))
        || colliders[3].IsTouchingLayers(LayerMask.GetMask("RightSide")))
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position = new Vector2(transform.position.x + 1, transform.position.y);
        }
    }
    private void IsGroundedCheck()
    {
        if(isGrounded)
        {
            return;
        }
        if (colliders[0].IsTouchingLayers(LayerMask.GetMask("Ground","Tetro"))
            || colliders[1].IsTouchingLayers(LayerMask.GetMask("Ground","Tetro"))
            || colliders[2].IsTouchingLayers(LayerMask.GetMask("Ground", "Tetro"))
            || colliders[3].IsTouchingLayers(LayerMask.GetMask("Ground", "Tetro")))
        {
            if (transform.position.y <= GAME_OVER_HEIGHT)
            {
                isGrounded = true;
                OnTetroGrounded?.Invoke();
            }
            else
            {
                OnGameOverCollision?.Invoke();
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 9)
        {
            isGrounded = true;
            OnTetroGrounded?.Invoke();
        }
    }
}
