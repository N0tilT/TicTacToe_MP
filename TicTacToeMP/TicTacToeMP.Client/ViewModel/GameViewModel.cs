using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TicTacToeMP.Core.Client.Core;
using TicTacToeMP.Core.Model.Game;
using TicTacToeMP.Core.Protocol.Serialization;
using TicTacToeMP.Core.Protocol;
using TicTacToeMP.Core.Model.Security;
using System.Text.Json;
using System.Threading;
using TicTacToeMP.Core.Model.ServerCore;
using System.DirectoryServices;

namespace TicTacToeMP.Core.Client.ViewModel
{
    public class CellComparer : IComparer<CellViewModel>
    {
        public int Compare(CellViewModel op1, CellViewModel op2)
        {
            if(op1.Cell.Index > op2.Cell.Index)
            {
                return 1;
            }
            else if(op1.Cell.Index < op2.Cell.Index)
            {
                return -1;
            }
            return 0;
        }
    }

    public class GameViewModel : ObservableObject
    {

        private ObservableCollection<CellViewModel> _cells = new ObservableCollection<CellViewModel>();
        private GameField _gameField;
        private int _fieldSize;
        private static MeowClient _meowClient;
        private Player _player;
        private Player _opponent;
        private Player _playerCross;
        private Player _playerNought;
        private GameCellState _cellState;
        private int _playerOneScore = 0;
        private int _playerTwoScore = 0;
        private int _roundCounter = 1;

        public ObservableCollection<CellViewModel> Cells { get => _cells; set { _cells = value; OnPropertyChanged("Cells"); } }

        public GameField GameField { get => _gameField; }
        public int FieldSize { get => _fieldSize; set => _fieldSize = value; }
        public static MeowClient MeowClientInstance { get => _meowClient; set => _meowClient = value; }
        public Player PlayerCross { get => _playerCross; set { _playerCross = value; OnPropertyChanged("PlayerCross"); } }
        public Player PlayerNought { get => _playerNought; set { _playerNought = value; OnPropertyChanged("PlayerNought"); } }

        public int PlayerOneScore { get => _playerOneScore; set => _playerOneScore = value; }
        public int PlayerTwoScore { get => _playerTwoScore; set => _playerTwoScore = value; }
        public int RoundCounter { get => _roundCounter; set => _roundCounter = value; }

        public GameViewModel(string playerName, string socket)
        {
            _player = new Player(playerName);

            _gameField = new GameField(LimitedFieldSize.ThreeByThree);
            FieldSize = _gameField.Size;

            MeowClientInstance = new MeowClient();
            MeowClientInstance.OnPacketRecieve += OnPacketRecieve;

            string ip = socket.Split(':')[0];
            string port = socket.Split(':')[1];

            MeowClientInstance.Connect(ip, int.Parse(port));

            string _handshakeMagic = "QWERTY";

            Thread.Sleep(1000);

            MeowClientInstance.QueuePacketSend(
                MeowPacketConverter.Serialize(
                    MeowPacketType.Handshake,
                    new MeowPacketHandshake
                    {
                        MagicHandshakeString = _handshakeMagic,
                    })
                    .ToPacket());

            MeowClientInstance.QueuePacketSend(
                MeowPacketConverter.Serialize(MeowPacketType.LobbyConnect,
                new MeowPacketLobbyConnect
                {
                    Player = JsonSerializer.Serialize(_player)
                }).ToPacket());


            while(_cellState == GameCellState.Empty)
            {
                Thread.Sleep(100);
            }

            while(PlayerCross == null || PlayerNought == null)
            {
                Thread.Sleep(100);
            }

            RefreshField();

            PlayerOneScore = 0;
            PlayerTwoScore = 0;
            RoundCounter = 1;

        }

        private void OnPacketRecieve(byte[] packet)
        {
            var parsed = MeowPacket.Parse(packet);

            if (parsed != null)
            {
                ProcessIncomingPacket(parsed);
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
                    ProccesIncomingTurn(packet);
                    break;
                case MeowPacketType.LobbyConnectionResponse:
                    ProccessResponse(packet);
                    break;
                case MeowPacketType.Win:
                    ProccessWin(packet);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ProccessWin(MeowPacket packet)
        {
            var win = MeowPacketConverter.Deserialize<MeowPacketWin>(packet);
            var winner = JsonSerializer.Deserialize<Player>(win.Winner);
            

            if (winner.Name == _player.Name)
            {
                PlayerOneScore++;
            }
            else
            {
                PlayerTwoScore++;
            }
            RoundCounter++;

            if (RoundCounter == 3)
            {
                MessageBox.Show("Игра окончена! Победу одержал(а) " + winner.Name + ".\n Сыграем ещё.","Победа!",MessageBoxButton.OK,MessageBoxImage.Asterisk);
                RoundCounter = 0;
                PlayerOneScore = 0;
                PlayerTwoScore = 0;
            }
            else
            {
                MessageBox.Show("Победу в раунде одержал(а) " + winner.Name,"Победа!",MessageBoxButton.OK);
            }

            RefreshField();
        }

        private void RefreshField()
        {
            _gameField = new GameField(LimitedFieldSize.ThreeByThree);
            List<CellViewModel> cells = new List<CellViewModel>();
            foreach (var cell in _gameField.Field)
            {
                cells.Add(new CellViewModel(cell, _cellState, MeowClientInstance, _player));
            }

            CellComparer cc = new CellComparer();
            cells.Sort(cc);
            Cells = new ObservableCollection<CellViewModel>(cells);
            MeowClientInstance.TurnCounter = 0;
        }

        private void ProccessResponse(MeowPacket packet)
        {
            var response = MeowPacketConverter.Deserialize<MeowPacketLobbyConnectionResponse>(packet);
            _cellState = JsonSerializer.Deserialize<GameCellState>(response.Response);
            var playerOne = JsonSerializer.Deserialize<Player>(response.PlayerOneString);
            var playerTwo = JsonSerializer.Deserialize<Player>(response.PlayerTwoString);

            if (playerOne.Name == _player.Name)
            {
                _opponent = playerTwo;
            }
            else
            {
                _opponent = playerOne;
            }

            if(_cellState == GameCellState.Cross)
            {
                PlayerCross = _player;
                PlayerNought = _opponent;
            }
            else
            {
                PlayerCross = _opponent;
                PlayerNought = _player;
            }

        }

        private void ProccesIncomingTurn(MeowPacket packet)
        {
            MeowClientInstance.TurnCounter++;
            var turnPacket = MeowPacketConverter.Deserialize<MeowPacketTurn>(packet);

            if(turnPacket != null)
            {
                Turn? turn = JsonSerializer.Deserialize<Turn>(turnPacket.TurnString);
                for (int i = 0; i < Cells.Count; i++)
                {
                    if (Cells[i].Cell.ID == turn?.CellID)
                    {
                        Cells[i].Cell = new GameCell() {ID = turn.CellID,Index=turn.CellIndex, State = turn.CellState};
                        break;
                    }
                }
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
