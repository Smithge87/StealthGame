using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 7;
    public float smoothMoveTime = .1f;
    public float turnSpeed = 8;

    float angle;
    float smoothMovementMagnitude;
    float smoothMoveVelocity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //-- take user input to make the box move and rotate
        Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
        //-- magnitude keeps the block from moving forever. didn't need this previously, not sure why but otherwise the block keeps going x
        float inputMagnitude = inputDirection.magnitude;
        //-- creates smoother movements. applied to the movement below
        smoothMovementMagnitude = Mathf.SmoothDamp(smoothMovementMagnitude, inputMagnitude, ref smoothMoveVelocity, smoothMoveTime);
        //-- smooths the angle rotation.
        angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed * inputMagnitude);

        transform.eulerAngles = Vector3.up * angle;
        transform.Translate(transform.forward * moveSpeed * Time.deltaTime *smoothMovementMagnitude, Space.World);
    }
}
