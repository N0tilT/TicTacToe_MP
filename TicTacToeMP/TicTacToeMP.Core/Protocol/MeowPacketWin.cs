using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeMP.Core.Protocol.Serialization;

namespace TicTacToeMP.Core.Protocol
{
    public class MeowPacketWin
    {
        [MeowField(1)]
        public string Winner;
    }
}
