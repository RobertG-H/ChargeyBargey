using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private GameObject slashHitBox;
    [SerializeField]
    private int playerNum;
    private PlayerAnimations animations;
    public ProjectileController[] projectiles;

    public AudioClip turnClip;
    public AudioClip jumpClip;
    public AudioClip deathClip;
    public AudioClip tiddlerClip;
    public AudioClip kiBlastClip;
    public AudioClip shotgunClip;
    public AudioClip sniperClip;
    public AudioClip blastClip;
    public AudioClip PDClip;

    public AudioSource audioSource;

    // [speed, duration]
    private float[,] projectileProps = new float[,] {
        {30, 0.35f},
        {20, 1},
        {12, 3},
        {50, 10},
        {25, 10},
        {8, 20},
    };
    private playerCharge pCharge;

    #region Conditions
    private bool isGrounded;
    private bool isJumping;
    private bool touchingLeftWall;
    private bool touchingRightWall;
    #endregion

    private Vector3 fullHopPoint;
    #region Constants
    private const float MAXSPEEDX = 16f;
    private float ACCELERATIONX = 50f;
    [SerializeField]
    private float rayCastDownDist = 2.0f;
    [SerializeField]
    private float rayCastSideDist = 0.5f;
    private const float jumpForce = 500f;
    private const float fastFallForce = -50.0f;
    private const float jumpForceHeld = 80f;
    private const float fullHopJumpHeight = 1.8f;
    #endregion

    void Start()
    {
        animations = GetComponent<PlayerAnimations>();
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        pCharge = GetComponent<playerCharge>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if ( !isGrounded ) {
            ACCELERATIONX = 100f;
        } else if ( isGrounded ){
            ACCELERATIONX = 100f;
        }
        int layerMask = 1 << 2;
        layerMask = ~layerMask;
        CheckGrounded( layerMask );
        CheckWalled( layerMask );
    }

    private void CheckGrounded( int layerMask )
    {
        //int layerMask = 1 << LayerMask.NameToLayer("Ignore Raycast");
        //layerMask = ~layerMask;
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * rayCastDownDist, Color.red);
        Debug.DrawRay(transform.position, transform.TransformDirection(new Vector3 (-0.15f,-1,0)) * rayCastDownDist, Color.red);
        Debug.DrawRay(transform.position, transform.TransformDirection(new Vector3 (0.15f,-1,0)) * rayCastDownDist, Color.red);
        Collider2D downCast = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.down), rayCastDownDist, layerMask).collider;
        Collider2D downleftCast = Physics2D.Raycast(transform.position, transform.TransformDirection(new Vector3 (-0.15f,-1,0)), rayCastDownDist, layerMask).collider;
        Collider2D downrightCast = Physics2D.Raycast(transform.position, transform.TransformDirection(new Vector3 (0.15f,-1,0)), rayCastDownDist, layerMask).collider; 
        if (downCast == null && downleftCast == null && downrightCast == null)
        {
            isGrounded = false;
        }
        else
        {
            isGrounded = true;
        }
    }

    public bool CheckWalled( int layerMask ) {
        Vector3 leftRay = new Vector3(-1, 0, 0);
        Vector3 rightRay = new Vector3(1, 0, 0);

        Debug.DrawRay(transform.position - new Vector3(0, 1.8f, 0), transform.TransformDirection(leftRay) * rayCastSideDist, Color.red);
        Debug.DrawRay(transform.position - new Vector3(0, 1.8f, 0), transform.TransformDirection(rightRay) * rayCastSideDist, Color.green);
        
        Collider2D raycastColliderLeft1 = Physics2D.Raycast(transform.position, transform.TransformDirection(leftRay), rayCastSideDist, layerMask).collider;
        Collider2D raycastColliderLeft2 = Physics2D.Raycast(transform.position - new Vector3(0,1.8f,0), transform.TransformDirection(leftRay), rayCastSideDist, layerMask).collider;
        Collider2D raycastColliderRight1 = Physics2D.Raycast(transform.position, transform.TransformDirection(rightRay), rayCastSideDist, layerMask).collider;
        Collider2D raycastColliderRight2 = Physics2D.Raycast(transform.position - new Vector3(0, 1.8f, 0), transform.TransformDirection(rightRay), rayCastSideDist, layerMask).collider;
        if (raycastColliderLeft1 != null || raycastColliderLeft2 != null) {
            touchingLeftWall = true;
            touchingRightWall = false;
            return true;
        }
        else if ( raycastColliderRight1 != null || raycastColliderRight2 != null) {
            touchingRightWall = true;
            touchingLeftWall = false;
            return true;
        } else {
            touchingLeftWall = false;
            touchingRightWall = false;
            return false;
        }
    }

    private Vector2 getForward() {
        return (spriteRenderer.flipX ? -1 : 1) * new Vector2(1,0);
    }

    public void Move(float speed)
    {
        if (Math.Abs(GetSpeedX()) > MAXSPEEDX)
        {
            if( isGrounded )
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

    
    public void JumpPressed()
    {
        if( !isGrounded && touchingLeftWall )
        {
            //left wall jump
            wallJump("left");
        } else if( !isGrounded && touchingRightWall ) 
        {
            //right wall jump
            wallJump("right");
        }else if ( isGrounded )
        {
            Jump();
        }
    }
    public void Jump()
    {
        fullHopPoint = new Vector3(transform.position.x, transform.position.y + fullHopJumpHeight, transform.position.z);
        rigidBody.AddForce(new Vector2(0, jumpForce));
        isJumping = true;
        playSound(jumpClip);
    }
    public void ContinueJump()
    {
        if (isJumping) {
            if (transform.position.y < fullHopPoint.y && rigidBody.velocity.y > 0) {
                rigidBody.AddForce(new Vector2(0, jumpForceHeld));
            }
            else {
                isJumping = false;
            }
        }
    }
    public void wallJump( string dir )
    {
        isJumping = true;
        touchingLeftWall = false;
        touchingRightWall = false;
        if ( dir == "right" ) {
            rigidBody.AddForce(new Vector2(-600.0f, 650.0f));
        } else if ( dir == "left" ) {
            rigidBody.AddForce(new Vector2(600.0f, 650.0f));
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

    public void Shoot(){
        if (pCharge.charge < 5) return;     
        int p = (int) pCharge.charge / 20;
        float xdirection = getForward().x;
        float power = pCharge.charge;

        if (p == 0)
            power = 2;
        else if (p == 2)
            power = 0.6f * pCharge.charge;
        else if (p == 5)
            power = 101;

        ProjectileController projectile = Instantiate (
            projectiles[p],
            transform.position + new Vector3(xdirection,0,-3),
            transform.rotation
        ); 
        projectile.Shoot(
            getForward(),
            projectileProps[p, 0],
            projectileProps[p, 1],
            power,
            playerNum,
            xdirection
        );

         if (p == 2) {
            ProjectileController projectileUpper = Instantiate (
                projectiles[p],
                transform.position + new Vector3(xdirection,0,-3),
                Quaternion.Euler(0, 0, 20 * xdirection)
            );
            projectileUpper.Shoot(
                new Vector2(xdirection, 0.446f),
                projectileProps[p,0],
                projectileProps[p,1],
                power,
                playerNum,
                xdirection
            );
            ProjectileController projectileLower = Instantiate (
                projectiles[p],
                transform.position + new Vector3(xdirection,0,-3),
                Quaternion.Euler(0, 0, -20 * xdirection)
            );
            projectileLower.Shoot(
                new Vector2(xdirection, -0.446f),
                projectileProps[p,0],
                projectileProps[p,1],
                power,
                playerNum,
                xdirection
            );
         }
/*
        if (p == 2) {
            ProjectileController projectileUpper = Instantiate (
                projectiles[p],
                transform.position + new Vector3(xdirection,0,-3),
                Quaternion.Euler(0, 0, 20 * xdirection)
            );
            
            projectileUpper.GetComponent<SpriteRenderer>().flipX = spriteRenderer.flipX;
            projectileUpper.tag = "projectile";
            Destroy(projectileUpper.gameObject, projectileProps[p, 1]);

            ProjectileController projectileLower = Instantiate (
                projectiles[p],
                transform.position + new Vector3(xdirection,0,-3),
                Quaternion.Euler(0, 0, -20 * xdirection)
            );
            projectileLower.Shoot(new Vector2(xdirection, -0.446f), projectileProps[p,0]);
            projectileLower.GetComponent<SpriteRenderer>().flipX = spriteRenderer.flipX;
            projectileLower.tag = "projectile";
            Destroy(projectileLower.gameObject, projectileProps[p, 1]);
        }
        */
        if (p == 0)
            pCharge.charge -= 2;
        else
            pCharge.charge = 0;

        //Play sound
        switch (p) {
            case 0:
                playSound(tiddlerClip);
                break;
            case 1:
                playSound(kiBlastClip);
                break;
            case 2:
                playSound(shotgunClip);
                break;
            case 3:
                playSound(sniperClip);
                break;
            case 4:
                playSound(blastClip);
                break;
            case 5:
                playSound(PDClip);
                break;
        }
        // Animation
        animations.ShootAnim();
    }

    public void FlipSlashHitBox()
    {
        // Uncomment for slash hitbox
        /*
        slashHitBox.transform.localPosition = new Vector2(-slashHitBox.transform.localPosition.x, slashHitBox.transform.localPosition.y);
        slashHitBox.transform.localScale = new Vector2(-slashHitBox.transform.localScale.x, slashHitBox.transform.localScale.y);
        */
    }


    public void playSound(AudioClip clipToPlay) {
        audioSource.clip = clipToPlay;
        audioSource.Play();
    }

    public void playTurnSound() {
        audioSource.clip = turnClip;
        audioSource.Play();
    }

}
