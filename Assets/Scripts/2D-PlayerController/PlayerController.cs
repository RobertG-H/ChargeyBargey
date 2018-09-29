using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    [SerializeField]
    private GameObject slashHitBox;
    private PlayerAnimations animations;

    #region Conditions
    private bool isGrounded;
    private bool isJumping;
    #endregion

    private Vector3 fullHopPoint;
    #region Constants
    private const float MAXSPEEDX = 10f;
    private const float ACCELERATIONX = 50f;
    [SerializeField]
    private float rayCastDownDist = 1.0f;
    private const float jumpForce = 350f;
    private const float fastFallForce = -50.0f;
    private const float jumpForceHeld = 60.0f;
    private const float fullHopJumpHeight = 1.2f;
    #endregion

    void Start()
    {
        animations = GetComponent<PlayerAnimations>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CheckGrounded();
    }

    private void CheckGrounded()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Ignore Raycast");
        layerMask = ~layerMask;
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * rayCastDownDist, Color.red);
        if (Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.down), rayCastDownDist, layerMask).collider == null)
        {
            isGrounded = false;
        }
        else
        {
            isGrounded = true;
        }
    }

    public void Move(float speed)
    {
        if (Math.Abs(GetSpeedX()) > MAXSPEEDX)
        {
            rigidBody.velocity = new Vector2(MAXSPEEDX * Math.Sign(rigidBody.velocity.x), rigidBody.velocity.y);
        }
        else
        {
            rigidBody.AddForce(new Vector2(speed * ACCELERATIONX, 0));
        }

    }

    public void Stop()
    {
        rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
    }

    public void Jump()
    {
        if(isGrounded)
        {
            fullHopPoint = new Vector3(transform.position.x, transform.position.y + fullHopJumpHeight, transform.position.z);
            rigidBody.AddForce(new Vector2(0, jumpForce));
            isJumping = true;
        }
    }

    public void ContinueJump()
    {
        if(isJumping)
        {
            if (transform.position.y < fullHopPoint.y && rigidBody.velocity.y > 0)
            {
                rigidBody.AddForce(new Vector2(0, jumpForceHeld));
            }
            else
            {
                isJumping = false;
            }
        }
    }

    public void FastFall()
    {
        if (!isGrounded)
        {
            rigidBody.AddForce(new Vector2(0, fastFallForce));
        }
    }

    public void SlashStart()
    {
        slashHitBox.SetActive(true);
        //animations.SlashAnim();
    }

    public void SlashEnd()
    {
        slashHitBox.SetActive(false);
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    public float GetSpeedX()
    {
        return rigidBody.velocity.x;
    }

    
    public void FlipSlashHitBox()
    {
        // Uncomment for slash hitbox
        /*
        slashHitBox.transform.localPosition = new Vector2(-slashHitBox.transform.localPosition.x, slashHitBox.transform.localPosition.y);
        slashHitBox.transform.localScale = new Vector2(-slashHitBox.transform.localScale.x, slashHitBox.transform.localScale.y);
        */
    }



    //    //
    //    //public enum State { IDLE, WALKING, JUMPING, FALLING };
    //    //public int currentState;
    //    //public float rayCastDownDist;

    //    //private float maxSpeedX = 10f;
    //    //private float accelX = 50f;
    //    //private float speedX;
    //    //private float slowDownX = 1.2f;

    //    //private Collider2D Hit;

    //    //private float jumpForce = 350f;
    //    //private float fastFallForce = -50.0f;
    //    //private float jumpForceHeld = 60.0f;
    //    //private bool isGrounded;
    //    //private bool isJumpHeld;
    //    //private float fullHopJumpHeight = 1.2f;
    //    //private Vector3 fullHopPoint;

    //    //public bool slash;
    //    //public GameObject SlashHitBox;
    //    //private int slashFrames = 9;
    //    //private int slashFramesCount = 0;

    //    //private Animator anim;



    //    //// Use this for initialization
    //    //void Start()
    //    //{
    //    //    RBody = GetComponent<Rigidbody2D>();
    //    //}

    //    //// Update is called once per frame
    //    //void Update()
    //    //{
    //    //    JumpUpdate(Input.GetButtonDown("Jump"), Input.GetButtonUp("Jump"), Input.GetButton("Jump"));
    //    //    FallUpdate(Input.GetAxisRaw("Vertical"));
    //    //    WalkUpdate(Input.GetAxisRaw("Horizontal"));
    //    //    SlashUpdate();
    //    //}

    //    //private void WalkUpdate(float InputHorizontal)
    //    //{

    //    //    speedX = InputHorizontal * accelX;// * Time.deltaTime;
    //    //    if (isGrounded)
    //    //    {
    //    //        if (speedX == 0)
    //    //        {
    //    //            currentState = (int)State.IDLE;
    //    //            RBody.velocity = new Vector2(RBody.velocity.x / slowDownX, RBody.velocity.y);
    //    //        }
    //    //        else
    //    //        {
    //    //            currentState = (int)State.WALKING;
    //    //        }
    //    //    }
    //    //    if (System.Math.Abs(RBody.velocity.x) > maxSpeedX)
    //    //    {
    //    //        RBody.velocity = new Vector2(maxSpeedX * System.Math.Sign(RBody.velocity.x), RBody.velocity.y);
    //    //    }
    //    //    else
    //    //    {
    //    //        RBody.AddForce(new Vector2(speedX, 0));
    //    //    }
    //    //}

    //    //private void JumpUpdate(bool InputJumpDown, bool InputJumpUp, bool InputJumpHeld)
    //    //{
    //    //    if (InputJumpDown && isGrounded)
    //    //    {
    //    //        isJumpHeld = true;
    //    //        fullHopPoint = new Vector3(transform.position.x, transform.position.y + fullHopJumpHeight, transform.position.z);
    //    //        RBody.AddForce(new Vector2(0, jumpForce));
    //    //        currentState = (int)State.JUMPING;
    //    //    }
    //    //    else if (InputJumpUp || transform.position.y >= fullHopPoint.y || RBody.velocity.y < 0)
    //    //    {
    //    //        isJumpHeld = false;
    //    //    }
    //    //    else if (InputJumpHeld && isJumpHeld && transform.position.y < fullHopPoint.y && RBody.velocity.y > 0)
    //    //    {
    //    //        RBody.AddForce(new Vector2(0, jumpForceHeld));
    //    //    }
    //    //}

    //    //private void FallUpdate(float InputVertical)
    //    //{
    //    //    int layerMask = 1 << LayerMask.NameToLayer("Ignore Raycast");
    //    //    layerMask = ~layerMask;
    //    //    Hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.down), rayCastDownDist, layerMask).collider;
    //    //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * rayCastDownDist, Color.red);
    //    //    if (Hit == null)
    //    //    {
    //    //        isGrounded = false;
    //    //        if (RBody.velocity.y < 0)
    //    //        {
    //    //            currentState = (int)State.FALLING;
    //    //            if (InputVertical < 0)
    //    //            {
    //    //                RBody.AddForce(new Vector2(0, fastFallForce));
    //    //            }
    //    //        }
    //    //        else if (RBody.velocity.y > 0)
    //    //        {
    //    //            currentState = (int)State.JUMPING;
    //    //        }
    //    //    }
    //    //    else// if(RBody.velocity.y == 0)
    //    //    {
    //    //        isGrounded = true;
    //    //    }
    //    //}

    //    //private void SlashUpdate()
    //    //{
    //    //    if (Input.GetButtonDown("Slash") && !slash)
    //    //    {
    //    //        slashFramesCount = 0;
    //    //        slash = true;
    //    //        SlashHitBox.SetActive(true);
    //    //    }
    //    //    if (slashFramesCount < slashFrames)
    //    //    {
    //    //        slashFramesCount++;
    //    //    }
    //    //    else
    //    //    {
    //    //        slash = false;
    //    //        SlashHitBox.SetActive(false);
    //    //    }
    //    //}

    //    //public float GetSpeedX()
    //    //{
    //    //    return RBody.velocity.x;
    //    //}





}
