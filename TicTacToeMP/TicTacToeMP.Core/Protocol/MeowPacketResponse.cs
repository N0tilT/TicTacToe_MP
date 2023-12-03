using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeMP.Core.Protocol.Serialization;

namespace TicTacToeMP.Core.Protocol
{
    public class MeowPacketResponse
    {
        [MeowField(1)]
        public string Response;
    }
}
