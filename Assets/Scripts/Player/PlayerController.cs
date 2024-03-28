using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController charController;
    public GameManager gameManager;
    public float playerSpeed = 5f;
    public float jumpForce = 5f;
    public bool isAction;

    float horizontalMovement;
    float verticalMovement;

    // Gravity
    public Transform gravitySphere;
    Vector3 gravityVector;
    public float gravity;
    public float gravitySphereRadius;
    public bool isGrounded;
    public LayerMask groundMask, npcLayer;

    // SFX
    [SerializeField] AudioSource[] footStepsSFX;
    int footStepsIndex = 0;
    float footStepSpeed;
    bool playerMoved;

    // Start is called before the first frame update
    void Start()
    {
        charController = GetComponent<CharacterController>();
        gameManager = FindFirstObjectByType<GameManager>();
        gameManager.ListAllNPCsAdded(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAction)
        {
            CharacterMovements();
            Jump();
            ParentQueryText();
            FootStep();
        }
        Gravity();
    }

    void CharacterMovements()
    {
        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");

        Vector3 moveVector = (horizontalMovement * transform.right + verticalMovement * transform.forward).normalized;
        charController.Move(moveVector * playerSpeed * Time.deltaTime);
    }

    void Gravity()
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
        
    }

    void Jump()
    {
        // Jump 
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isAction)
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
        
    }

    void FootStep()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            footStepSpeed = .375f;
        }
        else
        {
            footStepSpeed = .65f;
        }

        if ((horizontalMovement != 0 || verticalMovement != 0) && !playerMoved && isGrounded)
        {
            footStepsSFX[footStepsIndex].Play();
            playerMoved = true;
            Invoke(nameof(PlayerMovementBool), footStepSpeed);

            footStepsIndex++;
            if (footStepsIndex > 1)
            {
                footStepsIndex = 0;
            }
        }

    }

    void PlayerMovementBool()
    {
        playerMoved = false;
    }

    IEnumerator FootSteps()
    {
        yield return new WaitForSeconds(.5f);
    }
}