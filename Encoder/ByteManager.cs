using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAFactory.Fx.Utilities.Encoding
{
    public class ByteManager
    {
        public static List<byte> ChopByteArray(List<byte> bytes, long position, int count)
        {
            return ChopByteArray(bytes.ToArray(), position, count).ToList();
        }

        public static byte[] ChopByteArray(byte[] array, long position, int count)
        {
            if (array.Length < (position + count))
            {
                throw new IndexOutOfRangeException("The number of elements exeeds the total elements of the array");
            }

            byte[] result = new byte[count];

            for (int i = 0; i < count; ++i)
            {
                result[i] = array[position + i];
            }

            return result;
        }

        public static string GetStringValue(byte[] array)
        {
            if (array[array.Length - 1] == 0x0)
            {
                array = ByteManager.ChopByteArray(array, 0, array.Length - 1);
            }
            return System.Text.Encoding.UTF8.GetString(array);
        }
        public static short GetShortValue(byte[] array)
        {
            return BitConverter.ToInt16(array, 0);
        }
        public static ushort GetUShortValue(byte[] array)
        {
            return BitConverter.ToUInt16(array, 0);
        }
        public static uint GetUIntValue(byte[] array)
        {
            return BitConverter.ToUInt16(array, 0);
        }
        public static int GetIntValue(byte[] array)
        {
            return BitConverter.ToInt16(array, 0);
        }
        public static int GetSIntValue(byte[] array)
        {
            return GetIntValue(array);
        }
        public static long GetLongValue(byte[] array)
        {
            return BitConverter.ToInt32(array, 0); ;
        }
        public static ulong GetULongValue(byte[] array)
        {
            return BitConverter.ToUInt32(array, 0); ;
        }
        public static long GetSLongValue(byte[] array)
        {
            return GetLongValue(array); ;
        }

        public static Rational GetRationalValue(byte[] array)
        {
            Rational result = new Rational();

            result.Numerator = BitConverter.ToUInt16(array, 0);
            result.Denominator = BitConverter.ToUInt16(array, 4);

            return result;
        }
        public static Rational GetSRationalValue(byte[] array)
        {
            Rational result = new Rational();

            result.Numerator = BitConverter.ToInt16(array, 0);
            result.Denominator = BitConverter.ToInt16(array, 4);

            return result;
        }

        public static T GetValue<T>(byte[] array)
        {
            var result = new object();
            string typeName = typeof(T).ToString().ToUpper().Substring(typeof(T).ToString().LastIndexOf('.') + 1);
            switch (typeName)
            {
                case "INT16":
                case "TAGTYPECODE":
                case "VALUEELEMENTTYPE":
                    result = GetShortValue(array);
                    break;
                case "USHORT":
                case "UINT16":
                    result = GetUShortValue(array);
                    break;
                case "UINT32":
                    result = GetUIntValue(array);
                    break;
                case "INT32":
                    result = GetIntValue(array);
                    break;
                case "RATIONAL":
                    result = GetRationalValue(array);
                    break;
                case "SRATIONAL":
                    result = GetSRationalValue(array);
                    break;
                case "INT64":
                case "LONG":
                case "SLONG":
                    result = GetLongValue(array);
                    break;
                case "UINT64":
                case "ULONG":
                    result = GetULongValue(array);
                    break;
                case "STRING":
                    result = GetStringValue(array);
                    break;
                default:
                    throw new InvalidCastException("Cast not supported by the library.");
            };
            return (T)result;
        }

        public string GetByteListAsString(byte[] array)
        {
            return string.Empty;

        }
    }
}
