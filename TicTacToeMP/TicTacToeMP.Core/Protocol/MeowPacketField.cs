using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeMP.Core.Protocol
{
    /// <summary>
    /// Поле пакета
    /// </summary>
    public class MeowPacketField
    {
        /// <summary>
        /// Идентификатор свойства
        /// </summary>
        public byte FieldID { get; set; }
        /// <summary>
        /// Размер поля в байтах
        /// </summary>
        public byte FieldSize { get; set; }
        /// <summary>
        /// Значение поля
        /// </summary>
        public byte[]? Contents { get; set; }
    }
}
