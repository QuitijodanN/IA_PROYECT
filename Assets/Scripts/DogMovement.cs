using UnityEngine;

public class DogMovement : MonoBehaviour
{
    public float moveSpeed = 5f;     // Movement speed of the character
    public float rotationSpeed = 10f; // Rotation smoothness
    public Camera playerCamera;      // The camera following the player
    public Animator animator;

    private Vector3 moveDirection;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Get the input from WASD or arrow keys
        float horizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right
        float vertical = Input.GetAxis("Vertical");     // W/S or Up/Down

        // Get the direction based on camera forward direction
        Vector3 cameraForward = playerCamera.transform.forward;
        Vector3 cameraRight = playerCamera.transform.right;

        // Flatten the camera direction on the Y-axis to avoid flying characters
        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        // Calculate movement direction
        moveDirection = cameraForward * vertical + cameraRight * horizontal;
        moveDirection.Normalize();

        // Move the character based on input
        if (moveDirection != Vector3.zero)
        {
            // Rotate the character smoothly towards the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Move the character
            rb.MovePosition(transform.position + moveDirection * moveSpeed * Time.deltaTime);
        }
        UpdateAnimation();
    }


    void UpdateAnimation()
    {
            // Update the animator's "Walking" boolean parameter
            animator.SetBool("Walking", moveDirection != Vector3.zero);
    }
}