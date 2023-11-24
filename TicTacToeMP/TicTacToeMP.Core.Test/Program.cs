using System;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using TicTacToeMP.Core.Model.Game;
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
            //TestClient();
            TestModel();
        }

        private static void TestModel()
        {
            GameField gameField = new GameField(LimitedFieldSize.ThreeByThree);
            PrintField(gameField);

            GameField gameField2 = new GameField(LimitedFieldSize.FiveByFive);
            PrintField(gameField2);

            GameField gameField3 = new GameField(LimitedFieldSize.TenByTen);
            PrintField(gameField3);

        }

        private static void PrintField(GameField gameField)
        {
            Console.WriteLine("Field size: " + gameField.Size);
            for (int i = 0;i< gameField.Field.Count;i++)
            {
                Console.WriteLine(i+":");
                foreach (sbyte neighbour in gameField.Field[i].Neighbours)
                {
                    Console.Write(neighbour + " ");
                }
                Console.WriteLine();
            }
        }

        private static void TestClient()
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
