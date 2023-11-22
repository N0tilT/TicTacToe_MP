using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeMP.Core.Model.Game
{
    public enum GameCellState
    {
        Empty,
        Cross,
        Nought
    }
    public class GameCell
    {
        private static int counter = 0;
        private int _id;
        private GameCellState _state;

        public int ID { get { return _id; } }
        public GameCellState State { get { return _state; } set { _state = value; } }

        public GameCell()
        {
            _id = counter++;
            State = GameCellState.Empty;
        }

        public void SetCross()
        {
            State = GameCellState.Cross;
        }
        public void SetNought()
        {
            State = GameCellState.Nought;
        }
        public void SetEmpty()
        {
            State = GameCellState.Empty;
        }
    }
}
