using System;
using System.Text;

namespace MHXXSaveEditor.Util
{
    public class StringUtils
    {
        /* Bytes --> hexadecimal string */
        public static string BytesToHexString(byte[] bytes) {
            string str = "";
            foreach (var bit in bytes) {
                str += bit.ToString("X2");
            }
            return str;
        }

        /* Hexadecimal string --> bytes */
        public static byte[] HexStringToBytes(string hex) {
            byte[] bytes = new byte[hex.Length / 2];

            for (int i = 0; i < hex.Length; i += 2) {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }

        /* Bytes --> readable UTF-8 string */
        public static string BytesToUtf8String(byte[] bytes) {
                return Encoding.UTF8.GetString(bytes);
        }

        /* Bytes --> RGBA as hexadecimal string */
        public static string ColorBytesToString(byte[] rgba) {
            return BitConverter.ToString(rgba).Replace("-", string.Empty);
        }
    }
}
