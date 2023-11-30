using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TicTacToeMP.Core.Client.Core;
using TicTacToeMP.Core.Model.Game;

namespace TicTacToeMP.Core.Client.ViewModel
{
    public  class CellViewModel : ObservableObject
    {
        private GameCell _cell;
        private readonly GameCellState playerSign;

        public GameCell Cell { get { return _cell; } set { _cell = value; OnPropertyChanged("Cell"); } }
        


        private RelayCommand cellClickedCommand;
        private GameCellState _state;
        public GameCellState State { get => _state;set 
            { 
                _state = value;
                OnPropertyChanged("Cell"); } }

        public CellViewModel(GameCell cell, GameCellState playerSign)
        {
            _cell = cell;
            this.playerSign = playerSign;
        }

        public RelayCommand CellClickedCommand => cellClickedCommand ?? (
            cellClickedCommand = new RelayCommand(obj =>
            {
                Cell.State = State == playerSign ? GameCellState.Empty : playerSign;

                State = Cell.State;
                
            }));

    }
}
