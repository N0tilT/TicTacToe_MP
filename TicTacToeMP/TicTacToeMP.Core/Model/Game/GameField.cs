using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeMP.Core.Model.Game
{
    public class GameField
    {
        private int _size;
        public int Size { get { return _size; } set { _size = value; } }

        protected List<GameCell> _field;
        private LimitedFieldSize size;

        public List<GameCell> Field { get { return _field; }}

        public GameField()
        {
            _field = new List<GameCell>();
        }
        public GameField(int size) : this()
        {
            Size = size;
            Generate(Size);
        }

        public GameField(LimitedFieldSize size):this()
        {
            switch (size)
            {
                case LimitedFieldSize.ThreeByThree:
                    Size = 3;
                    Generate(Size);
                    break;
                case LimitedFieldSize.FiveByFive:
                    Size = 5;
                    Generate(Size);
                    break;
                case LimitedFieldSize.TenByTen:
                    Size = 10;
                    Generate(Size);
                    break;
            }
        }

        public void Generate(int size)
        {
            for (int i = 0; i < size; i++)
            {
                _field.Add(new GameCell());
            }
        }

        public void Clear()
        {
            foreach(GameCell cell in Field)
            {
                cell.SetEmpty();
            }
        }

    }
}
