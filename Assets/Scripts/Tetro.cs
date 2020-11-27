using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tetro : MonoBehaviour
{
    private const float GO_DOWN_SPEED = 5f;
    private bool isGrounded = false;
    private void Start()
    {
        
    }
    private void Update()
    {
        if (!isGrounded)
        {
            IsGroundedCheck();
            GoDown();
            RotateCW();
            RotateCCW();
            MoveLeft();
            MoveRight();
        }
    }

    public void GoDown()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y - GO_DOWN_SPEED * Time.deltaTime);
    }

    public void RotateCW()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            transform.Rotate(0f,0f, 90f,Space.Self);
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
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position = new Vector2(transform.position.x - 1, transform.position.y);
        }

    }

    public void MoveRight()
    {
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
    }
}
