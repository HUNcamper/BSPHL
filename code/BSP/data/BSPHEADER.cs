using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Sandbox.BSP.enums;

namespace Sandbox.BSP.data
{
	public class BSPHEADER : BSPData
	{
		public static int ByteSize = 4 + (16 * BSPLUMP.ByteSize);

		public uint nVersion;
		public BSPLUMP[] lump; // 16 max

		public BSPHEADER( uint nVersion, BSPLUMP[] lump )
		{
			this.nVersion = nVersion;
			this.lump = lump;
		}

		public BSPLUMP GetLump(LumpType lumpType)
		{
			return this.lump[(int) lumpType];
		}

		public static BSPHEADER FromBytes( byte[] bytes, int offset )
		{
			uint nVersion = BitConverter.ToUInt32( bytes, offset );

			BSPLUMP[] lump = new BSPLUMP[16];
			
			for ( int i = 0; i < lump.Length; i++ )
			{
				lump[i] = BSPLUMP.FromBytes( bytes, offset + 4 + ( i * BSPLUMP.ByteSize ) );
			}

			return new BSPHEADER( nVersion, lump );
		}

		public int GetByteSize()
		{
			return ByteSize;
		}
	}
}
