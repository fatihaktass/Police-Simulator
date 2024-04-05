using UnityEngine;
using UnityEngine.UI;

public class PlayerAnimations : MonoBehaviour
{
    public Animator animator;
    public PlayerController playerController;
    public Camera playerCamera;
    GameStatistics gameStatistics;

    // Player Energy
    public float currentValue = 20f;
    public float increaseInterval = 20f;
    static float runningSpeed;
    static float walkingSpeed;
    float increaseAmount = 1f;
    bool isPlayerRunning = false;
    public bool playerCantRun = false;
    [SerializeField] Slider energySlider;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponentInParent<PlayerController>();
        gameStatistics = FindAnyObjectByType<GameStatistics>();

        PlayerDatas();
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
            playerController.playerSpeed = runningSpeed;
            animator.SetBool("isRunning", true);
            isPlayerRunning = true;
            currentValue = Mathf.MoveTowards(currentValue, -increaseInterval, increaseAmount * Time.deltaTime);
            energySlider.value = currentValue;
        }
        else
        {
            animator.SetBool("isRunning", false);
            playerController.playerSpeed = walkingSpeed;
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

    public float SetRunningSpeed()
    {
        return runningSpeed;
    }

    public float SetWalkingSpeed()
    {
        return walkingSpeed;
    }

    void PlayerDatas()
    {
        float _rankPoints = gameStatistics.GetRankPoints();

        if (_rankPoints < 2000)
        {
            walkingSpeed = 3.2f;
            runningSpeed = 5.4f;
            increaseInterval = 20f;
            currentValue = 20f; 
        }
        else if (_rankPoints >= 2000 && _rankPoints < 4250)
        {
            walkingSpeed = 3.2f;
            runningSpeed = 5.4f;
            increaseInterval = 24f;
            currentValue = 24f;
        }
        else if (_rankPoints >= 4250 && _rankPoints < 6200)
        {
            walkingSpeed = 3.5f;
            runningSpeed = 6f;
            increaseInterval = 24f;
            currentValue = 24f;
        }
        else if (_rankPoints >= 6200 && _rankPoints < 10000)
        {
            walkingSpeed = 4f;
            runningSpeed = 6.4f;
            increaseInterval = 25f;
            currentValue = 25f;
        }
        else if (_rankPoints >= 10000)
        {
            walkingSpeed = 4.2f;
            runningSpeed = 6.6f;
            increaseInterval = 28f;
            currentValue = 28f;
        }

        energySlider.maxValue = currentValue;
    }
}
