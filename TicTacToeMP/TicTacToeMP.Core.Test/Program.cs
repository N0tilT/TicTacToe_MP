using System;
using System.Text.Json;
using System.Threading;
using TicTacToeMP.Core.Protocol;
using TicTacToeMP.Core.Protocol.Serialization;
using TicTacToeMP.Core.Test.Client;

namespace TicTacToeMP.Core.Test
{
    internal class Program
    {
        private static string _handshakeMagic;

        private static void Main()
        {
            Console.Title = "MeowClient";
            Console.ForegroundColor = ConsoleColor.White;

            var client = new MeowClient();
            client.OnPacketRecieve += OnPacketRecieve;
            client.Connect("127.0.0.1", 4910);

            _handshakeMagic = "QWERTY";

            Thread.Sleep(1000);

            Console.WriteLine("Sending meow-meow packet..");

            client.QueuePacketSend(
                MeowPacketConverter.Serialize(
                    MeowPacketType.Handshake,
                    new MeowPacketHandshake
                    {
                        MagicHandshakeString = _handshakeMagic,
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

            if (handshake.MagicHandshakeString == "QWERTY  MEOW")
            {
                Console.WriteLine("Handshake meowful!");
            }
        }
    }
}
