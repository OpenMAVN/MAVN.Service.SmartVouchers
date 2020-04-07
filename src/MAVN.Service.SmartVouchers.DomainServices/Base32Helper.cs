using System;
using System.Collections.Generic;
using System.Text;

namespace MAVN.Service.SmartVouchers.DomainServices
{
    internal static class Base32Helper
    {
        private const int SHIFT = 5;
        private const int MASK = 31;

        private static readonly char[] DIGITS = "ABCDEFGHIJKLMNPQRSTUVWXYZ2345678".ToCharArray();

        private static Dictionary<char, int> CHAR_MAP = new Dictionary<char, int>();

        static Base32Helper()
        {
            for (var i = 0; i < DIGITS.Length; i++)
                CHAR_MAP[DIGITS[i]] = i;
        }

        public static string Encode(byte[] data, bool padOutput = false)
        {
            if (data.Length == 0)
                return "";

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
    }
}
