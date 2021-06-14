using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;
using UnityEngine.UI;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class NetworkClient : MonoBehaviour
{
    public SocketIOComponent io;
    [Header("UI")]
    public GameObject connectPanel;
    public Text IPText;
    public InputField noPlayer;
    [Header("Player Object")]
    public GameObject playerPrefab;



    [Header("Server Object Parent")]
    public Transform networkContainer;
   
    public GameObject WaitPanel, ConnectPanel;
    public string roomID;
    private string playerCount;
    public static string ClientID { get; private set; }


    private Dictionary<string, NetworkIdentity> serverObjects;


    public bool onConnected = false;

    public bool isAI;

    public bool isAllPlayersJoined;

    // Start is called before the first frame update

    private void Start()
    {
        IPText.text = GetIPAddress();
        
    }
    private void Initialize()
    {
        io.virtualAwake();
       
        serverObjects = new Dictionary<string, NetworkIdentity>();

    }


    public void OnConnectToServer()
    {
        connectPanel.SetActive(false);
        WaitPanel.SetActive(true);
      
        Initialize();

        setupEvents();

        StartCoroutine(ConnectedToServer());
    }

    IEnumerator ConnectedToServer()
    {

        yield return new WaitUntil(() => onConnected);
      
        io.Emit("create");
      
        

    }

    public void OnClick_ManualSpawn()//Manual Spawn
    {
       
            string id = ClientID;

            GameObject go = Instantiate(playerPrefab, networkContainer);
            go.name = "Player_ID: " + id;

            NetworkIdentity ni = go.GetComponent<NetworkIdentity>();
            ni.SetControllerID(id);

            ni.SetSocketReference(io);

            serverObjects.Add(id, ni);
           
       
    }

    //  Subscribed Events of the Opponet Player
    private void setupEvents()
    {

        io.On("open", OnConnected);

        io.On("register", OnRegister);
      
        io.On("spawn", OnSpawn);

        io.On("removeplayer", OnLeaveGame);

        io.On("disconnected", OnDisconnected);

        io.On("transform", OnTransform);

        io.On("checkPlayerCount",getPlayerCount);

        io.On("message", OnRecienveMessage);

    }


   

    public void rejoin()
    {
        StartCoroutine(Reconnect());

    }

    IEnumerator Reconnect()
    {
           
        yield return new WaitForSeconds(0.5f);
       
        io.Emit("rejoin", ClientID);

       
    }  


    private void OnConnected(SocketIOEvent E)
    {
        onConnected = true;
      

    }

    private void OnRegister(SocketIOEvent E)
    {
        string id = E.data["id"].ToString().Trim('"');
        ClientID = id;

        StartCoroutine(JoinDynamic_Room());
    }

    IEnumerator JoinDynamic_Room()
    {
         yield return new WaitForSeconds(1f);
            connectPanel.SetActive(false);
      
        playerCount = noPlayer.text.ToString();
        if (noPlayer.text.ToString() == "2")
        {
            AI aI = new AI();
            aI.isAI = isAI.ToString();
            io.Emit("two_player", new JSONObject(JsonUtility.ToJson(aI)));
        }
        if (noPlayer.text.ToString() == "4")
        {
            AI aI = new AI();
            aI.isAI = isAI.ToString();
            io.Emit("two_player", new JSONObject(JsonUtility.ToJson(aI)));
        }

    }

    private void OnLeaveGame(SocketIOEvent E)
    {

        string id = E.data["id"].ToString().Trim('"');
       // Debug.Log(id);
        if (ClientID != id)
        {

            GameObject go = serverObjects[id].gameObject;
            Destroy(go);
            serverObjects.Remove(id);
        }
    }

    private void OnDisconnected(SocketIOEvent E)
    {
        string id = E.data["id"].ToString().Trim('"');
        // Debug.Log(id);
        

            GameObject go = serverObjects[id].gameObject;
            Destroy(go);
            serverObjects.Remove(id);
        
    }  
   
    private void OnSpawn(SocketIOEvent E)
    {
        string id = E.data["id"].ToString().Trim('"');
        string spawned = E.data["spawned"]["_spawned"].ToString().Trim('"');
        string name = E.data["username"]["_username"].str;
        string room = E.data["room"]["_room"].ToString().Trim('"');

        if (ClientID != id && spawned == "true" && roomID == room)
        {
            GameObject go = Instantiate(playerPrefab, networkContainer);
            go.name = "Player_ID: " + id;

            NetworkIdentity ni = go.GetComponent<NetworkIdentity>();
            ni.SetControllerID(id);
            ni.SetNetworkName(name);
            ni.SetSocketReference(io);
            serverObjects.Add(id, ni);

        }
        WaitPanel.SetActive(false);

    }

    private void OnTransform(SocketIOEvent E)
    {
        string id = E.data["id"].ToString().Trim('"');
        float xp = E.data["position"]["x"].f;
        float yp = E.data["position"]["y"].f;
        float zp = E.data["position"]["z"].f;
        float xr = E.data["rotation"]["x"].f;
        float yr = E.data["rotation"]["y"].f;
        float zr = E.data["rotation"]["z"].f;
        NetworkIdentity ni = serverObjects[id];
        ni.transform.position = new Vector3(xp, yp, zp);
        ni.transform.rotation = Quaternion.Lerp(ni.transform.rotation, Quaternion.Euler(xr, yr, zr), Time.deltaTime * 10);

    }

    private void getPlayerCount(SocketIOEvent E)
    {
        Debug.Log("PlayerCount: " + E.data);
        string playerCount = E.data["count"].ToString().Trim('"');

        if (playerCount == "2")
        {
            isAllPlayersJoined = true;

        }
    }

    private void OnRecienveMessage(SocketIOEvent E)
    {
        Debug.Log(E.data);
        string _roomID = E.data["roomID"].ToString().Trim('"');
        roomID = _roomID;
        var playersID = E.data["playerid"];
        
        StartCoroutine(rotine_OtherPlayersSpawn_thisClient());
    }

    IEnumerator rotine_OtherPlayersSpawn_thisClient()
    {
      
        io.Emit("spawn_other_players");
        OnClick_ManualSpawn();
        yield return new WaitForSeconds(1f);
       
        yield return new WaitForSeconds(0f);
       
    }


    public void OnApplicationQuit()
    {
        if (io.IsConnected)
        {
            //  "[\"{0}\",{1}]"
            RoomDetail roomDetail = new RoomDetail();
            roomDetail.roomName = roomID;
            roomDetail.playerCount = playerCount;

            io.Emit("deleteRoom", new JSONObject(JsonUtility.ToJson(roomDetail)));
           // io.Emit("deleteRoom","RoomID: "+roomName + " PlayerCount: "+ playerCount );
            io.Emit("removeplayer",  ClientID);
        }
    }
    private string GetIPAddress()
    {
        StringBuilder sb = new StringBuilder();
        String strHostName = string.Empty;
        strHostName = Dns.GetHostName();
      //  sb.Append("The Local Machine Host Name: " + strHostName);
        sb.AppendLine();
        IPHostEntry ipHostEntry = Dns.GetHostEntry(strHostName);
        IPAddress[] address = ipHostEntry.AddressList;
        foreach (var ip in ipHostEntry.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                if (ip.ToString().Contains("192.168.1"))
                {
                    sb.Append("Your Local-IP: " + ip.ToString());
                    sb.AppendLine();
                }
            }
        }
      
        return sb.ToString();



        //var host = Dns.GetHostEntry(Dns.GetHostName());
        //foreach (var ip in host.AddressList)
        //{
        //    if (ip.AddressFamily == AddressFamily.InterNetwork)
        //    {
        //        return ip.ToString();
        //    }
        //}
        //throw new Exception("No network adapters with an IPv4 address in the system!");

    }

}

[Serializable]
public class RoomDetail
{
    public string roomName;
    public string playerCount;

}
[Serializable]
public class AI
{

    public string isAI;
}
