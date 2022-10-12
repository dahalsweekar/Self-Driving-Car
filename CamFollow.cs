using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    void Start()
    {
   
    }

    
    void Update()
    {
        transform.position = target.position + offset;
    }
}
