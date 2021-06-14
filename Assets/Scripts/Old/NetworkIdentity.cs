using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using UnityEngine.UI;

public class NetworkIdentity : MonoBehaviour
{
    [Header("Player Variable Values")]
    
    public string id,playerName;
    public Text nameText;
    public bool isControlling;

    private SocketIOComponent socket;

    private void Awake()
    {
        isControlling = false;
    }

    public void SetControllerID(string ID)
    {
        id = ID;

        isControlling = (NetworkClient.ClientID == ID) ? true : false;
        //if (NetworkClient.ClientID == ID)
        //{
        //    isControlling = true;

        //}
        //else
        //{
        //    isControlling = false;
        //}
        
    }
   
    public void SetNetworkName(string str)
    {
        playerName = str;
        nameText.text = playerName;
    
    }
    public void SetSocketReference(SocketIOComponent Socket) {

        socket = Socket;
    }
    public SocketIOComponent GetSocket() {

        return socket;
    }
     public string GetID()
    {

        return id;
    
    }
    public bool IsControlling()
    {
        return isControlling;
    
    }

   
}
