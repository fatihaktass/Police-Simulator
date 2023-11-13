using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController charController;
    public GameManager gameManager;
    public float playerSpeed = 5f;
    public float jumpForce = 5f;


    // Gravity
    public Transform gravitySphere;
    Vector3 gravityVector;
    public float gravity;
    public float gravitySphereRadius;
    bool isGrounded;
    public LayerMask groundMask,npcLayer;

    // Start is called before the first frame update
    void Start()
    {
        charController = GetComponent<CharacterController>();
        gameManager = FindFirstObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CharacterMovements();
        GravityAndJump();
        ParentQueryText();
    }

    void CharacterMovements()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        Vector3 moveVector = (horizontalMovement * transform.right + verticalMovement * transform.forward).normalized;
        charController.Move(moveVector * playerSpeed * Time.deltaTime);
    }

    void GravityAndJump()
    {
        // Ground Check
        isGrounded = Physics.CheckSphere(gravitySphere.position, gravitySphereRadius, groundMask);

        // Gravity
        gravityVector.y += gravity * Time.deltaTime;
        charController.Move(gravityVector * Time.deltaTime);

        if (isGrounded && gravityVector.y < 0)
        {
            gravityVector.y = -3f;
        }

        // Jump 
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            gravityVector.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    void ParentQueryText()
    {
        bool isInteracting = Physics.CheckSphere(transform.position, 2.4f, npcLayer);
        gameManager.QueryText(isInteracting);
    }
}
