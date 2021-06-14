using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;
    public SocketIOComponent socket;

    public Canvas canvas;

    public InputField playerNameInput;
    public GameObject playerPrefab;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);

        }

        else
        {
            instance = this;

        }
    }
    public void JoinGame()
    {

        StartCoroutine(ConnectToServer());

    }

    IEnumerator ConnectToServer()
    {
        yield return new WaitForSeconds(0.5f);
        socket.Emit("player connect");
        yield return null;

        string playerName = playerNameInput.text;
        PlayerJSON playerJSON = new PlayerJSON(playerName);
        string data = JsonUtility.ToJson(playerJSON);
        socket.Emit("play", new JSONObject(data));
        canvas.gameObject.SetActive(false);

    }
    private void Start()
    {
        initilizeEvents();
    }

   void initilizeEvents()
    {

        socket.On("other player connected", OnOtherPlayerConnected);
        socket.On("play", OnPlay);
        socket.On("position", OnPlayerMove);
        socket.On("rotation", OnPlayerTurn);
        socket.On("other player disconnected", OnOtherPlayerDisconnected);




    }

   void OnOtherPlayerDisconnected(SocketIOEvent socketIOEvent)
    {
        
    }

  void  OnPlayerTurn(SocketIOEvent socketIOEvent)
    {
       
    }

 void OnPlayerMove(SocketIOEvent socketIOEvent)
    {
       
    }
 void OnOtherPlayerConnected(SocketIOEvent socketIOEvent)
    {
        print("SomeOne Joined");
        string data = socketIOEvent.data.ToString();
        UserJSON userJSON = UserJSON.CreateFromJSON(data);
        Vector3 position = new Vector3(userJSON.position[0], userJSON.position[1], userJSON.position[2]);
        Quaternion rotation = Quaternion.Euler(userJSON.rotation[0], userJSON.rotation[1], userJSON.rotation[2]);
        GameObject o = GameObject.Find(userJSON.name);
        if (o != null)
        {
            return;

        }
        GameObject go = Instantiate(playerPrefab, position, rotation);
        PlayerController pc = go.GetComponent<PlayerController>();
        GameObject t = GameObject.Find("player_name");

        Text playername = t.GetComponent<Text>();
        playername.text = userJSON.name;
        go.name = userJSON.name;



    }

void OnPlay(SocketIOEvent socketIOEvent)
    {
        print("you joined");
        string data = socketIOEvent.data.ToString();
        UserJSON currentUserJSON = UserJSON.CreateFromJSON(data);
        Vector3 position = new Vector3(currentUserJSON.position[0], currentUserJSON.position[1], currentUserJSON.position[2]);
        Quaternion rotation = Quaternion.Euler(currentUserJSON.rotation[0], currentUserJSON.rotation[1], currentUserJSON.rotation[2]);
        GameObject go = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

        PlayerController pc = go.GetComponent<PlayerController>();
        GameObject t = GameObject.Find("player_name");

        Text playername = t.GetComponent<Text>();
        playername.text = currentUserJSON.name;
        go.name = currentUserJSON.name;
        pc.isLocalPlayer=true;
    }

   

   
   
    

    [Serializable]
    public class PlayerJSON
    {

        public string name;

        public PlayerJSON(string _name)
        {

            name = _name;
        }

    }
    [Serializable]
    public class PositionJSON
    {
        public float[] position;
        public PositionJSON(Vector3 _position)
        {
            position = new float[] { _position.x, _position.y, _position.z };
        }
    }
    [Serializable]
    public class RotationJSON
    {
        public float[] rotation;
        public RotationJSON(Quaternion _rotation)
        {
            rotation = new float[] { _rotation.eulerAngles.x, _rotation.eulerAngles.y, _rotation.eulerAngles.z  };
        }
    }


     /// <summary>
     /// Telling Other client about myself or our data
     /// </summary>
    [Serializable]
    public class UserJSON
    {
        public string name;
        public float[] position;
        public float[] rotation;

        public static UserJSON CreateFromJSON(string data)
        {
            return JsonUtility.FromJson<UserJSON>(data);
        
        }

    }



}
