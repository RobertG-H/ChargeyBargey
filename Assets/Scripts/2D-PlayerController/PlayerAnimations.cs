using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public GameObject Head;
    public GameObject Body;
    public GameObject Face;
    public GameObject playerParticles;

    private PlayerController controller;
    private PlayerStateController stateController;
    private Animator mainAnim;
    private Animator headAnim;
    private Animator bodyAnim;
    private SpriteRenderer headSpriteRend;
    private SpriteRenderer bodySpriteRend;
    private SpriteRenderer faceSpriteRend;
    private ParticleSystem playerParticleSystem;
    ParticleSystem.EmissionModule emissionModule;
    ParticleSystem.ShapeModule shapeModule;

    private Vector3 playerParticlesRotation;
    private Vector3 particleTargetDir = new Vector3();

    // Use this for initialization
    void Start()
    {
        controller = gameObject.GetComponent<PlayerController>();
        stateController = gameObject.GetComponent<PlayerStateController>();
        mainAnim = GetComponent<Animator>();
        headAnim = Head.GetComponent<Animator>();
        bodyAnim = Body.GetComponent<Animator>();
        headSpriteRend = Head.GetComponent<SpriteRenderer>();
        bodySpriteRend = Body.GetComponent<SpriteRenderer>();
        faceSpriteRend = Face.GetComponent<SpriteRenderer>();
        playerParticleSystem = playerParticles.GetComponent<ParticleSystem>();
        emissionModule = playerParticleSystem.emission;
        shapeModule = playerParticleSystem.shape;
        playerParticlesRotation = new Vector3(270, 270, 0);
    }

    // Update is called once per frame
    void Update()
    {
        mainAnim.SetInteger("currentState", stateController.currentState);
        headAnim.SetInteger("currentState", stateController.currentState);
        headAnim.SetFloat("speedX", Math.Abs(controller.GetSpeedX() * 0.1f));

        bodyAnim.SetInteger("currentState", stateController.currentState);
        bodyAnim.SetFloat("speedX", Math.Abs(controller.GetSpeedX() * 0.1f));
        if ((controller.GetSpeedX() > 0 && headSpriteRend.flipX) || (controller.GetSpeedX() < 0 && !headSpriteRend.flipX))// && (stateController.currentState != (int)PlayerStateController.state.FALLING && stateController.currentState != (int)PlayerStateController.state.JUMPING))
        {
            headSpriteRend.flipX = !headSpriteRend.flipX;
            bodySpriteRend.flipX = !bodySpriteRend.flipX;
            faceSpriteRend.flipX = !faceSpriteRend.flipX;
            controller.playTurnSound();
        }
        updateParticles();
    }

    public void updateParticles() {
        float step = 6f * Time.deltaTime;
        // If player is moving right
        if (controller.GetSpeedX() > 0) {
            particleTargetDir =
                 new Vector3(-90, 40, playerParticles.transform.rotation.eulerAngles.z);
        }
        else if (controller.GetSpeedX() < 0) {
            particleTargetDir =
                new Vector3(120, 40, playerParticles.transform.rotation.eulerAngles.z);
        }
        else {
           particleTargetDir =
                new Vector3(0, 270, playerParticles.transform.rotation.eulerAngles.z);
        }

        // Rotate towards directions
        Vector3 newDir = Vector3.RotateTowards(playerParticlesRotation, particleTargetDir, step, 2.0f);
        playerParticlesRotation = newDir;
        playerParticles.transform.rotation = Quaternion.LookRotation(newDir);

        emissionModule.rateOverTime = controller.GetCharge()*2;
        shapeModule.angle = controller.GetCharge() / 4 + 13;

    }


    //public void ShootAnim() {
    //    headAnim.SetTrigger("shoot");
    //    bodyAnim.SetTrigger("shoot");
    //}

    public void HidePlayer() {
        playerParticles.SetActive(false);
        mainAnim.SetTrigger("hide");
    }

}
