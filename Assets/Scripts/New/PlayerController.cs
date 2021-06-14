using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public bool isLocalPlayer = false;
    public float speedForce;
    public float torqueForce;
    public Rigidbody rb;
    Vector3 oldPos, currentPos;
    Quaternion oldRot, currentRot;

   
    void Start()
    {
        oldPos = transform.position;
        currentPos = oldPos;
        oldRot = transform.rotation;
        currentRot = oldRot;
    }

  
    void Update()
    {
        if (isLocalPlayer)
        {
            playerMovement();
        }
       



    }

    private void playerMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        rb.AddForce(new Vector3(h, 0, v) * speedForce * Time.deltaTime);
        rb.AddTorque(new Vector3(v, 0, -1 * h) * torqueForce * Time.deltaTime);

        if (currentPos != oldPos)
        {
            oldPos = currentPos;
        
        }
        if (currentRot != oldRot)
        {
            oldRot = currentRot;
        
        }



    }
}
