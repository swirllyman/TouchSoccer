using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController_PointAndClick : InputController_Base
{
    [SerializeField] LayerMask hitMask;
    [SerializeField] Vector2 minMaxMoveDistance = new Vector3(2.0f, 4.0f);

    bool isMoving = false;

    Ray worldRay;
    RaycastHit hit;

    public override event OnDash onDash;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        if (isMoving)
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 worldHit = Vector3.zero;

            mousePos.z = 10;
            worldRay = Camera.main.ScreenPointToRay(mousePos);
            if (Physics.Raycast(worldRay, out hit, Mathf.Infinity, hitMask))
            {
                worldHit = hit.point;
            }

            moveDir = (worldHit - transform.position).normalized;


            float hitPointDistance = Vector3.Distance(hit.point, transform.position);
            movePerc = Utils.GetPerc(minMaxMoveDistance.x, minMaxMoveDistance.y, hitPointDistance);
        }
        else
        {
            moveDir = Vector3.zero;
        }
    }

    void GetInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isMoving = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isMoving = false;
            Dash();
        }
    }

    public override void Dash()
    {
        onDash?.Invoke();
    }

    private void OnDisable()
    {
        playerMovement.UpdateInputController();
    }
}
