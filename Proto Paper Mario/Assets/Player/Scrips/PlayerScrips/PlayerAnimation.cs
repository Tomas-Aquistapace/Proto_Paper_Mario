using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private Animator animator;
    [SerializeField] private float rotationSpeed;
    #endregion

    #region PRIVATE_FIELDS
    private bool left;
    private bool right;
    private bool lastDirection = true;

    private float leftNumber = ROTATION;
    private float rightNumber = ROTATION;

    private const float ROTATION = 180;
    #endregion

    #region PRIVATE_METHODS
    public void RunningAnimation(float horizontal, float vertical)
    {
        float greater = Mathf.Max(Mathf.Abs(horizontal), Mathf.Abs(vertical));
        animator.SetFloat("Speed", greater);
    }

    public void JumpingAnimation()
    {
        animator.SetTrigger("JumpingTrigger");
    }

    public void FallingAnimation(bool isFalling)
    {
        animator.SetBool("IsFalling", isFalling);
    }

    public void RotateCharacter()
    {
        if (Input.GetKeyDown("a") || left == true)
        {
            if (lastDirection == true)
            {
                if (leftNumber > 0)
                {
                    transform.Rotate(new Vector3(0f, rotationSpeed, 0f) * Time.deltaTime);
                    left = true;

                    leftNumber -= rotationSpeed * Time.deltaTime;
                }
                else
                {
                    left = false;
                    leftNumber = ROTATION;

                    transform.eulerAngles = new Vector3(0, 180, 0);

                    lastDirection = false;
                }
            }
        }
        else if (Input.GetKeyDown("d") || right == true)
        {
            if (lastDirection == false)
            {
                if (rightNumber > 0)
                {
                    transform.Rotate(new Vector3(0f, -rotationSpeed, 0f) * Time.deltaTime);
                    right = true;

                    rightNumber -= rotationSpeed * Time.deltaTime;
                }
                else
                {
                    right = false;
                    rightNumber = ROTATION;

                    transform.eulerAngles = new Vector3(0, 0, 0);

                    lastDirection = true;
                }
            }
        }
    }
    #endregion
}