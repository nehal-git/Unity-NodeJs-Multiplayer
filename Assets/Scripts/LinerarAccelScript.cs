using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinerarAccelScript : MonoBehaviour
{
    float speed = 10.0f;

    public Text acceltext;
    void Update()
    {
        Vector3 dir = Vector3.zero;

        // we assume that device is held parallel to the ground
        // and Home button is in the right hand

        // remap device acceleration axis to game coordinates:
        //  1) XY plane of the device is mapped onto XZ plane
        //  2) rotated 90 degrees around Y axis
        dir.x = Input.acceleration.x * 10;
        dir.y = Input.acceleration.y * 10;
        dir.z = Input.acceleration.z * 10;

        // clamp acceleration vector to unit sphere


        if (dir.z > 5)
        {


            // Move object
            acceltext.text = "X: " + dir.x.ToString() + ", Y: " + dir.y.ToString() + ", Z: " + dir.z.ToString();
        }
    }
}
