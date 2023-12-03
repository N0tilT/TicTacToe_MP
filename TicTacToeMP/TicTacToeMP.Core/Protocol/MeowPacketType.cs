using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeMP.Core.Protocol
{
    /// <summary>
    /// Возможные типы пакетов
    /// </summary>
    public enum MeowPacketType
    {
        Unknown,
        Handshake,
        Turn,
        LobbyConnect,
        LobbyList,
        LobbyConnectionResponse
    }
}
