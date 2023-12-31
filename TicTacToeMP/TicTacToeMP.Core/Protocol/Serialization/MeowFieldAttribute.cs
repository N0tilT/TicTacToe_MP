﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeMP.Core.Protocol.Serialization
{
    /// <summary>
    /// Класс атрибута сериализации пакета
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class MeowFieldAttribute : Attribute
    {
            public byte FieldID { get; }

            public MeowFieldAttribute(byte fieldId)
            {
                FieldID = fieldId;
            }
        }
    }
