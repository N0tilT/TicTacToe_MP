using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeMP.Core.Model.Security;
using TicTacToeMP.Core.Model.ServerCore;

namespace TicTacToeMP.Core.Model.Game
{
    public class Lobby
    {
        public Lobby(GameMode gameMode) 
        {
            Game = new GameEngine(gameMode);
        }
        public Lobby(Player playerOne, Player playerTwo,GameCellState playerOneSide, GameCellState playerTwoSide, GameMode gameMode):this(gameMode)
        {
            PlayerOne = playerOne;
            PlayerTwo = playerTwo;
            PlayerOneSide = playerOneSide;
            PlayerTwoSide = playerTwoSide;
        }

        public Player PlayerOne { get; set; }
        public ClientHandler PlayerOneClient { get; set; }
        public Player PlayerTwo { get; set; }
        public ClientHandler PlayerTwoClient { get; set; }
        public GameEngine Game { get; set; }
        public GameCellState PlayerOneSide { get; set; }
        public GameCellState PlayerTwoSide { get; set; }

        public void Join(Player player, ClientHandler client)
        {
            if (PlayerOne == null) 
            {
                PlayerOne = player;
                PlayerOneClient = client;
                PlayerOneSide = GameCellState.Cross;
                return;
            }
            if(PlayerTwo == null) 
            {
                PlayerTwo = player;
                PlayerTwoClient = client;
                PlayerTwoSide = GameCellState.Nought;
                return;
            }
        }
        public bool isFull()
        {
            return PlayerOne != null && PlayerTwo != null;
            
        }
    }
}
