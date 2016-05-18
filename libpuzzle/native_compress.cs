using System;

namespace LibPuzzle
{
    internal static partial class PuzzleNative
    {
        public static byte[] puzzle_compress_cvec(sbyte[] cvec)
        {
            int sizeof_compressed_vec = (cvec.Length + 2) / 3;
            if (sizeof_compressed_vec < 2)
                throw new Exception();

            byte[] compressed_cvec = new byte[sizeof_compressed_vec];

            int remaining = cvec.Length;
            int ptr = 0;
            int cptr = 0;
            while (remaining > 3)
            {
                compressed_cvec[cptr++] = (byte)((byte)(cvec[ptr + 0] + 2) + (byte)(cvec[ptr + 1] + 2) * 5 + (byte)(cvec[ptr + 2] + 2) * (5 * 5));
                ptr += 3;
                remaining -= 3;
            }

            if (remaining == 1)
            {
                compressed_cvec[cptr++] = (byte)(cvec[ptr + 0] + 1);
                compressed_cvec[0] |= 128;
            }
            else if (remaining == 2)
            {
                compressed_cvec[cptr] = (byte)((byte)(cvec[ptr + 0] + 2) + (byte)(cvec[ptr + 1] + 2) * 5);
                compressed_cvec[1] |= 128;
            }

            if (cptr != sizeof_compressed_vec)
                throw new Exception();

            return compressed_cvec;
        }

        public static sbyte[] puzzle_uncompress_cvec(byte[] compressed_cvec)
        {
            int remaining;

            if ((remaining = compressed_cvec.Length) < 2)
                throw new Exception();

            byte trailing_bits = (byte)(((compressed_cvec[0] & 128) >> 7) | ((compressed_cvec[1] & 128) >> 6));
            if (trailing_bits > 2)
                throw new Exception();

            int sizeof_vec = 3 * (compressed_cvec.Length - trailing_bits) + trailing_bits;
            sbyte[] cvec = new sbyte[sizeof_vec];
            if (trailing_bits != 0)
            {
                if (remaining <= 0)
                    throw new Exception();

                remaining--;
            }

            int ptr = 0;
            int cptr = 0;
            byte c;
            while (remaining > 0)
            {
                c = (byte)(compressed_cvec[cptr++] & 127);
                cvec[ptr++] = (sbyte)((c % 5) - 2);
                c /= 5;
                cvec[ptr++] = (sbyte)((c % 5) - 2);
                c /= 5;
                cvec[ptr++] = (sbyte)((c % 5) - 2);
                remaining--;
            }
            if (trailing_bits == 1)
            {
                cvec[ptr++] = (sbyte)((compressed_cvec[cptr] % 5) - 2);
            }
            else if (trailing_bits == 2)
            {
                c = (byte)(compressed_cvec[cptr++] & 127);
                cvec[ptr++] = (sbyte)((c % 5) - 2);
                cvec[ptr++] = (sbyte)((c / 5 % 5) - 2);
            }

            if (ptr != sizeof_vec)
                throw new Exception();

            return cvec;
        }
    }
}
