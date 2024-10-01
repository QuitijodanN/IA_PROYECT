using UnityEngine;

public class DogMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;  // Speed for smooth turning
    public Transform cameraTransform;
    public Animator animator;          // Reference to the Animator
    public Rigidbody rb;               // Reference to the Rigidbody

    private Vector3 moveDirection;
    private Vector3 smoothMoveDirection; // For smooth movement
                                         // If there is movement, set the "Walking" boolean to true, otherwise false
    private bool isWalking = false;
    private float targetSpeed = 0f;

    void Update()
    {
        HandleMovement();
        UpdateAnimation();
    }

    void HandleMovement()
    {
        // Get input for horizontal and vertical movement (WASD)
        float horizontal = Input.GetAxis("Horizontal"); // A (-1) / D (1)
        float vertical = Input.GetAxis("Vertical");     // W (1) / S (-1)

        // Get the forward direction of the camera
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Eliminate Y-axis movement to ensure movement on the horizontal plane
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        // Determine target movement direction based on camera orientation and player input
        Vector3 targetDirection = forward * vertical + right * horizontal;
        targetSpeed = targetDirection.magnitude;
        if (targetSpeed > 1) targetSpeed = 1f;
        targetDirection.Normalize();
        

        // Smoothly interpolate the movement direction
        smoothMoveDirection = Vector3.Lerp(smoothMoveDirection, targetDirection, Time.deltaTime * rotationSpeed);
        smoothMoveDirection.Normalize();

        // Smoothly rotate the character to face the direction of movement
        if (smoothMoveDirection != Vector3.zero) {
            Quaternion targetRotation = Quaternion.LookRotation(smoothMoveDirection);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * rotationSpeed));
        }
    }

    void FixedUpdate()
    {
        // Move the player using Rigidbody in FixedUpdate for smooth physics-based movement
        if (targetSpeed > 0) {
            rb.MovePosition(rb.position + smoothMoveDirection * targetSpeed * moveSpeed * Time.fixedDeltaTime);
            isWalking = true;
        }
        else {
            isWalking = false;
        }
    }

    void UpdateAnimation()
    {
        // Update the animator's "Walking" boolean parameter
        animator.SetBool("Walking", isWalking);
    }
}