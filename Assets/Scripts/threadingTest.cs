using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

public class threadingTest : MonoBehaviour
{
    private Thread thread;
    void Start()
    {
        string s = "";
        thread = new Thread(threadMethod);
        thread.Start();
        
    }

    //private ParameterizedThreadStart threadMethod()
    //{
      
    //}

    private void threadMethod()
    { 
    
    
    }
    
}
