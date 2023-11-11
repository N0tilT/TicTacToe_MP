using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeMP.Core.Protocol.Crypto;

namespace TicTacToeMP.Core.Protocol
{
    /// <summary>
    /// Обработчик шифрования пакетов
    /// </summary>
    public class MeowProtocolEncryptor
    {
        /// <summary>
        /// Ключ шифрования
        /// </summary>
        private static string Key { get; } = "2e985f930";

        /// <summary>
        /// Шифровка массива данных
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] data)
        {
            return RijndaelHandler.Encrypt(data, Key);
        }

        /// <summary>
        /// Дешифровка массива данных
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] data)
        {
            return RijndaelHandler.Decrypt(data, Key);
        }
    }
}
