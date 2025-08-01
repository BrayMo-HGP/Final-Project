using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;
    private PlayerMotor motor;
    private PlayerLook look;

    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();

        if (motor != null && onFoot.Jump != null)
            onFoot.Jump.performed += ctx => motor.Jump();
        else
            Debug.LogWarning("Motor or Jump action not assigned.");
    }

    void FixedUpdate()
    {
        if (onFoot.Movement != null && motor != null)
        {
            Vector2 input = onFoot.Movement.ReadValue<Vector2>();
            motor.ProcessMove(input);
        }
    }

    void LateUpdate()
    {
        if (onFoot.Look != null && look != null)
        {
            Vector2 lookInput = onFoot.Look.ReadValue<Vector2>();
            look.ProcessLook(lookInput);
        }
    }

    private void OnEnable()
    {
        onFoot.Enable(); // no null check needed for struct
    }

    private void OnDisable()
    {
        onFoot.Disable();
    }
}
