using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeMP.Core.Model.Game
{
    public enum LimitedFieldSize
    {
        ThreeByThree,
        FiveByFive,
        NineByNine
    }
    public class LimitedGameField : GameField
    {
        public LimitedGameField() : base() { }
        public LimitedGameField(LimitedFieldSize size) : base(size){}

    }
}
