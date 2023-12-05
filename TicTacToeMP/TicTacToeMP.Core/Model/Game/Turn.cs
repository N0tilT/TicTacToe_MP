using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeMP.Core.Model.Game
{
    public class Turn
    {
        public Turn(int cellId, int cellIndex, GameCellState cellState)
        {
            CellID = cellId;
            CellIndex = cellIndex;
            CellState = cellState;
        }

        public int CellID { get; set; }
        public int CellIndex { get; set; }
        public GameCellState CellState { get; set; }

    }
}
