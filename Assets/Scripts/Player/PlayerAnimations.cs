using UnityEngine;
using UnityEngine.UI;

public class PlayerAnimations : MonoBehaviour
{
    public Animator animator;
    public PlayerController playerController;
    public Camera playerCamera;

    // Player Energy
    float currentValue = 20f;
    float increaseInterval = 20f;
    float increaseAmount = 1f;
    bool isPlayerRunning = false;
    public bool playerCantRun = false;
    [SerializeField] Slider energySlider;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponentInParent<PlayerController>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);

        if (Input.GetKeyDown(KeyCode.Space) && playerController.IsGrounded())
        {
            animator.SetBool("isJumping", playerController.IsGrounded());
        }
        else
        {
            animator.SetBool("isJumping", !playerController.IsGrounded());
        }

        if (Input.GetKey(KeyCode.LeftShift) && !playerCantRun)
        {
            playerController.playerSpeed = 6f;
            animator.SetBool("isRunning", true);
            isPlayerRunning = true;
            currentValue = Mathf.MoveTowards(currentValue, -increaseInterval, increaseAmount * Time.deltaTime);
            energySlider.value = currentValue;
        }
        else
        {
            animator.SetBool("isRunning", false);
            playerController.playerSpeed = 3.5f;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Tired();
        }
        if (!isPlayerRunning)
        {
            currentValue = Mathf.MoveTowards(currentValue, increaseInterval, increaseAmount * Time.deltaTime);
            energySlider.value = currentValue;
        }
        if (currentValue <= 0)
        {
            currentValue = 0;
            playerCantRun = true;
        }
        if (currentValue > 10)
        {
            playerCantRun = false;
        }
    }

    void Tired()
    {
        isPlayerRunning = false;
    }
}
