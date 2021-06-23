/*
ServerSend contains methods for sending packets to clients 
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace BDVR_Dedicated_Server
{
    class ServerSend
    {
        /* TCP/UDP PACKET SENDING */
        /// <summary>
        /// Send a packet to a specific client via TCP
        /// </summary>
        /// <param name="_toClient">Client to send packet to</param>
        /// <param name="_packet">Packet to send</param>
        private static void SendTCP(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].tcp.SendData(_packet);
        }

        /// <summary>
        /// Send a packet to a specific client via UDP
        /// </summary>
        /// <param name="_toClient">Client to send packet to</param>
        /// <param name="_packet">Packet to send</param>
        private static void SendUDP(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].udp.SendData(_packet);
        }

        /// <summary>
        /// Send a packet to all clients via TCP
        /// </summary>
        /// <param name="_packet">Packet to send</param>
        private static void SendTCPToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.maxPlayers; i++)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }

        /// <summary>
        /// Send a packet to all clients via UDP
        /// </summary>
        /// <param name="_packet">Packet to send</param>
        private static void SendUDPToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.maxPlayers; i++)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }

        /// <summary>
        /// Send a packet to all clients except one via TCP
        /// </summary>
        /// <param name="_exceptClient">Client to avoid</param>
        /// <param name="_packet">Packet to send</param>
        private static void SendTCPToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.maxPlayers; i++)
            {
                if (i != _exceptClient)
                    Server.clients[i].tcp.SendData(_packet);
            }
        }

        /// <summary>
        /// Send a packet to all clients except one via UDP
        /// </summary>
        /// <param name="_exceptClient">Client to avoid</param>
        /// <param name="_packet">Packet to send</param>
        private static void SendUDPToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.maxPlayers; i++)
            {
                if (i != _exceptClient)
                    Server.clients[i].udp.SendData(_packet);
            }
        }

        /* METHODS TO SEND DATA TO SPECIFIC CLIENTS */
        /// <summary>
        /// Sends a welcome message to a specific client (via TCP)
        /// </summary>
        /// <param name="_toClient">Client to send message to</param>
        /// <param name="_msg">Message to send</param>
        public static void Welcome(int _toClient, string _msg)
        {
            using (Packet _packet = new Packet((int)ServerPackets.welcome))
            {
                _packet.Write(_msg);
                _packet.Write(_toClient);
                _packet.Write(Settings.sceneToLoad);

                SendTCP(_toClient, _packet);
            }
        }

        /// <summary>
        /// Sends a packet telling a client to spawn a player (via TCP)
        /// </summary>
        /// <param name="_toClient">Client to spawn player for</param>
        /// <param name="_player">Player to spawn</param>
        public static void SpawnPlayer(int _toClient, Player _player)
        {
            using (Packet _packet = new Packet((int)ServerPackets.spawnPlayer))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.username);
                _packet.Write(_player.spawnPoint);

                SendTCP(_toClient, _packet);
            }
        }

        public static void SendPlayerNames()
        {
            using (Packet _packet = new Packet((int)ServerPackets.sendPlayerNames))
            {
                int largestClientID = 0;
                foreach (Client _client in Server.clients.Values)
                {
                    if (_client.username != null)
                        largestClientID += 1;
                    else
                        break;
                }

                _packet.Write(largestClientID); // send count first to tell client to iterate x times
                for (int i = 1; i <= largestClientID; i++)
                {
                    Client _client = Server.clients[i];
                    _packet.Write(_client.id);
                    _packet.Write(_client.username);
                }


                SendTCPToAll(_packet);
            }
        }

        public static void PlayerTransform(Player _player)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerTransform))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.position);
                _packet.Write(_player.rotation);
                _packet.Write(_player.verInput);
                _packet.Write(_player.horInput);

                SendUDPToAll(_player.id, _packet);
            }
        }

        public static void PlayerDisconnected(int _playerID)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerDisconnected))
            {
                _packet.Write(_playerID);

                SendTCPToAll(_packet);
            }
        }

        public static void DayNightSync()
        {
            using (Packet _packet = new Packet((int)ServerPackets.dayNightSync))
            {
                // TODO: finish this


            }
        }

        public static void UpdateServerSettings()
        {
            using (Packet _packet = new Packet((int)ServerPackets.updateServerSettings))
            {
                _packet.Write(Settings.sceneToLoad);

                SendTCPToAll(_packet);
            }
        }
    }
}
