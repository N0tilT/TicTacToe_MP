using System;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using TicTacToeMP.Core.Model.Game;
using TicTacToeMP.Core.Model.ServerCore;
using TicTacToeMP.Core.Protocol;
using TicTacToeMP.Core.Protocol.Serialization;

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
            TestThreeByThree();
            //TestFiveByFive();
            //TestTenByTen();
            //TestIndex();

        }

        private static void TestIndex()
        {
            Console.WriteLine("3x3 Test:");
            GameEngine engine = new GameEngine(GameMode.Limited, LimitedFieldSize.ThreeByThree);
            for (int i = 0; i < engine.Field.Field.Count; i++)
            {
                Console.Write(engine.Field.Field[i].Index + " ");

            }
            Console.WriteLine();

            Console.WriteLine("5x5 Test:");
            GameEngine engine2 = new GameEngine(GameMode.Limited, LimitedFieldSize.FiveByFive);
            for (int i = 0; i < engine2.Field.Field.Count; i++)
            {
                Console.Write(engine2.Field.Field[i].Index + " ");

            }
            Console.WriteLine();

            Console.WriteLine("10x10 Test:");
            GameEngine engine3 = new GameEngine(GameMode.Limited, LimitedFieldSize.NineByNine);
            for (int i = 0; i < engine3.Field.Field.Count; i++)
            {
                Console.Write(engine3.Field.Field[i].Index + " ");

            }
            Console.WriteLine();

        }
        #region 3x3
        private static void TestThreeByThree()
        {
            Console.WriteLine("3x3 Test:");
            GameEngine engine = new GameEngine(GameMode.Limited, LimitedFieldSize.ThreeByThree);
            engine.MakeTurn(new Turn(0, GameCellState.Cross));
            if (engine.IsWinSignPlaced(0))
            {
                Console.WriteLine("Failed");
            }
            engine.MakeTurn(new Turn(2, GameCellState.Cross)); 
            if (engine.IsWinSignPlaced(2))
            {
                Console.WriteLine("Failed");
            }
            engine.MakeTurn(new Turn(6, GameCellState.Cross)); 
            Console.WriteLine("6,0,2:");
            if (engine.IsWinSignPlaced(6) && engine.IsWinSignPlaced(0) && engine.IsWinSignPlaced(2))
            {
                Console.WriteLine("passed");
            }
            else
            {
                Console.WriteLine("failed");
            }
            engine.MakeTurn(new Turn(0, GameCellState.Nought));
            engine.MakeTurn(new Turn(1, GameCellState.Nought));
            engine.MakeTurn(new Turn(5, GameCellState.Nought));
            Console.WriteLine("5,0,1:");
            if (engine.IsWinSignPlaced(5) && engine.IsWinSignPlaced(0) && engine.IsWinSignPlaced(1))
            {
                Console.WriteLine("passed");
            }
            else
            {
                Console.WriteLine("failed");
            }
            engine.MakeTurn(new Turn(0, GameCellState.Nought));
            engine.MakeTurn(new Turn(3, GameCellState.Nought));
            engine.MakeTurn(new Turn(7, GameCellState.Nought));
            Console.WriteLine("7,0,3:");
            if (engine.IsWinSignPlaced(7) && engine.IsWinSignPlaced(0) && engine.IsWinSignPlaced(3))
            {
                Console.WriteLine("passed");
            }
            else
            {
                Console.WriteLine("failed");
            }
            engine.MakeTurn(new Turn(0, GameCellState.Cross));
            engine.MakeTurn(new Turn(4, GameCellState.Cross));
            engine.MakeTurn(new Turn(8, GameCellState.Cross));

            Console.WriteLine("8,0,4:");
            if (engine.IsWinSignPlaced(8) && engine.IsWinSignPlaced(0) && engine.IsWinSignPlaced(4))
            {
                Console.WriteLine("passed");
            }
            else
            {
                Console.WriteLine("failed");
            }

            engine.Field.Clear();

            engine.MakeTurn(new Turn(4, GameCellState.Cross));
            engine.MakeTurn(new Turn(6, GameCellState.Cross));
            engine.MakeTurn(new Turn(5, GameCellState.Cross));

            Console.WriteLine("6,5,4:");
            if (engine.IsWinSignPlaced(4) && engine.IsWinSignPlaced(6) && engine.IsWinSignPlaced(5))
            {
                Console.WriteLine("passed");
            }
            else
            {
                Console.WriteLine("failed");
            }
        }
        #endregion
        #region 5x5
        private static void TestFiveByFive()
        {
            Console.WriteLine("5x5 Test:");
            GameEngine engine = new GameEngine(GameMode.Limited, LimitedFieldSize.FiveByFive);
            engine.MakeTurn(new Turn(0, GameCellState.Cross));
            engine.MakeTurn(new Turn(2, GameCellState.Cross));
            engine.MakeTurn(new Turn(6, GameCellState.Cross));
            engine.MakeTurn(new Turn(12, GameCellState.Cross));
            engine.MakeTurn(new Turn(20, GameCellState.Cross));
            Console.WriteLine("6,0,2:");
            if (engine.IsWinSignPlaced(6) && engine.IsWinSignPlaced(0) && engine.IsWinSignPlaced(2) && engine.IsWinSignPlaced(12) && engine.IsWinSignPlaced(20))
            {
                Console.WriteLine("passed");
            }
            else
            {
                Console.WriteLine("failed");
            }
            engine.MakeTurn(new Turn(0, GameCellState.Nought));
            engine.MakeTurn(new Turn(1, GameCellState.Nought));
            engine.MakeTurn(new Turn(5, GameCellState.Nought));
            engine.MakeTurn(new Turn(10, GameCellState.Nought));
            engine.MakeTurn(new Turn(18, GameCellState.Nought));
            Console.WriteLine("5,0,1:");
            if (engine.IsWinSignPlaced(5) && engine.IsWinSignPlaced(0) && engine.IsWinSignPlaced(1) && engine.IsWinSignPlaced(10) && engine.IsWinSignPlaced(18))
            {
                Console.WriteLine("passed");
            }
            else
            {
                Console.WriteLine("failed");
            }
            engine.MakeTurn(new Turn(0, GameCellState.Nought));
            engine.MakeTurn(new Turn(3, GameCellState.Nought));
            engine.MakeTurn(new Turn(7, GameCellState.Nought));
            engine.MakeTurn(new Turn(14, GameCellState.Nought));
            engine.MakeTurn(new Turn(22, GameCellState.Nought));
            Console.WriteLine("7,0,3:");
            if (engine.IsWinSignPlaced(7) && engine.IsWinSignPlaced(0) && engine.IsWinSignPlaced(3) && engine.IsWinSignPlaced(14) && engine.IsWinSignPlaced(22))
            {
                Console.WriteLine("passed");
            }
            else
            {
                Console.WriteLine("failed");
            }
            engine.MakeTurn(new Turn(0, GameCellState.Cross));
            engine.MakeTurn(new Turn(4, GameCellState.Cross));
            engine.MakeTurn(new Turn(8, GameCellState.Cross));
            engine.MakeTurn(new Turn(16, GameCellState.Cross));
            engine.MakeTurn(new Turn(24, GameCellState.Cross));

            Console.WriteLine("8,0,4:");
            if (engine.IsWinSignPlaced(8) && engine.IsWinSignPlaced(0) && engine.IsWinSignPlaced(4) && engine.IsWinSignPlaced(16) && engine.IsWinSignPlaced(24))
            {
                Console.WriteLine("passed");
            }
            else
            {
                Console.WriteLine("failed");
            }

            engine.Field.Clear();

            engine.MakeTurn(new Turn(21, GameCellState.Cross));
            engine.MakeTurn(new Turn(6, GameCellState.Cross));
            engine.MakeTurn(new Turn(5, GameCellState.Cross));
            engine.MakeTurn(new Turn(4, GameCellState.Cross));
            engine.MakeTurn(new Turn(15, GameCellState.Cross));

            Console.WriteLine("21,6,5,4,15:");
            if (engine.IsWinSignPlaced(21) && engine.IsWinSignPlaced(6) && engine.IsWinSignPlaced(5) && engine.IsWinSignPlaced(4) && engine.IsWinSignPlaced(15))
            {
                Console.WriteLine("passed");
            }
            else
            {
                Console.WriteLine("failed");
            }
        }
        #endregion
        #region 10x10
        private static void TestTenByTen()
        {
            Console.WriteLine("10x10 Test:");
            GameEngine engine = new GameEngine(GameMode.Limited, LimitedFieldSize.NineByNine);
            engine.MakeTurn(new Turn(0, GameCellState.Cross));
            engine.MakeTurn(new Turn(2, GameCellState.Cross));
            engine.MakeTurn(new Turn(6, GameCellState.Cross));
            engine.MakeTurn(new Turn(12, GameCellState.Cross));
            engine.MakeTurn(new Turn(20, GameCellState.Cross));
            Console.WriteLine("6,0,2:");
            if (engine.IsWinSignPlaced(6) && engine.IsWinSignPlaced(0) && engine.IsWinSignPlaced(2) && engine.IsWinSignPlaced(12) && engine.IsWinSignPlaced(20))
            {
                Console.WriteLine("passed");
            }
            else
            {
                Console.WriteLine("failed");
            }
            engine.MakeTurn(new Turn(0, GameCellState.Nought));
            engine.MakeTurn(new Turn(1, GameCellState.Nought));
            engine.MakeTurn(new Turn(5, GameCellState.Nought));
            engine.MakeTurn(new Turn(10, GameCellState.Nought));
            engine.MakeTurn(new Turn(18, GameCellState.Nought));
            Console.WriteLine("5,0,1:");
            if (engine.IsWinSignPlaced(5) && engine.IsWinSignPlaced(0) && engine.IsWinSignPlaced(1) && engine.IsWinSignPlaced(10) && engine.IsWinSignPlaced(18))
            {
                Console.WriteLine("passed");
            }
            else
            {
                Console.WriteLine("failed");
            }
            engine.MakeTurn(new Turn(0, GameCellState.Nought));
            engine.MakeTurn(new Turn(3, GameCellState.Nought));
            engine.MakeTurn(new Turn(7, GameCellState.Nought));
            engine.MakeTurn(new Turn(14, GameCellState.Nought));
            engine.MakeTurn(new Turn(22, GameCellState.Nought));
            Console.WriteLine("7,0,3:");
            if (engine.IsWinSignPlaced(7) && engine.IsWinSignPlaced(0) && engine.IsWinSignPlaced(3) && engine.IsWinSignPlaced(14) && engine.IsWinSignPlaced(22))
            {
                Console.WriteLine("passed");
            }
            else
            {
                Console.WriteLine("failed");
            }
            engine.MakeTurn(new Turn(0, GameCellState.Cross));
            engine.MakeTurn(new Turn(4, GameCellState.Cross));
            engine.MakeTurn(new Turn(8, GameCellState.Cross));
            engine.MakeTurn(new Turn(16, GameCellState.Cross));
            engine.MakeTurn(new Turn(24, GameCellState.Cross));

            Console.WriteLine("8,0,4:");
            if (engine.IsWinSignPlaced(8) && engine.IsWinSignPlaced(0) && engine.IsWinSignPlaced(4) && engine.IsWinSignPlaced(16) && engine.IsWinSignPlaced(24))
            {
                Console.WriteLine("passed");
            }
            else
            {
                Console.WriteLine("failed");
            }

            engine.Field.Clear();

            engine.MakeTurn(new Turn(21, GameCellState.Cross));
            engine.MakeTurn(new Turn(6, GameCellState.Cross));
            engine.MakeTurn(new Turn(5, GameCellState.Cross));
            engine.MakeTurn(new Turn(4, GameCellState.Cross));
            engine.MakeTurn(new Turn(15, GameCellState.Cross));

            Console.WriteLine("21,6,5,4,15:");
            if (engine.IsWinSignPlaced(21) && engine.IsWinSignPlaced(6) && engine.IsWinSignPlaced(5) && engine.IsWinSignPlaced(4) && engine.IsWinSignPlaced(15))
            {
                Console.WriteLine("passed");
            }
            else
            {
                Console.WriteLine("failed");
            }
        }
#endregion
        private static void PrintField(GameField gameField)
        {
            Console.WriteLine("Field size: " + gameField.Size);
            for (int i = 0;i< gameField.Field.Count;i++)
            {
                Console.WriteLine(gameField.Field[i].State);
                
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
