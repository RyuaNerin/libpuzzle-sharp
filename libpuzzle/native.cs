using System;

namespace LibPuzzle
{
    internal static partial class PuzzleNative
    {
        public const int PUZZLE_DEFAULT_LAMBDAS = 9;
        public const int PUZZLE_DEFAULT_MAX_WIDTH = 3000;
        public const int PUZZLE_DEFAULT_MAX_HEIGHT = 3000;
        public const double PUZZLE_DEFAULT_NOISE_CUTOFF = 2.0;
        public const double PUZZLE_DEFAULT_P_RATIO = 2.0;
        public const int PUZZLE_MIN_P = 2;
        public const int PUZZLE_PIXEL_FUZZ_SIZE = 1;
        public const int PUZZLE_NEIGHBORS = 8;
        public const int PUZZLE_MIN_SIZE_FOR_CROPPING = 100;
        public const double PUZZLE_DEFAULT_CONTRAST_BARRIER_FOR_CROPPING = 0.05;
        public const double PUZZLE_DEFAULT_MAX_CROPPING_RATIO = 0.25;
        public const bool PUZZLE_DEFAULT_ENABLE_AUTOCROP = true;

        public struct PuzzleView
        {
            public int width;
            public int height;
            public byte[] map;
        }

        public struct PuzzleAvgLvls
        {
            public int lambdas;
            public double[] lvls;
        }

        public struct PuzzleDvec
        {
            public int sizeof_compressed_vec;
            public double[] vec;
        }
    }
}
