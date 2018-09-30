using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    private PlayerController controller;
    [SerializeField]
    private int playerNum;

    // Use this for initialization
    void Start()
    {
        controller = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!controller.IsDead) {
            if (Input.GetAxisRaw("Horizontal" + playerNum.ToString()) < 0) {
                controller.Move(-1.0f);

            }
            else if (Input.GetAxisRaw("Horizontal" + playerNum.ToString()) > 0) {
                controller.Move(1.0f);
            }
            else if (Input.GetAxisRaw("Horizontal" + playerNum.ToString()) == 0) {
                controller.Stop();
            }

            if (Input.GetAxisRaw("Vertical" + playerNum.ToString()) < 0) {
                controller.FastFall();
            }

            if (Input.GetButtonDown("Jump" + playerNum.ToString())) {
                controller.JumpPressed();
            }
            else if (Input.GetButton("Jump" + playerNum.ToString())) {
                controller.ContinueJump();
            }

            if (Input.GetButtonDown("Fire" + playerNum.ToString())) {
                controller.Shoot();
            }
        }

    }

}
