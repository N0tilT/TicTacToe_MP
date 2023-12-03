using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TicTacToeMP.Core.Client.Core;
using TicTacToeMP.Core.Model.Client;
using TicTacToeMP.Core.Model.Game;
using TicTacToeMP.Core.Protocol.Serialization;
using TicTacToeMP.Core.Protocol;

namespace TicTacToeMP.Core.Client.ViewModel
{
    public class GameViewModel : ObservableObject
    {

        private ObservableCollection<CellViewModel> _cells = new ObservableCollection<CellViewModel>();
        private GameField _gameField;
        private int _fieldSize;
        private static MeowClient _meowClient;

        public ObservableCollection<CellViewModel> Cells { get => _cells; set { _cells = value; OnPropertyChanged("Cells"); } }

        public GameField GameField { get => _gameField; }
        public int FieldSize { get => _fieldSize; set => _fieldSize = value; }
        public static MeowClient MeowClientInstance { get => _meowClient; set => _meowClient = value; }

        public GameViewModel()
        {
            _gameField = new GameField(LimitedFieldSize.ThreeByThree);
            FieldSize = _gameField.Size;

            Console.Title = "MeowClient";
            Console.ForegroundColor = ConsoleColor.White;

            MeowClientInstance = new MeowClient();
            MeowClientInstance.OnPacketRecieve += OnPacketRecieve;
            MeowClientInstance.Connect("127.0.0.1", 4910);

            string _handshakeMagic = "QWERTY";

            Thread.Sleep(1000);

            Console.WriteLine("Sending meow-meow packet..");

            MeowClientInstance.QueuePacketSend(
                MeowPacketConverter.Serialize(
                    MeowPacketType.Handshake,
                    new MeowPacketHandshake
                    {
                        MagicHandshakeString = _handshakeMagic,
                    })
                    .ToPacket());


            Cells = new ObservableCollection<CellViewModel>();
            foreach (var cell in _gameField.Field)
            {
                Cells.Add(new CellViewModel(cell, GameCellState.Cross));
            }
           
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
                MessageBox.Show("Handshake meowful!");
            }
        }

    }
}
