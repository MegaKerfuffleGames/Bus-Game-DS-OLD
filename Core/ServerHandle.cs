/*
ServerHandle provides logic for incoming packets, with each method
being resposible for a given packet.
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace BDVR_Dedicated_Server
{
    class ServerHandle
    {
        /// <summary>
        /// Acknowledges that a client has received the server's
        /// welcome message and verifies client ID
        /// </summary>
        /// <param name="_fromClient">Sending client</param>
        /// <param name="_packet">Packet from client</param>
        public static void WelcomeReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _username = _packet.ReadString();


            if (_fromClient != _clientIdCheck)
            {
                Debug.Log($"Player {_username} (ID {_fromClient}) has an incorrect client ID ({_clientIdCheck})");
                Debug.Log("If you are seeing this message, something has probably gone very wrong");
            }
            else { 
                Debug.Log($"{_username} joined the game.");
                Server.clients[_fromClient].username = _username;
                ServerSend.SendPlayerNames();
                ServerSend.UpdateServerSettings();
            }
            // Connection established at this point
        }

        public static void ReadyToSpawn(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();

            if (_fromClient == _clientIdCheck) { 
                Debug.Log($"Sending {Server.clients[_fromClient].username} into game...");
                Server.clients[_fromClient].SendIntoGame();
            }
            else
            {
                Debug.Log($"Player ID {_fromClient} has an incorrect client ID ({_clientIdCheck})");
            }
        }

        public static void SendServerSettings(int _fromClient, Packet _packet)
        {
            Debug.Log($"Received new settings from player {_fromClient}");
            Settings.sceneToLoad = _packet.ReadInt();

            // send server settings to clients
            ServerSend.UpdateServerSettings();
        }

        public static void PlayerMovement(int _fromClient, Packet _packet)
        {
            // please remove the try-catch when done debugging
            try { 
                Vector3 _position = _packet.ReadVector3();
                Quaternion _rotation = _packet.ReadQuaternion();
                float _verInp = _packet.ReadFloat(); // could use these inputs to verify movement?
                float _horInp = _packet.ReadFloat();

                Server.clients[_fromClient].player.UpdateTransform(_position, _rotation, _verInp, _horInp);
            }
            catch
            {
                Debug.Log($"Failed to update player movement from client {_fromClient}");
            }
        }
    }
}
