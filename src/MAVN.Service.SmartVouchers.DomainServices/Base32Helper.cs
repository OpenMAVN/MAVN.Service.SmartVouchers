using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MAVN.Service.SmartVouchers.DomainServices
{
    internal static class Base32Helper
    {
        private const string SEPARATOR = "-";

        private static readonly char[] DIGITS;
        private static readonly int MASK;
        private static readonly int SHIFT;
        private static Dictionary<char, int> CHAR_MAP = new Dictionary<char, int>();

        static Base32Helper()
        {
            DIGITS = "ABCDEFGHIJKLMNPQRSTUVWXYZ2345678".ToCharArray();
            MASK = DIGITS.Length - 1;
            SHIFT = GetNumberOfTrailingZeros(DIGITS.Length);
            for (var i = 0; i < DIGITS.Length; i++)
                CHAR_MAP[DIGITS[i]] = i;
        }

        private static int GetNumberOfTrailingZeros(int i)
        {
            if (i == 0) return 32;
            // HD, Figure 5-14
            int y;
            var n = 31;
            y = i << 16;
            if (y != 0) { n = n - 16; i = y; }
            y = i << 8;
            if (y != 0) { n = n - 8; i = y; }
            y = i << 4;
            if (y != 0) { n = n - 4; i = y; }
            y = i << 2;
            if (y != 0) { n = n - 2; i = y; }
            return n - (int)((uint)(i << 1) >> 31);
        }

        public static byte[] Decode(string encoded)
        {
            // Remove whitespace and separators
            encoded = encoded.Trim().Replace(SEPARATOR, "");

            // Remove padding. Note: the padding is used as hint to determine how many
            // bits to decode from the last incomplete chunk (which is commented out
            // below, so this may have been wrong to start with).
            encoded = Regex.Replace(encoded, "[=]*$", "");

            // Canonicalize to all upper case
            encoded = encoded.ToUpper();
            if (encoded.Length == 0)
            {
                return new byte[0];
            }
            var encodedLength = encoded.Length;
            var outLength = encodedLength * SHIFT / 8;
            var result = new byte[outLength];
            var buffer = 0;
            var next = 0;
            var bitsLeft = 0;
            foreach (var c in encoded.ToCharArray())
            {
                if (!CHAR_MAP.ContainsKey(c))
                {
                    throw new DecodingException("Illegal character: " + c);
                }
                buffer <<= SHIFT;
                buffer |= CHAR_MAP[c] & MASK;
                bitsLeft += SHIFT;
                if (bitsLeft >= 8)
                {
                    result[next++] = (byte)(buffer >> bitsLeft - 8);
                    bitsLeft -= 8;
                }
            }
            // We'll ignore leftover bits for now.
            //
            // if (next != outLength || bitsLeft >= SHIFT) {
            //  throw new DecodingException("Bits left: " + bitsLeft);
            // }
            return result;
        }


        public static string Encode(byte[] data, bool padOutput = false)
        {
            if (data.Length == 0)
            {
                return "";
            }

            // SHIFT is the number of bits per output character, so the length of the
            // output is the length of the input multiplied by 8/SHIFT, rounded up.
            if (data.Length >= 1 << 28)
            {
                // The computation below will fail, so don't do it.
                throw new ArgumentOutOfRangeException("data");
            }

            var outputLength = (data.Length * 8 + SHIFT - 1) / SHIFT;
            var result = new StringBuilder(outputLength);

            int buffer = data[0];
            var next = 1;
            var bitsLeft = 8;
            while (bitsLeft > 0 || next < data.Length)
            {
                if (bitsLeft < SHIFT)
                {
                    if (next < data.Length)
                    {
                        buffer <<= 8;
                        buffer |= data[next++] & 0xff;
                        bitsLeft += 8;
                    }
                    else
                    {
                        var pad = SHIFT - bitsLeft;
                        buffer <<= pad;
                        bitsLeft += pad;
                    }
                }
                var index = MASK & buffer >> bitsLeft - SHIFT;
                bitsLeft -= SHIFT;
                result.Append(DIGITS[index]);
            }
            if (padOutput)
            {
                var padding = 8 - result.Length % 8;
                if (padding > 0) result.Append(new string('=', padding == 8 ? 0 : padding));
            }
            return result.ToString();
        }

        private class DecodingException : Exception
        {
            public DecodingException(string message) : base(message)
            {
            }
        }
    }
}
