using System;

namespace LibPuzzle
{
    internal static partial class PuzzleNative
    {
        public static sbyte[] puzzle_vector_sub(sbyte[] cvec1, sbyte[] cvec2, bool fix_for_texts)
        {
            if (cvec1.Length != cvec2.Length || cvec1.Length <= 0)
                throw new Exception();
  
            sbyte[] cvecr = new sbyte[cvec1.Length];

            int remaining = cvec1.Length;
            if (fix_for_texts)
            {
                sbyte c1, c2, cr;
                do
                {
                    --remaining;
                    c1 = (sbyte)cvec1[remaining];
                    c2 = (sbyte)cvec2[remaining];
                    if ((c1 == 0 && c2 == -2) || (c1 == -2 && c2 == 0))
                        cr = -3;
                    else if ((c1 == 0 && c2 == +2) || (c1 == +2 && c2 == 0))
                        cr = +3;
                    else
                        cr = (sbyte)(c1 - c2);

                    cvecr[remaining] = cr;
                } while (remaining > 0);        
            }
            else
            {
                do
                {
                    --remaining;
                    cvecr[remaining] = (sbyte)(cvec1[remaining] - cvec2[remaining]);
                } while (remaining > 0);
            }
            
            return cvecr;
        }

        public static double puzzle_vector_euclidean_length(sbyte[] cvec)
        {
            int remaining;

            if ((remaining = cvec.Length) <= 0)
                return 0.0;

            ulong t = 0;
            ulong c;
            int c2;
            do
            {
                remaining--;
                c2 = cvec[remaining];
                c = (ulong)(c2 * c2);
                if (ulong.MaxValue - t < c)
                    throw new Exception();

                t += c;
            } while (remaining > 0);
    
            return Math.Sqrt(t);
        }

        public static double puzzle_vector_normalized_distance(sbyte[] cvec1, sbyte[] cvec2, bool fix_for_texts)
        {
            sbyte[] cvecr = puzzle_vector_sub(cvec1, cvec2, fix_for_texts);
            double dt, dr;

            dt = puzzle_vector_euclidean_length(cvecr);
            dr = puzzle_vector_euclidean_length(cvec1) + puzzle_vector_euclidean_length(cvec2);
            if (dr == 0.0)
                return 0.0;

            return dt / dr;
        }
    }
}
