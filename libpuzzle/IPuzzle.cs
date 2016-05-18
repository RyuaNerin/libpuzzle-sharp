using System;
using System.Collections;
using System.Collections.Generic;

namespace LibPuzzle
{
    public enum PuzzleThreshold
    {
        High,
        Normal,
        Low,
        Lower
    }

    [Serializable]
    public abstract class IPuzzle : ICloneable, IEquatable<IPuzzle>, IEnumerable
    {
        public const double DefaultSimilarityThreshold = 0.6;
        public const double HighSimilarityThreshold = 0.7;
        public const double LowSimilarityThreshold = 0.3;
        public const double LowerSimilarityThreshold = 0.2;

        public abstract object Clone();

        public abstract bool Equals(IPuzzle other);

        public abstract IEnumerator GetEnumerator();

        internal abstract sbyte[] GetCvec();
        internal abstract byte[] GetCompressedCvec();

        public double GetDistanceFrom(IPuzzle other)
        {
            return GetDistanceFrom(other, true);
        }
        public double GetDistanceFrom(IPuzzle other, bool fixForTexts)
        {
            if (other == null) throw new ArgumentNullException("other");
            return PuzzleNative.puzzle_vector_normalized_distance(this.GetCvec(), other.GetCvec(), fixForTexts);
        }

        public bool CheckSmilarity(PuzzleThreshold threshold, IPuzzle other)
        {
            return CheckSmilarity(threshold, other, true);
        }
        public bool CheckSmilarity(PuzzleThreshold threshold, IPuzzle other, bool fixForTexts)
        {
            if (other == null) throw new ArgumentNullException("other");
            var s = this.GetDistanceFrom(other, fixForTexts);

            switch (threshold)
            {
                case PuzzleThreshold.High:   return s < IPuzzle.HighSimilarityThreshold;
                case PuzzleThreshold.Normal: return s < IPuzzle.DefaultSimilarityThreshold;
                case PuzzleThreshold.Low:    return s < IPuzzle.LowSimilarityThreshold;
                case PuzzleThreshold.Lower:  return s < IPuzzle.LowerSimilarityThreshold;
            }

            return false;
        }

        protected static bool EqualsArray(sbyte[] a1, sbyte[] a2)
        {
            if (a1 == null || a2 == null) return false;
            if (a1.Length != a2.Length) return false;

            for (int i = 0; i < a1.Length; ++i)
                if (a1[i] != a2[i])
                    return false;

            return true;
        }
        protected static bool EqualsArray(byte[] a1, byte[] a2)
        {
            if (a1 == null || a2 == null) return false;
            if (a1.Length != a2.Length) return false;

            for (int i = 0; i < a1.Length; ++i)
                if (a1[i] != a2[i])
                    return false;

            return true;
        }
    }
}
