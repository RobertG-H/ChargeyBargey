using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    private PlayerController controller;
    // Use this for initialization
    void Start()
    {
        controller = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            controller.Move(-1.0f);
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            controller.Move(1.0f);
        }
        else if (Input.GetAxisRaw("Horizontal") == 0)
        {
            controller.Stop();
        }

        if (Input.GetAxisRaw("Vertical") < 0)
        {
            controller.FastFall();
        }

        if (Input.GetButtonDown("Jump"))
        {
            controller.Jump();
        }
        else if (Input.GetButton("Jump"))
        {
            controller.ContinueJump();
        }

        // Uncomment for slash
        /*if (Input.GetButtonDown("Slash"))
        {
            controller.SlashStart();
        }*/

    }
}
