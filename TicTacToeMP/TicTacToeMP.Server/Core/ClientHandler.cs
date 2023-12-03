using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TicTacToeMP.Core.Model.Game;
using TicTacToeMP.Core.Protocol;
using TicTacToeMP.Core.Protocol.Serialization;

namespace TicTacToeMP.Server.Core
{
    public class ClientHandler
    {
        public Lobby Lobby { get; }
        public Socket Client { get; }

        private readonly Queue<byte[]> _packetSendingQueue = new Queue<byte[]>();

        public ClientHandler(Socket client, Lobby lobby)
        {
            Client = client;
            Lobby = lobby;

            Task.Run((Action)ProcessIncomingPackets);
            Task.Run((Action)SendPackets);
        }

        private void ProcessIncomingPackets()
        {
            while (true) // Слушаем пакеты, пока клиент не отключится.
            {
                var buff = new byte[256]; // Максимальный размер пакета - 256 байт.
                Client.Receive(buff);

                buff = buff.TakeWhile((b, i) =>
                {
                    if (b != 0xFF) return true;
                    return buff[i + 1] != 0;
                }).Concat(new byte[] { 0xFF, 0 }).ToArray();

                var parsed = MeowPacket.Parse(buff);

                if (parsed != null)
                {
                    ProcessIncomingPacket(parsed);
                }
            }
        }

        private void ProcessIncomingPacket(MeowPacket packet)
        {
            var type = MeowPacketTypeManager.GetTypeFromPacket(packet);

            switch (type)
            {
                case MeowPacketType.Handshake:
                    ProcessHandshake(packet);
                    break;
                case MeowPacketType.Turn:
                    ProcessIncomingTurn(packet);
                    break;
                case MeowPacketType.LobbyList:
                    ProcessLobbyList(packet);
                    break;
                case MeowPacketType.LobbyConnect:
                    ProcessLobbyConnection(packet);
                    break;
                case MeowPacketType.Unknown:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ProcessLobbyConnection(MeowPacket packet)
        {
            throw new NotImplementedException();
        }

        private void ProcessLobbyList(MeowPacket packet)
        {
            throw new NotImplementedException();
        }

        private void ProcessIncomingTurn(MeowPacket packet)
        {
            var turn = MeowPacketConverter.Deserialize<MeowPacketTurn>(packet);
            Lobby.Game.Place(int.Parse(turn.TurnString),Lobby.PlayerOne.Name == turn.Player ? Lobby.PlayerOneSide : Lobby.PlayerTwoSide);

        }

        private void ProcessHandshake(MeowPacket packet)
        {
            Console.WriteLine("Recieved handshake packet.");

            var handshake = MeowPacketConverter.Deserialize<MeowPacketHandshake>(packet);
            handshake.MagicHandshakeString += "  MEOW";

            Console.WriteLine("Answering..");

            QueuePacketSend(MeowPacketConverter.Serialize(MeowPacketType.Handshake, handshake).ToPacket());
        }

        public void QueuePacketSend(byte[] packet)
        {
            if (packet.Length > 256)
            {
                throw new Exception("Max packet size is 256 bytes.");
            }

            _packetSendingQueue.Enqueue(packet);
        }

        private void SendPackets()
        {
            while (true)
            {
                if (_packetSendingQueue.Count == 0)
                {
                    Thread.Sleep(100);
                    continue;
                }

                var packet = _packetSendingQueue.Dequeue();
                Client.Send(packet);

                Thread.Sleep(100);
            }
        }
    }
}
