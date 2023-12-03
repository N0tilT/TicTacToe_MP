using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TicTacToeMP.Core.Client.Core;
using TicTacToeMP.Core.Model.Client;
using TicTacToeMP.Core.Model.Game;
using TicTacToeMP.Core.Protocol.Serialization;
using TicTacToeMP.Core.Protocol;

namespace TicTacToeMP.Core.Client.ViewModel
{
    public  class CellViewModel : ObservableObject
    {
        private GameCell _cell;
        private readonly GameCellState playerSign;
        private readonly MeowClient _meowClient;

        public MeowClient MeowClientInstance => _meowClient; 
        public GameCell Cell { get { return _cell; } set { _cell = value; OnPropertyChanged("Cell"); } }
        


        private RelayCommand cellClickedCommand;
        private GameCellState _state;
        public GameCellState State { get => _state;set 
            { 
                _state = value;
                OnPropertyChanged("Cell"); } }

        public CellViewModel(GameCell cell, GameCellState playerSign, MeowClient client)
        {
            _cell = cell;
            this.playerSign = playerSign;
            _meowClient = client;
        }

        public RelayCommand CellClickedCommand => cellClickedCommand ?? (
            cellClickedCommand = new RelayCommand(obj =>
            {
                Cell.State = State == playerSign ? GameCellState.Empty : playerSign;

                State = Cell.State;

                MeowClientInstance.QueuePacketSend(
                MeowPacketConverter.Serialize(MeowPacketType.Turn,
                new MeowPacketTurn
                {
                    Player = JsonSerializer.Serialize(new Turn(this.Cell.Index,this.Cell.State))
                }).ToPacket());

            }));

    }
}
