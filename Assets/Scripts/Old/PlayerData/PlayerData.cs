using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData 
{
    [Serializable]
    public class Player
    {

        public string id;
        public Position position;
        public Rotation rotation;
        public Username username;
        public Spawned spawned;
        public Room room;

    }
    [Serializable]
    public class Position
    {
        public float x, y, z;
    }
    [Serializable]
    public class Rotation
    {
        public float x, y, z;
    }
    [Serializable]
    public class Username
    {
        public string _username;

    }
    [Serializable]
    public class Spawned
    {
        public string _spawned;

    }
    [Serializable]
    public class Room
    {
        public string _room;

    }
}
