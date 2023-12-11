using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TicTacToeMP.Core.Client.Core;
using TicTacToeMP.Core.Model.Game;
using TicTacToeMP.Core.Protocol.Serialization;
using TicTacToeMP.Core.Protocol;
using TicTacToeMP.Core.Model.ServerCore;
using TicTacToeMP.Core.Model.Security;
using System.Windows;

namespace TicTacToeMP.Core.Client.ViewModel
{
    public  class CellViewModel : ObservableObject
    {
        private GameCell _cell;
        private readonly GameCellState playerSign;
        private readonly MeowClient _meowClient;
        private Player _player;

        public MeowClient MeowClientInstance => _meowClient; 
        public GameCell Cell { get { return _cell; } set { _cell = value; OnPropertyChanged("Cell"); } }
        


        private RelayCommand cellClickedCommand;
        private GameCellState _state;
        public GameCellState State { get => _state;set 
            { 
                _state = value;
                OnPropertyChanged("Cell"); } }

        public CellViewModel(GameCell cell, GameCellState playerSign, MeowClient client, Player player)
        {
            _cell = cell;
            this.playerSign = playerSign;
            _meowClient = client;
            _player = player;
        }

        public RelayCommand CellClickedCommand => cellClickedCommand ?? (
            cellClickedCommand = new RelayCommand(obj =>
            {
                if((playerSign == GameCellState.Cross && MeowClientInstance.TurnCounter % 2==0)||
                    (playerSign == GameCellState.Nought && MeowClientInstance.TurnCounter % 2 == 1))
                {
                    if(Cell.State == GameCellState.Empty)
                    {
                        Cell.State = playerSign;

                        State = Cell.State;

                        Thread.Sleep(1000);

                        MeowClientInstance.QueuePacketSend(
                        MeowPacketConverter.Serialize(MeowPacketType.Turn,
                        new MeowPacketTurn
                        {
                            Player = JsonSerializer.Serialize<Player>(_player),
                            TurnString = JsonSerializer.Serialize<Turn>(new Turn(this.Cell.ID, this.Cell.Index, this.Cell.State)),
                            TurnNumber = MeowClientInstance.TurnCounter++,
                        }).ToPacket());
                    }
                }

            }));

    }
}
