using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float smoothMoveTime = .1f;
    public float turnSpeed = 8f;
 
    float smoothInputMagnitude;
    float smoothMoveVelocity;
    float angle;
    bool disabled;
    Vector3 velocity;
 
    Rigidbody rb;
 
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Guard.OnGuardHasSpottedPlayer += Disable;
    }
 
    void Update()
    {
        Vector3 inputDirection = Vector3.zero;
        if(!disabled)
        {
            inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        }
        
        float inputMagnitude = inputDirection.magnitude;
        smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, inputMagnitude, ref smoothMoveVelocity, smoothMoveTime);
 
        float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
        angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed * inputMagnitude);
 
        velocity = transform.forward * moveSpeed * smoothInputMagnitude;
    }

    void Disable()
    {
        disabled = true;
    }
 
    void FixedUpdate()
    {
        rb.MoveRotation(Quaternion.Euler(Vector3.up * angle));
        rb.MovePosition(rb.position + velocity * Time.deltaTime);
    }

    void OnDestroyMethod()
    {
        Guard.OnGuardHasSpottedPlayer -= Disable;
    }
}
