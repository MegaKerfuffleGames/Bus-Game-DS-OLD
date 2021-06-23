/*
Player contains the representation of a client's in-game
player for the server
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace BDVR_Dedicated_Server
{
    class Player
    {
        public int id;
        public string username;
        
        // keys are scene index, values are spawnpoint in GameMaster
        private Dictionary<int, int> spawnPoints = new Dictionary<int, int>()
        {
            {1, 1 },    // saved spawnpoint for classic
            {2, 1 }     // saved spawnpoint for adventure
        };
        public int spawnPoint = 1; // active spawn point
        public Vector3 position;
        public Quaternion rotation;
        public float verInput;
        public float horInput;

        public Player(int _id, string _username, int _scene)
        {
            id = _id;
            username = _username;
            spawnPoint = spawnPoints[_scene];
        }

        /// <summary>
        /// Use to update the player's spawnpoint on server-side.
        /// </summary>
        /// <param name="_spawnPoint"></param>
        public void SaveSpawnPoint(int _scene, int _spawnPoint)
        {
            spawnPoints[_scene] = _spawnPoint;
            
        }

        public void UpdateTransform(Vector3 _position, Quaternion _rotation, float _verInp, float _horInp)
        {
            position = _position;
            rotation = _rotation;
            verInput = _verInp;
            horInput = _horInp;

            ServerSend.PlayerTransform(this);
        }
    }
}
