using System;
using System.Drawing;

namespace LibPuzzle
{
    internal static partial class PuzzleNative
    {
        public static double puzzle_median(double[] vec, int size)
        {
            if (size == 0)
                return 0.0;

            Array.Sort<double>(vec);

            int n;
            int o;
            if ((n = size / 2) == 0)
            {
                if (size > 1)
                    o = 1;
                else
                    o = 0;
            }
            else
                o = n + 1;

            if (o < n)
                throw new Exception();

            double avg;
            avg = (vec[n] + vec[o]) / 2.0;
            if (avg < vec[n] || avg > vec[o])
                avg = vec[n];

            return avg;
        }

        public static sbyte[] puzzle_fill_cvec_from_dvec(PuzzleContext context, PuzzleDvec dvec)
        {
            if (dvec.sizeof_compressed_vec == 0)
                throw new Exception();

            int s;

            int sizeof_vec = dvec.sizeof_compressed_vec;
            sbyte[] cvec = new sbyte[sizeof_vec];

            int sizeof_lights = sizeof_vec;
            int sizeof_darks  = sizeof_vec;
            
            double[] lights = new double[sizeof_lights];
            double[] darks  = new double[sizeof_darks];

            int dvecptr = 0;
            int pos_lights = 0, pos_darks = 0;

            double dv;
            for (s = 0; s < sizeof_vec; ++s)
            {
                dv = dvec.vec[dvecptr++];
                if (dv >= -context.puzzle_noise_cutoff &&
                    dv <=  context.puzzle_noise_cutoff)
                    continue;

                if (dv < context.puzzle_noise_cutoff)
                {
                    darks[pos_darks++] = dv;
                    if (pos_darks > sizeof_darks)
                        throw new Exception();
                }
                else if (dv > context.puzzle_noise_cutoff)
                {
                    lights[pos_lights++] = dv;
                    if (pos_lights > sizeof_lights)
                        throw new Exception();
                }
            }
            double lighter_cutoff = puzzle_median(lights, pos_lights);
            double darker_cutoff  = puzzle_median(darks,  pos_darks);

            dvecptr = 0;
            int cvecptr = 0;
            for (s = 0; s < sizeof_vec; ++s)
            {
                dv = dvec.vec[dvecptr++];
                if (dv >= -context.puzzle_noise_cutoff &&
                    dv <=  context.puzzle_noise_cutoff)
                    cvec[cvecptr++] = 0;
                else if (dv < 0.0)
                    cvec[cvecptr++] = (sbyte)(dv < darker_cutoff  ? -2 : -1);
                else
                    cvec[cvecptr++] = (sbyte)(dv > lighter_cutoff ?  2 :  1);
            };

            if (cvecptr != sizeof_vec)
                throw new Exception();
    
            return cvec;
        }

        public static sbyte[] puzzle_fill_cvec_from_file(PuzzleContext context, Bitmap bitmap)
        {
            return puzzle_fill_cvec_from_dvec(context, puzzle_fill_dvec_from_file(context, bitmap));
        }

    }
}
