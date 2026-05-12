using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineInputAxisController cinemachineInput;
    private bool isRightClick;
    private void Update()
    {
        cinemachineInput.enabled = isRightClick;
        Cursor.lockState = isRightClick ? CursorLockMode.Locked : CursorLockMode.None;


    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        isRightClick = context.ReadValueAsButton();
    } 
}
