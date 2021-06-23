/*
Server provides all the logic for the server
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace BDVR_Dedicated_Server
{
    class Server
    {
        public static int maxPlayers { get; private set; }
        public static int port { get; private set; }

        public static Dictionary<int, Client> clients = new Dictionary<int, Client>();
        public static Dictionary<string, NetworkedObject> objects = new Dictionary<string, NetworkedObject>();

        public delegate void PacketHandler(int _fromClient, Packet _packet);
        public static Dictionary<int, PacketHandler> packetHandlers;

        private static TcpListener tcpListener;
        private static UdpClient udpListener;
        
        /// <summary>
        /// Starts the server on a given port and a given
        /// maximum number of players
        /// </summary>
        /// <param name="_maxPlayers">Maximum players (#)</param>
        /// <param name="_port">Server port</param>
        public static void Start(int _maxPlayers, int _port)
        {
            // assign properties
            maxPlayers = _maxPlayers;
            port = _port;

            InitServerData();

            // init tcplistener
            tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);

            udpListener = new UdpClient(port);
            udpListener.BeginReceive(UDPReceiveCallback, null);

            Debug.Log($"BDVR Dedicated Server successfully started on {port}");
        }

        /// <summary>
        /// 
        /// </summary>
        private static void InitServerData()
        {
            for (int i = 1; i <= maxPlayers; i++)
            {
                clients.Add(i, new Client(i));
            }

            packetHandlers = new Dictionary<int, PacketHandler>()
            {
                { (int)ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived },
                { (int)ClientPackets.playerMovement, ServerHandle.PlayerMovement },
                { (int)ClientPackets.readyToSpawn, ServerHandle.ReadyToSpawn },
                { (int)ClientPackets.sendServerSettings, ServerHandle.SendServerSettings }
            };
        }

        public static void SendUDP(IPEndPoint _clientEndPoint, Packet _packet)
        {
            try
            {
                if (_clientEndPoint != null)
                {
                    udpListener.BeginSend(_packet.ToArray(), _packet.Length(), _clientEndPoint, null, null);
                }
            }
            catch (Exception _ex)
            {
                Debug.LogWarning($"shits fucked yo {_ex}");
            }
        }

        /* CLIENT CONNECTION METHODS */
        /// <summary>
        /// Handles connections from a client over TCP
        /// </summary>
        /// <param name="_result"></param>
        private static void TCPConnectCallback(IAsyncResult _result)
        {
            TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);

            Debug.Log($"Receiving TCP connection from {_client.Client.RemoteEndPoint}...");

            for (int i = 1; i <= maxPlayers; i++)
            {
                if (clients[i].tcp.socket == null)
                {
                    // connect the client
                    clients[i].tcp.Connect(_client);
                    return;
                }
            }
            Debug.Log($"{_client.Client.RemoteEndPoint} failed to connect: Server is full");
        }

        /// <summary>
        /// Handles connections from a client over UDP
        /// </summary>
        /// <param name="_result"></param>
        private static void UDPReceiveCallback(IAsyncResult _result)
        {
            try
            {
                IPEndPoint _clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] _data = udpListener.EndReceive(_result, ref _clientEndPoint);
                udpListener.BeginReceive(UDPReceiveCallback, null);

                if (_data.Length < 4) // may cause problems with high network traffic
                {
                    return; 
                }

                using (Packet _packet = new Packet(_data))
                {
                    int _clientId = _packet.ReadInt();

                    if (_clientId == 0)
                        return;

                    if (clients[_clientId].udp.endPoint == null)
                    {
                        clients[_clientId].udp.Connect(_clientEndPoint);
                        return;
                    }

                    if (clients[_clientId].udp.endPoint.ToString() == _clientEndPoint.ToString())
                    {
                        clients[_clientId].udp.HandleData(_packet);
                    }
                }
            }
            catch (Exception _ex)
            {
                Debug.LogWarning($"Error receiving UDP data: {_ex}");
            }
        }
    }
}
