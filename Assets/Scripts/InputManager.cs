using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerInput input;
    public static InputManager Instance;

    public Vector2 movementInput;

    public bool jump_Input;


    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        AllInputs();
    }

    private void OnEnable()
    {
        if (input == null)
        {
            input = new PlayerInput();

            input.Main.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();

            input.Actions.Jump.performed += ctx => jump_Input = true;

        }

        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    public void AllInputs()
    {
        //InputVoids
        JumpInput();
    }

    private void JumpInput() { if (jump_Input) Invoke(nameof(JumpReset), .01f); }

    private void JumpReset() { jump_Input = false; }

}
