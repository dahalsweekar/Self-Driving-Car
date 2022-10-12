using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public float carSpeed;
    public float rotationSpeed;
    private Rigidbody car_rb;
    public float rot_val;
    bool is_moving;

    void Start()
    {
        is_moving = false;
        car_rb = GetComponent<Rigidbody>();
    }


    void Update()
    {

        if (Input.GetKey(KeyCode.W))
        {
            car_rb.AddForce(transform.forward * carSpeed * Time.deltaTime * 2);
            is_moving = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            car_rb.AddForce(-transform.forward * carSpeed * Time.deltaTime * 2);
            is_moving = true;
        }


        if (car_rb.velocity.magnitude > 0.1)
        {
            is_moving = true;
        }
        else
        {
            is_moving = false;
        }

        if (is_moving) { 
        float turn = Input.GetAxis("Horizontal");
        transform.Rotate(0, turn * rotationSpeed * Time.deltaTime, 0);
        }

        is_moving = false;
    }
}
