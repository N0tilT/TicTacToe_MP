using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeMP.Core.Protocol.Serialization;

namespace TicTacToeMP.Core.Model.Game
{
    public class MeowPacketLobbyConnect 
    {
        [MeowField(1)]
        public string Player;
    }
}
