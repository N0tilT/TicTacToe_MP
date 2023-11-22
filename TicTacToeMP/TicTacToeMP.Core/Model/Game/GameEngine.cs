using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeMP.Core.Model.Security;

namespace TicTacToeMP.Core.Model.Game
{
    public class GameEngine
    {

        private List<Player> _players;
        private List<int> _scores;
        private GameField _gameField;

        public List<Player> Players { get { return _players; } }
        public List<int> Scores { get { return _scores; } }
        public GameField Field { get { return _gameField; } }

        public GameEngine()
        {
            _players = new List<Player>()
            {
                new Player("Player 1"),
                new Player("Player 2"),
            };

            _scores = new List<int>()
            {
                0,0
            };

            _gameField = new GameField();
        }

        public void AddWinPoint(int id)
        {
            if(id >= 0 && id < 2)
            {
                _scores[id] += 1;
            }
        }

        public bool CheckWinner()
        {
            int countElements = Field.Size * 4 - 4;
            bool[] checkedFields = new bool[countElements];

            for (int i = 0; i < countElements; i++)
            {
                checkedFields[i] = false;
            }

            for (int i = 0; i < countElements; i++)
            {
                GameCellState currentCellState = Field.Field[i].State;
            }

            return false;
        }

        public void SetPlayers(List<Player> players)
        {
            if(players.Count != 2)
            {
                throw new ArgumentException();
            }

            _players = players;
        }


    }
}
