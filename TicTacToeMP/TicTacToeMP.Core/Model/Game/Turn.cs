using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeMP.Core.Model.Game
{
    public class Turn
    {
        public Turn(int cellIndex, GameCellState cellState)
        {
            CellIndex = cellIndex;
            CellState = cellState;
        }

        public int CellIndex { get; set; }
        public GameCellState CellState { get; set; }

    }
}
