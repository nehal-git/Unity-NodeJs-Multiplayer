using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerName : MonoBehaviour
{
    public static PlayerName instance;
    public  string str;
    public Text nametxt;
    public GameObject canvas;


    private void Awake()
    {
        instance = this;
    }
    public void Gamed()
    {
        str = nametxt.text;
      //  canvas.gameObject.SetActive(false);
    
    }





}
