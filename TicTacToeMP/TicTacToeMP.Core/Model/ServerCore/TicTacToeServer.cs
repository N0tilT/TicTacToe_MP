using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TicTacToeMP.Core.Model.Game;

namespace TicTacToeMP.Core.Model.ServerCore
{
    public class TicTacToeServer
    {
        private readonly Socket _socket;
        private readonly List<ClientHandler> _clients;
        private List<Lobby> _lobbies;

        private bool _listening;
        private bool _stopListening;

        public TicTacToeServer()
        {
            var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            var ipAddress = ipHostInfo.AddressList[0];

            _socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _clients = new List<ClientHandler>();
            _lobbies = new List<Lobby>() { new Lobby(GameMode.Limited)};
        }

        public void Start()
        {
            if (_listening)
            {
                throw new Exception("Server is already listening incoming requests.");
            }

            _socket.Bind(new IPEndPoint(IPAddress.Any, 4137));
            _socket.Listen(10);

            _listening = true;

            Console.WriteLine($"Server started at: {((IPEndPoint)_socket.LocalEndPoint).Address}:{((IPEndPoint)_socket.LocalEndPoint).Port}");
        }

        public void Stop()
        {
            if (!_listening)
            {
                throw new Exception("Server is already not listening incoming requests.");
            }

            _stopListening = true;
            _socket.Shutdown(SocketShutdown.Both);
            _listening = false;
        }

        public void AcceptClients()
        {
            while (true)
            {
                if (_stopListening)
                {
                    return;
                }

                Socket client;

                try
                {
                    client = _socket.Accept();
                }
                catch { return; }

                Console.WriteLine($"[!] Accepted client from {(IPEndPoint)client.RemoteEndPoint}");
                
                var c = new ClientHandler(client, _lobbies);
                _clients.Add(c);
            }
        }
    }
}
