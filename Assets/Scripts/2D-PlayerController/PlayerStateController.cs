using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : MonoBehaviour {

    public enum state {IDLE, WALKING, JUMPING, FALLING};
    public int currentState;

    private Rigidbody2D rigidBody;
    private PlayerController controller;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        controller = GetComponent<PlayerController>();
    }
    void Update()
    {
        if (CheckIdle())
        {
            SetStateIdle();
        }
        else if (CheckWalking())
        {
            SetStateWalking();
        }
        else if (CheckJumping())
        {
            SetStateJumping();
        }
        else if (CheckFalling())
        {
            SetStateFalling();
        }

    }

    private bool CheckIdle()
    {
        if (controller.IsGrounded() && Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
            return true;
        return false;
    }

    private bool CheckWalking()
    {
        if (controller.IsGrounded() && Math.Abs(Input.GetAxisRaw("Horizontal")) > 0)
            return true;
        return false;
    }

    private bool CheckJumping()
    {
        if (!controller.IsGrounded() && rigidBody.velocity.y > 0)
            return true;
        return false;
    }

    private bool CheckFalling()
    {
        if (!controller.IsGrounded() && rigidBody.velocity.y < 0)
            return true;
        return false;
    }

    public void SetStateIdle()
    {
        currentState = (int)state.IDLE;
    }

    public void SetStateWalking()
    {
        currentState = (int)state.WALKING;
    }

    public void SetStateJumping()
    {
        currentState = (int)state.JUMPING;
    }
    
    public void SetStateFalling()
    {
        currentState = (int)state.FALLING;
    }
}
