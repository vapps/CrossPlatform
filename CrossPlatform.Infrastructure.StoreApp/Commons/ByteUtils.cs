using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossPlatform.Infrastructure.StoreApp.Commons
{
    public class ByteUtils
    {
        const int ODD = 1;
        const int PAIR = 2;

        public static string byteToHex(byte buf)
        {
            return String.Format("{0:X2}", (Convert.ToInt32(buf)));
        }

        public static string byteToHex(byte[] buf)
        {
            StringBuilder _sb = new StringBuilder();

            foreach (byte _tmp in buf)
            {
                _sb.Append(ByteUtils.byteToHex(_tmp));
            }

            return _sb.ToString();
        }

        public static byte[] hexToByte(string buf)
        {
            if (isOdd(buf.Length))
            {
                return null;
            }

            int _len = buf.Length / PAIR;
            byte[] _buf = new byte[_len];
            string _strChunk = null;

            for (int i = 0; i < _len; i++)
            {
                _strChunk = buf.Substring(i * 2, 2);

                _buf[i] = (byte)Convert.ToInt32(_strChunk, 16);
            }

            return _buf;
        }

        public static bool isOdd(int len)
        {
            return !ByteUtils.isPair(len);
        }

        public static bool isPair(int len)
        {
            if ((len % PAIR) == ODD)
                return false;

            return true;
        }

        public static bool isPrintable(char input)
        {
            // 'space' to '~' is valid character
            if (input < 0x20 || input > 0x7E) return false;

            return true;
        }

        public static byte[] extractBuffer(byte[] data, int offset, int length)
        {
            byte[] _tmp = new byte[length];

            System.Array.Copy(data, offset, _tmp, 0, length);

            return _tmp;
        }

        public static byte[] changeToNetworkOrdering(byte[] input)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(input);

            return input;
        }

        public static short byteToShort(byte[] input)
        {
            return byteToShort(input, 0);
        }

        public static short byteToShort(byte[] input, int pos)
        {
            return (short)BitConverter.ToUInt16(input, pos);
        }

        public static int byteToInt(byte[] input)
        {
            return byteToInt(input, 0);
        }

        public static int byteToInt(byte[] input, int pos)
        {
            return (int)BitConverter.ToUInt32(input, pos);
        }

        public static long byteToLong(byte[] input)
        {
            return byteToLong(input, 0);
        }

        public static long byteToLong(byte[] input, int pos)
        {
            return (long)BitConverter.ToUInt64(input, pos);
        }

        public static float byteToFloat(byte[] input)
        {
            return byteToFloat(input, 0);
        }

        public static float byteToFloat(byte[] input, int pos)
        {
            return (float)BitConverter.ToSingle(input, pos);
        }

        public static double byteToDouble(byte[] input)
        {
            return byteToDouble(input, 0);
        }

        public static double byteToDouble(byte[] input, int pos)
        {
            return (double)BitConverter.ToDouble(input, pos);
        }

        public static byte[] shortToByte(short input)
        {
            byte[] _tmp = BitConverter.GetBytes(input);

            ByteUtils.changeToNetworkOrdering(_tmp);

            return _tmp;
        }

        public static byte[] intToByte(int input)
        {
            byte[] _tmp = BitConverter.GetBytes(input);

            ByteUtils.changeToNetworkOrdering(_tmp);

            return _tmp;
        }

        public static byte[] longToByte(long input)
        {
            byte[] _tmp = BitConverter.GetBytes(input);

            ByteUtils.changeToNetworkOrdering(_tmp);

            return _tmp;
        }

        public static byte[] floatToByte(float input)
        {
            byte[] _tmp = BitConverter.GetBytes(input);

            ByteUtils.changeToNetworkOrdering(_tmp);

            return _tmp;
        }

        public static byte[] doubleToByte(double input)
        {
            byte[] _tmp = BitConverter.GetBytes(input);

            ByteUtils.changeToNetworkOrdering(_tmp);

            return _tmp;
        }

        public static string getString(byte[] input)
        {
            return System.Text.Encoding.GetEncoding("euc-kr").GetString(input, 0, input.Length);
        }

        public static byte[] getBytes(string input)
        {
            return System.Text.Encoding.GetEncoding("euc-kr").GetBytes(input);
        }
    }
}
