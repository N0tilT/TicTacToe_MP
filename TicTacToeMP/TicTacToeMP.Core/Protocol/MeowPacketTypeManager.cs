using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeMP.Core.Protocol
{
    /// <summary>
    /// Статический менеджер типов пакетов.
    /// </summary>
    public static class MeowPacketTypeManager
    {
        /// <summary>
        /// Словарь, где ключом выступает тип пакета, а значением пара байта типа и байта подтипа пакета
        /// </summary>
        private static readonly Dictionary<MeowPacketType, Tuple<byte, byte>> TypeDictionary =
            new Dictionary<MeowPacketType, Tuple<byte, byte>>();

        static MeowPacketTypeManager()
        {
            RegisterType(MeowPacketType.Handshake, 1, 0);
            RegisterType(MeowPacketType.Turn, 2, 0);
            RegisterType(MeowPacketType.LobbyConnect, 3, 0);
            RegisterType(MeowPacketType.LobbyList, 3, 1);
            RegisterType(MeowPacketType.LobbyConnectionResponse, 4, 0);
        }

        /// <summary>
        /// Регистрация нового типа пакета
        /// </summary>
        /// <param name="type">тип пакета</param>
        /// <param name="btype">байт подтипа пакета</param>
        /// <param name="bsubtype">байт подтипа пакета</param>
        /// <exception cref="Exception"></exception>
        public static void RegisterType(MeowPacketType type, byte btype, byte bsubtype)
        {
            if (TypeDictionary.ContainsKey(type))
            {
                throw new Exception($"Packet type {type:G} is already registered.");
            }

            TypeDictionary.Add(type, Tuple.Create(btype, bsubtype));
        }

        /// <summary>
        /// Получение информации о типе пакета
        /// </summary>
        /// <param name="type">тип пакета</param>
        /// <returns>пара байтов типа и подтипа пакета</returns>
        /// <exception cref="Exception"></exception>
        public static Tuple<byte, byte> GetBytesOfType(MeowPacketType type)
        {
            if (!TypeDictionary.ContainsKey(type))
            {
                throw new Exception($"Packet type {type:G} is not registered.");
            }

            return TypeDictionary[type];
        }

        /// <summary>
        /// Получение типа пакета
        /// </summary>
        /// <param name="packet">пакет</param>
        /// <returns>тип пакета</returns>
        public static MeowPacketType GetTypeFromPacket(MeowPacket packet)
        {
            var type = packet.PacketType;
            var subtype = packet.PacketSubtype;

            foreach (var tuple in TypeDictionary)
            {
                var value = tuple.Value;

                if (value.Item1 == type && value.Item2 == subtype)
                {
                    return tuple.Key;
                }
            }

            return MeowPacketType.Unknown;
        }
    }
}
