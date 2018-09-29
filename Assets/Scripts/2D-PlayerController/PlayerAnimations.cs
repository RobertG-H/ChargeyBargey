using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private PlayerController controller;
    private PlayerStateController stateController;
    private Animator anim;
    private SpriteRenderer spriteRend;


    // Use this for initialization
    void Start()
    {
        controller = gameObject.GetComponent<PlayerController>();
        stateController = gameObject.GetComponent<PlayerStateController>();
        anim = gameObject.GetComponent<Animator>();
        spriteRend = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetInteger("currentState", stateController.currentState);
        anim.SetFloat("speedX", Math.Abs(controller.GetSpeedX() * 0.1f));
        //Anim.SetBool("slash", playerStateController.slash);
        if ((controller.GetSpeedX() > 0 && spriteRend.flipX) || (controller.GetSpeedX() < 0 && !spriteRend.flipX) && (stateController.currentState != (int)PlayerStateController.state.FALLING && stateController.currentState != (int)PlayerStateController.state.JUMPING))
        {
            spriteRend.flipX = !spriteRend.flipX;
            controller.FlipSlashHitBox();
        }
    }

    public void SlashAnim()
    {
        anim.SetTrigger("slash");
    }
}
