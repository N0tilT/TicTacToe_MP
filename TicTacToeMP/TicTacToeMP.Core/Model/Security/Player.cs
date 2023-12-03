using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeMP.Core.Model.Security
{
    public class Player
    {
        private static int counter=0;
        private int _id;
        private string _name;

        public int Id { get { return _id; } }
        public string Name { get { return _name; } }

        public Player(string name)
        {
            _id = counter++;
            _name = name;
        }
    }
}
