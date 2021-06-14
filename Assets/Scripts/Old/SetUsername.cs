using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUsername : MonoBehaviour
{
    [Header("References")]
    public NetworkIdentity networkIdentity;

    private PlayerData.Player player;

    private void Start()
    {

        player = new PlayerData.Player();
        player.username = new PlayerData.Username();
        if (networkIdentity.IsControlling())
        {
            sendData();


        }
       
    }
    private void Update()
    {
        
    }
    void sendData()
    {
        player.username._username = PlayerName.instance.str;
        networkIdentity.SetNetworkName(player.username._username);
        networkIdentity.GetSocket().Emit("setusername", new JSONObject(JsonUtility.ToJson(player)));

    }
}
