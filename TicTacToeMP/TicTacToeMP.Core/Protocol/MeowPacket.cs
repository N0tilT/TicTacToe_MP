using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeMP.Core.Protocol
{
    /// <summary>
    /// Класс пакета протокола MeowProtocol
    /// </summary>
    public class MeowPacket
    {
        /// <summary>
        /// Тип пакета
        /// </summary>
        public byte PacketType { get; private set; }

        /// <summary>
        /// Подтип пакета
        /// </summary>
        public byte PacketSubtype { get; private set; }

        /// <summary>
        /// Список полей пакета - данные
        /// </summary>
        public List<MeowPacketField> Fields { get; set; } = new List<MeowPacketField>();

        /// <summary>
        /// Флаг защищённости пакета
        /// </summary>
        public bool Protected { get; set; }

        /// <summary>
        /// Флаг необходимости замены заголовка
        /// </summary>
        private bool ChangeHeaders { get; set; }

        private MeowPacket() { }

        #region CreatePacket
        /// <summary>
        /// Метод создания единственного экземпляра пакета по типу
        /// </summary>
        /// <param name="type">тип создаваемого пакета</param>
        /// <returns>статичный пакет</returns>
        public static MeowPacket Create(MeowPacketType type)
        {
            var t = MeowPacketTypeManager.GetBytesOfType(type);
            return Create(t.Item1, t.Item2);
        }

        /// <summary>
        /// Метод создания единственного экземпляра пакета по байтам типа и подтипа пакета
        /// </summary>
        /// <param name="type">байт тип создаваемого пакета</param>
        /// <param name="subtype">байт подтипа создаваемого пакета</param>
        /// <returns>статичный пакет</returns>
        public static MeowPacket Create(byte type, byte subtype)
        {
            return new MeowPacket
            {
                PacketType = type,
                PacketSubtype = subtype
            };
        }

        #endregion

        #region Getters`n`Setters
        /// <summary>
        /// Получить поле пакета по ID
        /// </summary>
        /// <param name="id">id поля</param>
        /// <returns>поле пакета</returns>
        public MeowPacketField GetField(byte id)
        {
            return Fields.Find(field => field.FieldID == id);
        }

        /// <summary>
        /// Проверка на существование поля в пакете
        /// </summary>
        /// <param name="id">id поля</param>
        /// <returns>true, если у пакета есть искомое поле</returns>
        public bool HasField(byte id)
        {
            return GetField(id) != null;
        }

        /// <summary>
        /// Геттер поля пакета
        /// </summary>
        /// <typeparam name="T">Тип получаемого значения</typeparam>
        /// <param name="id">id искомого поля</param>
        /// <returns>Значение поля типа Т</returns>
        /// <exception cref="Exception"></exception>
        public T GetValue<T>(byte id) where T : struct
        {
            var field = GetField(id) ?? throw new Exception($"Field with ID {id} wasn't found.");

            var neededSize = Marshal.SizeOf(typeof(T));
            if (field.FieldSize != neededSize)
            {
                throw new Exception($"Can't convert field to type {typeof(T).FullName}.\n" +
                                    $"We have {field.FieldSize} bytes but we need exactly {neededSize}.");
            }

            return ByteArrayToFixedObject<T>(field.Contents);
        }
        /// <summary>
        /// Сеттер поля пакета
        /// </summary>
        /// <param name="id">id искомого поля</param>
        /// <param name="structure">Value-type записываемое значение</param>
        /// <exception cref="Exception"></exception>
        public void SetValue(byte id, object structure)
        {
            if (!structure.GetType().IsValueType)
            {
                throw new Exception("Only value types are available.");
            }

            //Добавляем поле, если его нет в пакете
            var field = GetField(id);

            if (field == null)
            {
                field = new MeowPacketField
                {
                    FieldID = id
                };

                Fields.Add(field);
            }

            //Устанавливаем размер поля и значение в виде массива байтов
            var bytes = FixedObjectToByteArray(structure);

            if (bytes.Length > byte.MaxValue)
            {
                throw new Exception("Object is too big. Max length is 255 bytes.");
            }

            field.FieldSize = (byte)bytes.Length;
            field.Contents = bytes;
        }

        /// <summary>
        /// Геттер массива данных
        /// </summary>
        /// <param name="id">id свойства</param>
        /// <returns>массив данных байтов</returns>
        /// <exception cref="Exception"></exception>
        public byte[] GetValueRaw(byte id)
        {
            var field = GetField(id);

            if (field == null)
            {
                throw new Exception($"Field with ID {id} wasn't found.");
            }

            return field.Contents;
        }

        /// <summary>
        /// Сеттер, принимающий массив данных
        /// </summary>
        /// <param name="id">id свойства</param>
        /// <param name="rawData">массив данных</param>
        /// <exception cref="Exception"></exception>
        public void SetValueRaw(byte id, byte[] rawData)
        {
            var field = GetField(id);

            if (field == null)
            {
                field = new MeowPacketField
                {
                    FieldID = id
                };

                Fields.Add(field);
            }

            if (rawData.Length > byte.MaxValue)
            {
                throw new Exception("Object is too big. Max length is 255 bytes.");
            }

            field.FieldSize = (byte)rawData.Length;
            field.Contents = rawData;
        }
        #endregion

        #region ByteValueConverter
        /// <summary>
        /// Преобразование массива байтов в объект
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="bytes">массив байтов для преобразования</param>
        /// <returns>Объект указанного типа</returns>
        private T ByteArrayToFixedObject<T>(byte[] bytes) where T : struct
        {
            T structure;

            //Закрепляем массив байтов в памяти
            var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);

            //Преобразовываем закреплённый массив байтов в объект
            try
            {
                structure = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            }
            finally
            {
                handle.Free();
            }

            return structure;
        }

        /// <summary>
        /// Преобразование объекта фиксированной длины в массив байтов
        /// </summary>
        /// <param name="value">объект фиксированной длины</param>
        /// <returns>массив байтов</returns>
        public byte[] FixedObjectToByteArray(object value)
        {
            //Получаем размер нашего объекта из unmanaged кода
            var rawsize = Marshal.SizeOf(value);
            var rawdata = new byte[rawsize];

            //Запрещаем GC удалять созданный объект
            var handle =
                GCHandle.Alloc(rawdata,
                    GCHandleType.Pinned);

            //Получаем указатель на созданный массив и заполняем его значениями объекта
            Marshal.StructureToPtr(value,
                handle.AddrOfPinnedObject(),
                false);

            //Освобождаем выделенную ранее память
            handle.Free();
            return rawdata;
        }
        #endregion

        #region PacketParser

        /// <summary>
        /// Преобразование пакета в массив байтов
        /// </summary>
        /// <returns>массив байтов - готовый для отправки пакет</returns>
        public byte[] ToPacket()
        {
            var packet = new MemoryStream();

            //Начало пакета
            packet.Write(
                ChangeHeaders
                    ? new byte[] { 0x95, 0xAA, 0xFF, PacketType, PacketSubtype }
                    : new byte[] { 0xAF, 0xAA, 0xAF, PacketType, PacketSubtype }, 0, 5);

            // Сортируем поля по ID
            var fields = Fields.OrderBy(field => field.FieldID);

            // Записываем поля
            foreach (var field in fields)
            {
                packet.Write(new[] { field.FieldID, field.FieldSize }, 0, 2);
                packet.Write(field.Contents, 0, field.Contents.Length);
            }

            // Окончание пакета
            packet.Write(new byte[] { 0xFF, 0x00 }, 0, 2);

            return packet.ToArray();
        }

        /// <summary>
        /// Преобразование массива байтов в пакет
        /// </summary>
        /// <param name="packet">пакет в виде массива байтов</param>
        /// <param name="markAsEncrypted">были ли зашифрованы данные</param>
        /// <returns>пакет</returns>
        public static MeowPacket? Parse(byte[] packet, bool markAsEncrypted = false)
        {
            /*
             * Минимальный размер пакета - 7 байт
             * HEADER (3) + TYPE (1) + SUBTYPE (1) + PACKET ENDING (2)
             */
            if (packet.Length < 7)
            {
                return null;
            }

            var encrypted = false;

            // Проверяем заголовок
            if (packet[0] != 0xAF ||
                packet[1] != 0xAA ||
                packet[2] != 0xAF)
            {
                // Проверка защищённого пакета
                if (packet[0] == 0x95 ||
                    packet[1] == 0xAA ||
                    packet[2] == 0xFF)
                {
                    encrypted = true;
                }
                else
                {
                    return null;
                }
            }

            var mIndex = packet.Length - 1;

            // Проверяем окончание
            if (packet[mIndex - 1] != 0xFF ||
                packet[mIndex] != 0x00)
            {
                return null;
            }

            var type = packet[3];
            var subtype = packet[4];

            //Создаём новый объект пакета на основе полученного типа,подтипа
            var meowPacket = new MeowPacket { PacketType = type, PacketSubtype = subtype, Protected = markAsEncrypted };

            //Обработка полей пакета байтов, начиная с 5 элемента
            var fields = packet.Skip(5).ToArray();
            while (true)
            {
                //Поля пакета пусты
                if (fields.Length == 2)
                {
                    return encrypted ? DecryptPacket(meowPacket) : meowPacket;
                }

                //Обрабатываем доступные поля
                var fieldId = fields[0];
                var fieldSize = fields[1];

                //Читаем содержимое пакета байтов
                var contents = fieldSize != 0 ?
                    fields.Skip(2).Take(fieldSize).ToArray() : null;

                //Записываем
                meowPacket.Fields.Add(new MeowPacketField
                {
                    FieldID = fieldId,
                    FieldSize = fieldSize,
                    Contents = contents
                });

                //Переходим к следующему полю пакета байтов
                fields = fields.Skip(2 + fieldSize).ToArray();
            }
        }
        #endregion

        #region PacketEncrypter

        /// <summary>
        /// Шифровка пакета
        /// </summary>
        /// <returns>Зашифрованный квадрат</returns>
        public MeowPacket Encrypt()
        {
            return EncryptPacket(this);
        }

        /// <summary>
        /// Дешифровка пакета
        /// </summary>
        /// <returns>дешифрованный пакет</returns>
        public MeowPacket Decrypt()
        {
            return DecryptPacket(this);
        }

        /// <summary>
        /// Шифровка пакета
        /// </summary>
        /// <param name="packet">пакет, который нужно зашифровать</param>
        /// <returns>Пакет, содержащий зашифрованный пакет</returns>
        public static MeowPacket? EncryptPacket(MeowPacket packet)
        {
            if (packet == null)
            {
                return null;
            }


            //Получаем пакет в байтах и шифруем его
            var rawBytes = packet.ToPacket();
            var encrypted = MeowProtocolEncryptor.Encrypt(rawBytes);

            //Создаём новый пакет, оборачивающий зашифрованный и меняем его заголовок
            var p = Create(0, 0);
            p.SetValueRaw(0, encrypted);
            p.ChangeHeaders = true; 

            return p;
        }

        private static MeowPacket? DecryptPacket(MeowPacket packet)
        {
            //Зашифрованные данные располагаются в 0-м свойстве
            if (!packet.HasField(0))
            {
                return null;
            }

            //Зашифрованный пакет расшифровывается и преобразовывается из массива байтов в пакет
            var rawData = packet.GetValueRaw(0);
            var decrypted = MeowProtocolEncryptor.Decrypt(rawData);

            return Parse(decrypted, true);
        }
        #endregion

    }
}
