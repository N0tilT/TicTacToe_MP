using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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
                    Generate(Size*Size);
                    break;
                case LimitedFieldSize.FiveByFive:
                    Size = 5;
                    Generate(Size*Size);
                    break;
                case LimitedFieldSize.TenByTen:
                    Size = 10;
                    Generate(Size * Size);
                    break;
            }
        }

        public void Generate(int size)
        {
            _field.Add(new GameCell(new sbyte[] { 5, 7, 1, 3 }));
            int rank = 1;
            int circleCounter = 0;
            int counter = 0;
            int rankcounter = 0;
            int cornerCounter = 0;

            sbyte shift;
            for (int i = 1; i < size; i++)
            {
                counter++;

                shift = 7;
                sbyte left = (sbyte)(shift + 8 * circleCounter + 2 * 3);
                sbyte top = (sbyte)(shift + 8 * circleCounter);
                sbyte right = (sbyte)(shift + 8 * circleCounter + 2);
                sbyte bottom = (sbyte)(shift + 8 * circleCounter + 2 * 2);
                //Каждый раз шагаем rank раз 1 поворот(2 стороны поля).
                //Пройдя поворот и упершись в угол увеличиваем rank
                if (counter == rank)
                {
                    switch (cornerCounter % 4)
                    {
                        case 0://upper right
                            left = -1;
                            bottom = 1;
                            break;
                        case 1://bottom right
                            left = 1;
                            top = -1;
                            break;
                        case 2:// bottom left
                            right = -1;
                            top = 1;
                            break;
                        case 3://upper left
                            right = 1;
                            bottom = -1;
                            circleCounter++;
                            top = (sbyte)(shift + 8 * circleCounter);
                            break;
                    }
                    cornerCounter++;

                    counter = 0;
                    rankcounter++;
                    if(rankcounter == 2)
                    {
                        rank++;
                        rankcounter = 0;
                    }
                }
                else
                {
                    switch (cornerCounter % 4)
                    {
                        case 0://upper side
                            left = -1;
                            right = 1;
                            bottom = (sbyte)(top - 8);
                            break;
                        case 1://right side
                            left = (sbyte)(right - 8); 
                            top = -1;
                            bottom = 1;
                            break;
                        case 2://bottom side
                            left = 1;
                            top = (sbyte)(bottom - 8);
                            right = -1;
                            break;
                        case 3://left side
                            top = 1;
                            right = (sbyte)(left - 8);
                            bottom = -1;
                            break;
                    }
                }


                _field.Add(new GameCell(new sbyte[] { left, top, right, bottom }));

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
