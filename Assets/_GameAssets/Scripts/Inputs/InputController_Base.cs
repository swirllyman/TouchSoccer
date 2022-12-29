using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputController_Base : MonoBehaviour
{
    public Vector3 moveDir { get; protected set; }
    public float movePerc { get; protected set; }
    public PlayerMovement playerMovement;
    public abstract void Dash();


    public delegate void OnDash();
    public abstract event OnDash onDash;
}
