using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerNetworkData : MonoBehaviour
{
    [Header("References")] 
    public NetworkIdentity networkIdentity;
    private Vector3 oldPos;
    private Quaternion oldRot;

    private NetworkClient networkClient;
   
    private PlayerData.Player player;
    private float stillcounter = 0;

    private void Start()
    {
        oldPos = transform.position;
        oldRot = transform.rotation;
        networkClient = GameObject.Find("NetworkClient").GetComponent<NetworkClient>();
      
        player= new PlayerData.Player();
        player.position = new PlayerData.Position();
        player.rotation = new PlayerData.Rotation();
        player.spawned = new PlayerData.Spawned();
        player.room = new PlayerData.Room();
        player.username = new PlayerData.Username();
        player.position.x = 0;
        player.position.y = 0;
        player.position.z = 0;
        player.rotation.x = 0;
        player.rotation.y = 0;
        player.rotation.z = 0;

        if (!networkIdentity.IsControlling())
        {
            enabled = false;


        }
        sendSpawnData();
    }

    private void sendSpawnData()
    {
        if (networkIdentity.IsControlling())
        {

            player.spawned._spawned = "true";
            player.room._room = networkClient.roomID;
            player.username._username = PlayerName.instance.str;
            networkIdentity.SetNetworkName(player.username._username);
            networkIdentity.GetSocket().Emit("spawn", new JSONObject(JsonUtility.ToJson(player)));

        }
       
    }
    public void Update()
    {
        if (networkIdentity.IsControlling())
        {
            if (oldPos != transform.position)
            {


                oldPos = transform.position;
                stillcounter = 0;
                sendData();
            }
            else
            {

                stillcounter += Time.deltaTime;
                if (stillcounter >= 1)
                {
                    stillcounter = 0;
                    sendData();
                }
            }

        }

    }

    private void sendData()
    {
        player.position.x = Mathf.Round(transform.position.x * 1000.0f) / 1000.0f;
        player.position.y = Mathf.Round(transform.position.y * 1000.0f) / 1000.0f;
        player.position.z = Mathf.Round(transform.position.z * 1000.0f) / 1000.0f;

        player.rotation.x = Mathf.Round(transform.localEulerAngles.x * 1000.0f) / 1000.0f;
        player.rotation.y = Mathf.Round(transform.localEulerAngles.y * 1000.0f) / 1000.0f;

        player.rotation.z = Mathf.Round(transform.localEulerAngles.z * 1000.0f) / 1000.0f;
      //  player.rotation.z = transform.rotation.w;
        networkIdentity.GetSocket().Emit("transform", new JSONObject(JsonUtility.ToJson(player)));
       
       
       
    }
}
