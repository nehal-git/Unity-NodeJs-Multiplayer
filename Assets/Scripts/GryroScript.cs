using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GryroScript : MonoBehaviour
{

    private Gyroscope gyroscope;
    private Quaternion quaternion;
    private bool gyroBool;

    private void Start()
    {
        if (gyroBool)
            return;

        if (SystemInfo.supportsGyroscope)
        {
            gyroscope = Input.gyro;
            gyroscope.enabled = true;
            gyroBool = true;
        
        }
    }

    private void Update()
    {
        if (gyroBool)
        {
            Debug.Log(gyroscope.attitude);
        
        }
    }



}
