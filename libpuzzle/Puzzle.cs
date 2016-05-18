using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LibPuzzle
{
    [Serializable]
    public class Puzzle : IPuzzle, IEquatable<Puzzle>, IEnumerable<sbyte>
    {
        internal Puzzle(sbyte[] vec)
        {
            this.m_vec = (sbyte[])vec.Clone();
        }

        public CompressedPuzzle ToCompressedPuzzle()
        {
            return new CompressedPuzzle(PuzzleNative.puzzle_compress_cvec(this.m_vec));
        }

        internal readonly sbyte[] m_vec;
        public sbyte[] GetSBytes()
        {
            return (sbyte[])this.m_vec.Clone();
        }

        public override IEnumerator GetEnumerator()
        {
            return this.GetSBytes().GetEnumerator();
        }
        IEnumerator<sbyte> IEnumerable<sbyte>.GetEnumerator()
        {
            return ((IEnumerable<sbyte>)this.GetSBytes()).GetEnumerator();
        }

        internal override sbyte[] GetCvec()
        {
            return this.m_vec;
        }

        internal byte[] m_compressedVec;
        internal override byte[] GetCompressedCvec()
        {
            if (this.m_compressedVec == null)
                this.m_compressedVec = PuzzleNative.puzzle_compress_cvec(this.m_vec);

            return this.m_compressedVec;
        }

        public override object Clone()
        {
            return new Puzzle(this.m_vec);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            IPuzzle ipuzzle = obj as IPuzzle;
            if (ipuzzle == null) return false;

            return this.Equals(ipuzzle);
        }
        public override bool Equals(IPuzzle other)
        {
            if (other == null) return false;

            Puzzle ipuzzle = other as Puzzle;

            if (ipuzzle != null)
                return IPuzzle.EqualsArray(this.m_vec, ipuzzle.m_vec);
            else
                return IPuzzle.EqualsArray(this.GetCvec(), other.GetCvec());
        }
        public bool Equals(Puzzle other)
        {
            if (other == null) return false;

            return IPuzzle.EqualsArray(this.m_vec, other.m_vec);
        }

        private int? m_hash;
        public override int GetHashCode()
        {
            if (!this.m_hash.HasValue)
            {
                int sum = 5381;
                for (long i = 0; i < this.m_vec.LongLength; ++i)
                    sum = (sum + (sum << 5)) ^ (byte)this.m_vec[i];

                this.m_hash = sum;
            }

            return this.m_hash.Value;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(this.m_vec.Length * 2);

            for (long i = 0; i < this.m_vec.LongLength; ++i)
                sb.AppendFormat("{0:X2}", this.m_vec[i]);

            return sb.ToString();
        }
    }
}
