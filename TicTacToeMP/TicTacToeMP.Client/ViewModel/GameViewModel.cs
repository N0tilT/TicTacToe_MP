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

namespace TicTacToeMP.Core.Client.ViewModel
{
    public class GameViewModel : ObservableObject
    {

        private ObservableCollection<CellViewModel> _cells = new ObservableCollection<CellViewModel>();
        private GameField _gameField;
        private int _fieldSize;

        public ObservableCollection<CellViewModel> Cells { get => _cells; set { _cells = value; OnPropertyChanged("Cells"); } }

        public GameField GameField { get => _gameField; }
        public int FieldSize { get => _fieldSize; set => _fieldSize = value; }


        public GameViewModel()
        {
            _gameField = new GameField(LimitedFieldSize.ThreeByThree);
            FieldSize = _gameField.Size;

            Cells = new ObservableCollection<CellViewModel>();
            foreach (var cell in _gameField.Field)
            {
                Cells.Add(new CellViewModel(cell, GameCellState.Cross));
            }
           
        }


    }
}
