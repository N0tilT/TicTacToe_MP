using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeMP.Core.Protocol.Serialization
{
    public class MeowPacketConverter
    {
        public static MeowPacket Serialize(MeowPacketType type, object obj, bool strict = false)
        {
            var t = MeowPacketTypeManager.GetType(type);
            return Serialize(t.Item1, t.Item2, obj, strict);
        }

        public static MeowPacket Serialize(byte type, byte subtype, object obj, bool strict = false)
        {
            var fields = GetFields(obj.GetType());

            if (strict)
            {
                var usedUp = new List<byte>();

                foreach (var field in fields)
                {
                    if (usedUp.Contains(field.Item2))
                    {
                        throw new Exception("One field used two times.");
                    }

                    usedUp.Add(field.Item2);
                }
            }

            var packet = MeowPacket.Create(type, subtype);

            foreach (var field in fields)
            {
                packet.SetValue(field.Item2, field.Item1.GetValue(obj));
            }

            return packet;
        }

        public static T Deserialize<T>(MeowPacket packet, bool strict = false)
        {
            var fields = GetFields(typeof(T));
            var instance = Activator.CreateInstance<T>();

            if (fields.Count == 0)
            {
                return instance;
            }

            foreach (var tuple in fields)
            {
                var field = tuple.Item1;
                var packetFieldId = tuple.Item2;

                if (!packet.HasField(packetFieldId))
                {
                    if (strict)
                    {
                        throw new Exception($"Couldn't get field[{packetFieldId}] for {field.Name}");
                    }

                    continue;
                }

                var value = typeof(MeowPacket)
                    .GetMethod("GetValue")?
                    .MakeGenericMethod(field.FieldType)
                    .Invoke(packet, new object[] { packetFieldId });

                if (value == null)
                {
                    if (strict)
                    {
                        throw new Exception($"Couldn't get value for field[{packetFieldId}] for {field.Name}");
                    }

                    continue;
                }

                field.SetValue(instance, value);
            }

            return instance;
        }

        private static List<Tuple<FieldInfo, byte>> GetFields(Type t)
        {
            return t.GetFields(BindingFlags.Instance |
                                     BindingFlags.NonPublic |
            BindingFlags.Public)
                .Where(field => field.GetCustomAttribute<MeowFieldAttribute>() != null)
            .Select(field => Tuple.Create(field, field.GetCustomAttribute<MeowFieldAttribute>().FieldID))
                .ToList();
        }
    }
}
