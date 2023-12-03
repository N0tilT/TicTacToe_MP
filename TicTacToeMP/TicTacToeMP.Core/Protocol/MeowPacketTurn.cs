using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeMP.Core.Protocol.Serialization;

namespace TicTacToeMP.Core.Protocol
{
    public class MeowPacketTurn
    {
        [MeowField(1)]
        public string Player;

        [MeowField(2)]
        public string TurnString;

    }
}
