using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerManager : MonoBehaviour
{
    [Header("Player Settings")]
    public float speedForce;
    public float torqueForce;
    public Rigidbody rb;
    public GameObject Cameraob,offset;
    public LookAtConstraint lookAt;
    public RotationConstraint rotAt;


    [Header("Class Reference")]
    public NetworkIdentity networkIdentity;

    private void Awake()
    {
        var look = new ConstraintSource { sourceTransform = Camera.main.transform, weight=-1 };
        var rot = new ConstraintSource { sourceTransform = Camera.main.transform, weight = 1 };
        lookAt.SetSource(0,look);
        lookAt.worldUpObject= look.sourceTransform;
        
        rotAt.SetSource(0, rot);

       
        }
    private void Update()
    {
        if (networkIdentity.IsControlling())
        {
            playerMovement();
            if(Cameraob==null)
            {
                Cameraob = Camera.main.gameObject;

            }
                
            

            Cameraob.transform.position = new Vector3(offset.transform.position.x,Cameraob.transform.position.y,offset.transform.position.z);
        }
       
    }

    private void playerMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v= Input.GetAxis("Vertical");

       // rb.AddForce(new Vector3(h, 0, v) * speedForce *Time.deltaTime);
        rb.AddTorque(new Vector3(v, 0, -1*h) * torqueForce* Time.deltaTime);

    }
}
