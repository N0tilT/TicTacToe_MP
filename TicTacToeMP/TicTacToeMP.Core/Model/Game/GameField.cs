using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeMP.Core.Model.Game
{
    public class GameField
    {
        private int _size; 
        protected int _winRowLength;

        public int Size { get { return _size; } set { _size = value; } }

        protected List<GameCell> _field;
        private LimitedFieldSize size;

        public List<GameCell> Field { get { return _field; } }
        public int WinRowLength { get { return _winRowLength; } }

        public GameField()
        {
            _field = new List<GameCell>();
        }
        public GameField(int size) : this()
        {
            if (size < 3)
            {
                throw new ArgumentException();
            }
            else if (size >= 5)
            {
                _winRowLength = 5;
            }
            else
            {
                _winRowLength = 3;
            }
            Size = size;
            Generate(Size);
        }

        public GameField(LimitedFieldSize size):this()
        {
            switch (size)
            {
                case LimitedFieldSize.ThreeByThree:
                    Size = 3;
                    _winRowLength = 3;
                    Generate(Size*Size);
                    break;
                case LimitedFieldSize.FiveByFive:
                    Size = 5;
                    _winRowLength=5;
                    Generate(Size*Size);
                    break;
                case LimitedFieldSize.TenByTen:
                    Size = 10;
                    _winRowLength = 5;
                    Generate(Size * Size);
                    break;
            }
        }

        public void Generate(int size)
        {
            _field.Add(new GameCell(new int[] { 5, 7, 1, 3 }));
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
                            bottom = (sbyte)((-1)*(top - 8));
                            break;
                        case 1://right side
                            left = (sbyte)((-1) * (right - 8)); 
                            top = -1;
                            bottom = 1;
                            break;
                        case 2://bottom side
                            left = 1;
                            top = (sbyte)((-1) * (bottom - 8));
                            right = -1;
                            break;
                        case 3://left side
                            top = 1;
                            right = (sbyte)((-1) * (left - 8));
                            bottom = -1;
                            break;
                    }
                }


                _field.Add(new GameCell(new int[] { left, top, right, bottom }));

            }

            SetIndexes();
        }

        private void SetIndexes()
        {
            int currentUp = 0;
            int currentDown = 0;
            Field[currentUp].Index = Size * Size / 2;
            for (int i = 0; i <= (Size - 1) / 2; i++)
            {
                SetRowIndexes(currentUp);
                SetRowIndexes(currentDown);
                if(currentUp + Field[currentUp].Neighbours[1] < Size * Size)
                {
                    Field[currentUp + Field[currentUp].Neighbours[1]].Index = Field[currentUp].Index - Size;
                    currentUp += Field[currentUp].Neighbours[1];
                }
                if (currentDown + Field[currentDown].Neighbours[3] < Size * Size)
                {
                    Field[currentDown + Field[currentDown].Neighbours[3]].Index = Field[currentDown].Index + Size;
                    currentDown += Field[currentDown].Neighbours[3];
                }
            }
        }

        private void SetRowIndexes(int start)
        {
            int currentLeft = start;
            int currentRight = start;
            for (int i = 0;i < (Size - 1) / 2;i++)
            {
                SetLeftIndex(currentLeft);
                SetRightIndex(currentRight);
                currentLeft += Field[currentLeft].Neighbours[0];
                currentRight += Field[currentRight].Neighbours[2];
            }

        }

        private void SetRightIndex(int current)
        {
            int leftNeighbour = current + Field[current].Neighbours[2];
            Field[leftNeighbour].Index = Field[current].Index + 1;
        }

        private void SetLeftIndex(int current)
        {
            int leftNeighbour = current + Field[current].Neighbours[0];
            Field[leftNeighbour].Index = Field[current].Index - 1;
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
