using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float minDashForce = 500.0f;
    [SerializeField] float maxDashForce = 1000.0f;
    [SerializeField] float minDashMag = 10.0f;
    [SerializeField] float maxDashMag = 20.0f;
    [SerializeField] float moveSpeed = 10;
    [SerializeField] ForceMode movementForceMode;

    [SerializeField] Transform ball;
    [SerializeField] Transform cameraFollow;
    [SerializeField] Vector2 cameraFollowOffsetMinMax;
    [SerializeField] Vector2 ballDistanceMinMax;
    Rigidbody myBody;
    bool move = false;
    Vector3 moveDirection;
    Vector3 worldPos;
    RaycastHit hit;

    List<Vector3> currentVelocity = new List<Vector3>();
    Vector3 avgVel;
    int currentIdx = 0;
    int totalFramesSampled = 5;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
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
        }

        if (move)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10;
            Ray worldRay = Camera.main.ScreenPointToRay(mousePos);

            if(Physics.Raycast(worldRay, out hit, Mathf.Infinity))
            {
                worldPos = hit.point;
            }

            currentVelocity.Add(worldPos - transform.position);
            moveDirection = (worldPos - transform.position).normalized;
            if(currentVelocity.Count > totalFramesSampled)
            {
                currentVelocity.RemoveAt(0);
            }
            currentIdx = (currentIdx + 1) % totalFramesSampled;
            transform.LookAt(worldPos, Vector3.up);
            //Debug.DrawRay(transform.position + transform.up * 1.0f, moveDirection, Color.red);
            SetAvgVelocity();
        }
    }

    private void FixedUpdate()
    {
        if (move)
        {
            myBody.AddForce(moveDirection * moveSpeed, movementForceMode);
            float ballDistance = Vector3.Distance(transform.position, ball.position);
            float followOffsetPerc = 0;
            if (ballDistance >= ballDistanceMinMax.x)
            {
                ballDistance = Mathf.Clamp(ballDistance, ballDistanceMinMax.x, ballDistanceMinMax.y);
                followOffsetPerc = (ballDistance - ballDistanceMinMax.x) / (ballDistanceMinMax.y - ballDistanceMinMax.x);
            }
            cameraFollow.transform.position = transform.position + (ball.position - transform.position).normalized * Mathf.Lerp(cameraFollowOffsetMinMax.x, cameraFollowOffsetMinMax.y, followOffsetPerc);
        }
    }

    void SetAvgVelocity()
    {
        avgVel = Vector3.zero;
        foreach(Vector3 v in currentVelocity)
        {
            avgVel += v;
        }

        avgVel = avgVel / currentVelocity.Count;
        //currentVelocity
    }

    void Dash()
    {
        float mag = avgVel.magnitude > maxDashMag ? maxDashMag : avgVel.magnitude;
        //Debug.Log("Avg Mag: " + mag);
        float dashPerc = (mag - minDashMag) / (maxDashMag - minDashMag);
        float dashSpeed = Mathf.Lerp(minDashForce, maxDashForce, dashPerc);
        //Debug.Log("Dashing at perc: " + dashPerc);
        myBody.AddForce(avgVel.normalized * dashSpeed, ForceMode.Impulse);
    }
}
