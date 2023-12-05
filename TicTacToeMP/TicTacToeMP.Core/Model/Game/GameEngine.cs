﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using TicTacToeMP.Core.Model.Security;

namespace TicTacToeMP.Core.Model.Game
{
    public enum GameMode
    {
        Endless,Limited
    }
    public class GameEngine
    {
        private GameField _gameField;   
        public GameField Field { get { return _gameField; } }

        public GameEngine()
        {
            _gameField = new GameField();
        }

        public GameEngine(GameMode mode):base()
        {
            if(mode == GameMode.Limited)
            {
                _gameField = new LimitedGameField(LimitedFieldSize.ThreeByThree);
            }
            else
            {
                _gameField = new EndlessGameField(5);

            }
        }
        public GameEngine(GameMode mode, LimitedFieldSize size)
        {
            if (mode == GameMode.Limited)
            {
                _gameField = new LimitedGameField(size);
            }
            else
            {
                _gameField = new EndlessGameField(size);
            }
        }


        public void MakeTurn(Turn turn)
        {
            //if (Field.Field[id].State != GameCellState.Empty)
            //{
            //    throw new ArgumentException();
            //}
            switch (turn.CellState)
            {
                case GameCellState.Nought:
                    Field.Field[turn.CellIndex].SetNought();
                    break;
                case GameCellState.Cross:
                    Field.Field[turn.CellIndex].SetCross();
                    break;
            }
        }

        public bool IsWinSignPlaced(int lastPlacedId)
        {
            int[] neighbours = Field.Field[lastPlacedId].Neighbours;
            byte sum = 1;
            for (int i=0; i< 2; i++)
            {
                sum += CheckNeighbour(i, lastPlacedId);
                if(sum < Field.WinRowLength)
                {
                    sum += CheckNeighbour(i + 2, lastPlacedId);
                }
                if(sum == Field.WinRowLength)
                {
                    return true;
                }

                sum = 1;
                sum += CheckNeighbourDiagonal(i,lastPlacedId, lastPlacedId + neighbours[i]);
                if(sum < Field.WinRowLength)
                {
                    sum += CheckNeighbourDiagonal(i + 2,lastPlacedId, lastPlacedId + neighbours[i + 2]);
                }
                if(sum == Field.WinRowLength)
                {
                    return true;
                }

                sum = 0;
            }


            return false;
        }

        private byte CheckNeighbourDiagonal(int i, int current, int neighbour)
        {
            if(neighbour >= Field.Field.Count) { return 0; }
            int diagonalNeighbour = Field.Field[neighbour].Neighbours[i< Field.Field[i].Neighbours.Length -1 ? i + 1:0];
            if(neighbour + diagonalNeighbour >= Field.Field.Count) { return 1; }
            if (Field.Field[current].State != Field.Field[neighbour + diagonalNeighbour].State)
            {
                return 0;
            }

            return (byte)(1 + 
                CheckNeighbourDiagonal(i, neighbour+diagonalNeighbour, 
                neighbour + diagonalNeighbour + Field.Field[neighbour + diagonalNeighbour].Neighbours[i]));
        }
        
        private byte CheckNeighbour(int i, int current)
        {
            if(current >= Field.Field.Count) return 0;
            if (current + Field.Field[current].Neighbours[i] >= Field.Field.Count) { return 0; }
            if (Field.Field[current].State != Field.Field[current + Field.Field[current].Neighbours[i]].State)
            {
                return 0;
            }

            return (byte)(1 + CheckNeighbour(i, current + Field.Field[current].Neighbours[i]));


        }

    }
}
