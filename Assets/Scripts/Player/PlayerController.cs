using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Transform cameraTransform;
    private Vector2 moveInput;
    private Vector3 movementVector;
    private Animator animator;
    private bool isMoving = false;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        MovePlayer();
        MovementAnimation();
    }

    private void MovePlayer()
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 move = forward * moveInput.y + right * moveInput.x;

        transform.Translate(move * speed *  Time.deltaTime, Space.World);

        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }

    public void HandleMovement(InputAction.CallbackContext context)
    {
        if(context.performed) isMoving = true;
        if(context.canceled) isMoving = false;

        moveInput = context.ReadValue<Vector2>();
        movementVector = new Vector3(moveInput.x, 0, moveInput.y);
    }

    private void MovementAnimation()
    {
        if (isMoving) animator.SetBool("IsRunning", true);
        else animator.SetBool("IsRunning", false);
    }
}
