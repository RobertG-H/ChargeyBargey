using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : MonoBehaviour {

    public delegate void DeathEvent(int playerNum);
    public event DeathEvent OnPlayerDeath;
    public enum state {IDLE, WALKING, JUMPING, FALLING, DEAD, WALLSLIDE, SHOOTING};
    public int currentState;
    [SerializeField]

    private Rigidbody2D rigidBody;
    private PlayerController controller;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        controller = GetComponent<PlayerController>();
    }
    void Update()
    {
        if(currentState != (int)state.DEAD)
        {
            if (CheckCharge())
            {
                SetStateDead();
            }
            else if (CheckShooting())
            {
                SetStateShooting();
            }
            else if (CheckIdle())
            {
                SetStateIdle();
            }
            else if (CheckWall())
            {
                SetStateWall();
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
    }

    private bool CheckIdle()
    {
        if (controller.IsGrounded() && Input.GetAxisRaw("Horizontal" + controller.GetPlayerNum().ToString()) == 0)
            return true;
        return false;
    }

    private bool CheckWall() 
    {
        int layerMask = 1 << 2;
        layerMask = ~layerMask;
        if (controller.CheckWalled(layerMask))
            return true;
        return false;
    }

    private bool CheckWalking()
    {
        if (controller.IsGrounded() && Math.Abs(Input.GetAxisRaw("Horizontal" + controller.GetPlayerNum().ToString())) > 0)
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

    private bool CheckCharge()
    {
        if (controller.GetCharge() > 100)
        {
            return true;
        }

        return false;
    }

    private bool CheckShooting()
    {
        return controller.IsShooting();
    }

    public void SetStateIdle()
    {
        currentState = (int)state.IDLE;
    }

    public void SetStateWall() 
    {
        currentState = (int)state.WALLSLIDE;
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

    public void SetStateDead()
    {
        if(OnPlayerDeath != null)
        {
            currentState = (int)state.DEAD;
            OnPlayerDeath(controller.GetPlayerNum());
            controller.playDead();
        }
        else
        {
            Debug.Log("Event was null");
        }
    }

    public void SetStateShooting()
    {
        currentState = (int)state.SHOOTING;
    }

}
