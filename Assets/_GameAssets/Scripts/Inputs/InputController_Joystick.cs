using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController_Joystick : InputController_Base
{
    public override event OnDash onDash;

    public override void Dash()
    {
        onDash?.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        movePerc = Mathf.Max(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    private void OnDisable()
    {
        playerMovement.UpdateInputController();
    }
}
