using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float dashMaxHoldDownTime = 1.0f;
    [SerializeField] float minDashForce = 500.0f;
    [SerializeField] float maxDashForce = 1000.0f;
    [SerializeField] float minDashMag = 10.0f;
    [SerializeField] float maxDashMag = 20.0f;
    [SerializeField] float moveSpeed = 10;
    [SerializeField] Image dashFillImage;
    [SerializeField] Vector2 minMaxMoveDistance = new Vector3(2.0f, 4.0f);
    [SerializeField] ForceMode movementForceMode;
    [SerializeField] LayerMask hitMask;
    Rigidbody myBody;
    bool move = false;
    Vector3 moveDirection;
    Vector3 worldPos;
    RaycastHit hit;

    List<Vector3> currentVelocity = new List<Vector3>();
    Vector3 avgVel;
    int currentIdx = 0;
    int totalFramesSampled = 5;

    float dashTimer = 0.0f;
    float dashPerc = 0.0f;
    float movePerc = 0.0f;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        UpdateMoveDirection();

        ShowDebug();

    }

    private void FixedUpdate()
    {
        if (move)
        {
            myBody.AddForce(moveDirection * moveSpeed * movePerc, movementForceMode);
        }
    }

    void ShowDebug()
    {

        for (int i = 1; i < currentVelocity.Count; i++)
        {
            Debug.DrawLine(currentVelocity[i - 1], currentVelocity[i], Color.red, .25f);
        }

        //foreach(Vector3 point in currentVelocity)
        //{
        //    Debug.DrawLine()
        //}
    }

    void UpdateMoveDirection()
    {
        movePerc = 0.0f;

        if (move)
        {
            dashTimer += Time.deltaTime;
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10;
            Ray worldRay = Camera.main.ScreenPointToRay(mousePos);

            if (Physics.Raycast(worldRay, out hit, Mathf.Infinity, hitMask))
            {
                worldPos = hit.point;
            }

            float hitPointDistance = Vector3.Distance(hit.point, transform.position);

            movePerc = Utils.GetPerc(minMaxMoveDistance.x, minMaxMoveDistance.y, hitPointDistance);

            currentVelocity.Add(worldPos);
            moveDirection = (worldPos - transform.position).normalized;

            if (currentVelocity.Count > totalFramesSampled)
            {
                currentVelocity.RemoveAt(0);
            }
            currentIdx = (currentIdx + 1) % totalFramesSampled;
            transform.LookAt(worldPos, Vector3.up);
            //Debug.DrawRay(transform.position + transform.up * 1.0f, moveDirection, Color.red);
            SetAvgVelocity();
        }

        dashPerc = Mathf.Clamp01(dashTimer / dashMaxHoldDownTime);
        dashFillImage.fillAmount = dashPerc;
    }

    void CheckInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            move = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            move = false;
            currentVelocity.Clear();
            if (avgVel.magnitude > minDashMag)
            {
                Dash();
            }

            dashTimer = 0.0f;
        }
    }

    void SetAvgVelocity()
    {
        avgVel = Vector3.zero;
        foreach(Vector3 v in currentVelocity)
        {
            avgVel += v;
        }

        avgVel /=  currentVelocity.Count;
    }

    void Dash()
    {

        //float mag = avgVel.magnitude > maxDashMag ? maxDashMag : avgVel.magnitude;
        //float dashPerc = (mag - minDashMag) / (maxDashMag - minDashMag);
        float dashSpeed = Mathf.Lerp(minDashForce, maxDashForce, dashPerc);
        myBody.AddForce(moveDirection * dashSpeed, ForceMode.Impulse);
        //Debug.Log("Avg Mag: " + mag);
        //Debug.Log("Dashing at perc: " + dashPerc);
    }
}
