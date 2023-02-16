using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.BSP.data
{
	public class BSPLUMP : BSPData
	{
		public static int ByteSize = 4 + 4;

		public uint nOffset;
		public uint nLength;

		public BSPLUMP( uint nOffset, uint nLength )
		{
			this.nOffset = nOffset;
			this.nLength = nLength;
		}

		public static BSPLUMP FromBytes( byte[] bytes, int offset )
		{
			uint nOffset = BitConverter.ToUInt32( bytes, offset );
			uint nLength = BitConverter.ToUInt32( bytes, offset + 4 );

			return new BSPLUMP( nOffset, nLength );
		}

		public int GetByteSize()
		{
			return ByteSize;
		}
	}
}
