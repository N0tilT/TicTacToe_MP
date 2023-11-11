using System;
using System.Threading;
using TicTacToeMP.Core.Protocol;
using TicTacToeMP.Core.Protocol.Serialization;
using TicTacToeMP.Core.Test.Client;

namespace TicTacToeMP.Core.Test
{
    internal class Program
    {
        private static int _handshakeMagic;

        private static void Main()
        {
            Console.Title = "MeowClient";
            Console.ForegroundColor = ConsoleColor.White;

            var client = new MeowClient();
            client.OnPacketRecieve += OnPacketRecieve;
            client.Connect("127.0.0.1", 4910);

            var rand = new Random();
            _handshakeMagic = rand.Next();

            Thread.Sleep(1000);

            Console.WriteLine("Sending meow-meow packet..");

            client.QueuePacketSend(
                MeowPacketConverter.Serialize(
                    MeowPacketType.Handshake,
                    new MeowPacketHandshake
                    {
                        MagicHandshakeNumber = _handshakeMagic
                    })
                    .ToPacket());

            while (true) { }
        }

        private static void OnPacketRecieve(byte[] packet)
        {
            var parsed = MeowPacket.Parse(packet);

            if (parsed != null)
            {
                ProcessIncomingPacket(parsed);
            }
        }

        private static void ProcessIncomingPacket(MeowPacket packet)
        {
            var type = MeowPacketTypeManager.GetTypeFromPacket(packet);

            switch (type)
            {
                case MeowPacketType.Handshake:
                    ProcessHandshake(packet);
                    break;
                case MeowPacketType.Unknown:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void ProcessHandshake(MeowPacket packet)
        {
            var handshake = MeowPacketConverter.Deserialize<MeowPacketHandshake>(packet);

            if (_handshakeMagic - handshake.MagicHandshakeNumber == 15)
            {
                Console.WriteLine("Handshake meowful!");
            }
        }
    }
}
