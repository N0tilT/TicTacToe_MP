using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeMP.Core.Protocol.Serialization;

namespace TicTacToeMP.Core.Protocol
{
    public class MeowPacketLobbyConnectionResponse
    {
        [MeowField(1)]
        public string Response;

        [MeowField(2)]
        public string PlayerOneString;

        [MeowField(3)]
        public string PlayerTwoString;
    }
}
