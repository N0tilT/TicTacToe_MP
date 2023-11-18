using System;
using TicTacToeMP.Server.Core;

namespace TicTacToeMP.Server
{
    internal class Program
    {
        private static void Main()
        {
            Console.Title = "MeowServer";
            Console.ForegroundColor = ConsoleColor.White;

            var server = new TicTacToeServer();
            server.Start();
            server.AcceptClients();
        }
    }
}
