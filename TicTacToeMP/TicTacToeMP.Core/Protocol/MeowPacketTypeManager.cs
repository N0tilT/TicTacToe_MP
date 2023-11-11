using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeMP.Core.Protocol
{
    public class MeowPacketTypeManager
    {
        private static readonly Dictionary<MeowPacketType, Tuple<byte, byte>> TypeDictionary =
            new Dictionary<MeowPacketType, Tuple<byte, byte>>();

        static MeowPacketTypeManager()
        {
            RegisterType(MeowPacketType.Handshake, 1, 0);
        }

        public static void RegisterType(MeowPacketType type, byte btype, byte bsubtype)
        {
            if (TypeDictionary.ContainsKey(type))
            {
                throw new Exception($"Packet type {type:G} is already registered.");
            }

            TypeDictionary.Add(type, Tuple.Create(btype, bsubtype));
        }

        public static Tuple<byte, byte> GetType(MeowPacketType type)
        {
            if (!TypeDictionary.ContainsKey(type))
            {
                throw new Exception($"Packet type {type:G} is not registered.");
            }

            return TypeDictionary[type];
        }

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
