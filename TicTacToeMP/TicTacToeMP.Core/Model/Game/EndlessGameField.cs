using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeMP.Core.Model.Game
{
    public class EndlessGameField : GameField
    {
        public EndlessGameField():base(3) {}
        public EndlessGameField(int size):base(size){ }
        public EndlessGameField(LimitedFieldSize size) : base(size) { }

        public void Grow()
        {
            Size += 2;
        }
    }
}
