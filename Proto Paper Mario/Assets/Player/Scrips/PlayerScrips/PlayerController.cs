using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private Rigidbody rb = null;
    [SerializeField] private PlayerAnimation playerAnimation = null;
    [Space]
    [Header("Walking Speed")]
    [SerializeField] private float startSpeed = 3;
    [SerializeField] private float topSpeed = 10;
    [Space]
    [Range(0, 1)]
    [SerializeField] private float delaySpeed = 0.35f;
    [Space]
    [Header("Jumping and Gravity")]
    [SerializeField] private float jumpForce = 5;
    [Space]
    [SerializeField] private float coyoteTime = 0.1f;
    [Space]
    [SerializeField] Transform[] groundDetectionRays = null;
    [SerializeField] private float hightLimit = 10;
    [Space]
    [SerializeField] private float jumpingGravity = 1;
    [SerializeField] private float fallingGravity = 10;
    [Space]
    [SerializeField] private LayerMask jumpLayerMask;
    #endregion

    #region PRIVATE_FIELDS
    private Ray ray;
    private float walkingTimer = 0f;
    private float coyoteTimer = 0f;

    private bool inGround = false;
    #endregion

    #region CONSTANTS
    private const float FALLING_SPEED = 0.02f;
    private const float GRAVITY_MULTIPLIER = 100;
    private const float GLOBAL_GRAVITY = -9.81f;
    #endregion

    #region UNITY_METHODS
    private void Update()
    {        
        playerAnimation.RotateCharacter();
        JumpInteraction();

        DrawDebugLines();
    }

    private void FixedUpdate()
    {
        MovementInteraction();
        Gravity();
    }
    #endregion

    #region PRIVATE_METHODS
    private void MovementInteraction()
    {
        float dirHor = Input.GetAxis("Horizontal") ;
        float dirVer = Input.GetAxis("Vertical");

        playerAnimation.RunningAnimation(dirHor, dirVer);

        if (dirHor == 0 && dirVer == 0)
        {
            walkingTimer = 0;
            return;
        }

        walkingTimer += Time.deltaTime;
        float currentSpeed = (walkingTimer <= delaySpeed) ? startSpeed : topSpeed;

        dirHor *= (currentSpeed) * Time.deltaTime;        
        dirVer *= (currentSpeed) * Time.deltaTime;

        // Move the character using the camera direction
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        Vector3 forwardRelative = dirVer * cameraForward;
        Vector3 rightRelative = dirHor * cameraRight;

        Vector3 cameraRelativeMovement = forwardRelative + rightRelative;

        rb.MovePosition(transform.position + cameraRelativeMovement);
    }

    private void JumpInteraction()
    {
        if(!inGround && coyoteTimer > coyoteTime)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerAnimation.JumpingAnimation();
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void Gravity()
    {
        for (int i = 0; i < groundDetectionRays.Length; i++)
        {
            ray = new Ray(groundDetectionRays[i].position, Vector3.down);

            if (Physics.Raycast(ray, out RaycastHit hit, hightLimit, jumpLayerMask))
            {
                inGround = true;
                break;
            }
            else
            {
                inGround = false;
            }
        }

        bool positiveVelocity = rb.velocity.y >= 0;

        if (!inGround && !positiveVelocity)
        {
            playerAnimation.FallingAnimation(true);
            coyoteTimer += Time.deltaTime;
        }
        else
        {
            playerAnimation.FallingAnimation(false);
            coyoteTimer = 0;
        }

        float usedGravity = (positiveVelocity) ? fallingGravity * GRAVITY_MULTIPLIER : jumpingGravity * GRAVITY_MULTIPLIER;
        Vector3 gravityVector = Vector3.up * GLOBAL_GRAVITY * usedGravity * Time.deltaTime;
        
        rb.AddForce(gravityVector, ForceMode.Force);
    }
    #endregion

    #region DEBUG_METHODS
    private void DrawDebugLines()
    {
        for (int i = 0; i < groundDetectionRays.Length; i++)
        {
            Debug.DrawLine(groundDetectionRays[i].position, groundDetectionRays[i].position - new Vector3(0, hightLimit, 0), Color.blue);
        }
    }
    #endregion
}