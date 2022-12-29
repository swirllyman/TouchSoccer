using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float dashRechargeTime = 1.0f;
    [SerializeField] float minDashForce = 500.0f;
    [SerializeField] float maxDashForce = 1000.0f;
    [SerializeField] float minDashMag = 10.0f;
    [SerializeField] float maxDashMag = 20.0f;
    [SerializeField] float moveSpeed = 10;
    [SerializeField] Image dashFillImage;
    [SerializeField] ForceMode movementForceMode;

    float dashTimer = 0.0f;
    float dashPerc = 0.0f;

    Rigidbody myBody;
    InputController_Base inputController;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody>();
        inputController = GetComponent<InputController_Base>();
        inputController.onDash += Dash;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMoveDirection();

    }

    private void FixedUpdate()
    {
        myBody.AddForce(inputController.moveDir * moveSpeed * inputController.movePerc, movementForceMode);
    }

    public void UpdateInputController()
    {
        inputController.onDash -= Dash;
        inputController = GetComponent<InputController_Base>();
        inputController.onDash += Dash;
    }

    void UpdateMoveDirection()
    {
        dashTimer += Time.deltaTime;
        dashPerc = Mathf.Clamp01(dashTimer / dashRechargeTime);
        dashFillImage.fillAmount = dashPerc;

        transform.LookAt(transform.position + inputController.moveDir);
    }

    void Dash()
    {
        float dashSpeed = Mathf.Lerp(minDashForce, maxDashForce, dashPerc);
        myBody.AddForce(inputController.moveDir * dashSpeed, ForceMode.Impulse);
        dashTimer = 0.0f;
    }
}
