using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LibPuzzle
{
    [Serializable]
    public class CompressedPuzzle : IPuzzle, IEquatable<CompressedPuzzle>, IEnumerable<byte>
    {
        internal CompressedPuzzle(byte[] vec)
        {
            this.m_vec = (byte[])vec;
        }

        public Puzzle ToUncompressedPuzzle()
        {
            return new Puzzle(PuzzleNative.puzzle_uncompress_cvec(this.m_vec));
        }

        internal readonly byte[] m_vec;
        public byte[] GetBytes()
        {
            return (byte[])this.m_vec.Clone();
        }

        public override IEnumerator GetEnumerator()
        {
            return this.GetBytes().GetEnumerator();
        }
        IEnumerator<byte> IEnumerable<byte>.GetEnumerator()
        {
            return ((IEnumerable<byte>)this.GetBytes()).GetEnumerator();
        }

        internal override byte[] GetCompressedCvec()
        {
            return this.m_vec;
        }

        internal sbyte[] m_uncompressedVec;
        internal override sbyte[] GetCvec()
        {
            if (this.m_uncompressedVec == null)
                this.m_uncompressedVec = PuzzleNative.puzzle_uncompress_cvec(this.m_vec);

            return this.m_uncompressedVec;
        }

        public override object Clone()
        {
            return new CompressedPuzzle(this.m_vec);
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

            CompressedPuzzle ipuzzle = other as CompressedPuzzle;

            if (ipuzzle != null)
                return IPuzzle.EqualsArray(this.m_vec, ipuzzle.m_vec);
            else
                return IPuzzle.EqualsArray(this.GetCvec(), other.GetCvec());
        }
        public bool Equals(CompressedPuzzle other)
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
