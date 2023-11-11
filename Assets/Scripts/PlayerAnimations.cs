using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public Animator animator;
    public PlayerController playerController;
    public Camera playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("isRunning", true);
            playerController.playerSpeed = 6f;
        }
        else
        {
            animator.SetBool("isRunning", false);
            playerController.playerSpeed = 3.5f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && playerController.IsGrounded())
        {
            animator.SetBool("isGrounded", playerController.IsGrounded());
        }
        else
        {
            animator.SetBool("isGrounded", false);
        }
    }
}
