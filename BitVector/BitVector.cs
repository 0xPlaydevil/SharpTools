using System.Collections;
using System.Collections.Generic;
using System;

namespace PlayDev
{
	public struct BitVector
	{
		int[] bits;

		public int Length { get { return length; } }
		int length;
		public int UnitCount { get { return length==0? 0: (length-1) / 32 + 1; } }
		public bool this[int i]
		{
			get
			{
				return (bits[i/32]& 1<<31-i%32) !=0;
			}
			set
			{
				if(value)
				{
					bits[i / 32] |= 1 << 31 - i % 32;
				}
				else
				{
					bits[i / 32] &= ~(1 << 31 - i % 32);
				}
			}
		}

		public BitVector(int length)
		{
			UnityEngine.Debug.Log($"-1/32:{-1/32}");
			bits = new int[length == 0 ? 0 : (length - 1) / 32 + 1];
			this.length = length;
		}
		public BitVector(BitVector bits)
		{
			this.bits = bits.bits.Clone() as int[];
			length = bits.Length;
		}
		public BitVector(byte[] bits)
		{
			this.bits = new int[bits.Length / 4];
			Buffer.BlockCopy(bits, 0, this.bits, 0, bits.Length);
			length = bits.Length * 8;
		}
		public BitVector(int[] bits)
		{
			this.bits = bits.Clone() as int[];
			length = bits.Length * 32;
		}
		
		public static BitVector operator&(BitVector a, BitVector b)
		{
			return ForEachUnit(a, b, (m, n) => m & n);
		}
		public static BitVector operator |(BitVector a, BitVector b)
		{
			return ForEachUnit(a, b, (m, n) => m | n);
		}
		public static BitVector operator~(BitVector a)
		{
			return ForEachUnit(a, m => ~m);
		}
		public static BitVector operator ^(BitVector a, BitVector b)
		{
			return ForEachUnit(a, b, (m, n) => m ^ n);
		}


		static BitVector ForEachUnit(BitVector a,BitVector b,Func<int,int,int> op)
		{
			int[] r = new int[a.UnitCount > b.UnitCount ? b.UnitCount : a.UnitCount];
			for(int i=0;i<r.Length;++i)
			{
				r[i] = op(a.bits[i], b.bits[i]);
			}
			return new BitVector(r);
		}
		static BitVector ForEachUnit(BitVector a,Func<int,int> op)
		{
			int[] r = new int[a.UnitCount];
			for(int i=0;i<r.Length;++i)
			{
				r[i] = op(a.bits[i]);
			}
			return new BitVector(r);
		}

	}	
}
