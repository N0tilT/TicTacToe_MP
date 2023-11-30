using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeMP.Core.Model.Game
{
    public enum NeighbourPosition
    {
        Left, Right, Top, Bottom
    }
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
        private int[] _neighbours;
        private int index;

        public int ID { get { return _id; } }
        public GameCellState State { get { return _state; } set { _state = value; } }
        public int[] Neighbours { get { return _neighbours; } }

        public int Index { get => index; set => index = value; }


        public GameCell(int[] neighbours)
        {
            _id = counter++;
            State = GameCellState.Empty;
            _neighbours = neighbours;
        }

        public GameCell()
        {
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

        public int GetNeighbour(int cellId, NeighbourPosition neighbourPosition)
        {
            return neighbourPosition switch
            {
                NeighbourPosition.Left => cellId + _neighbours[0],
                NeighbourPosition.Top => cellId + _neighbours[1],
                NeighbourPosition.Right => cellId + _neighbours[2],
                NeighbourPosition.Bottom => cellId + _neighbours[3],
                _ => cellId + Neighbours[((int)neighbourPosition)],
            };
        }
    }
}
