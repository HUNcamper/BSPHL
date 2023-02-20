using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.BSP.data
{
	internal class BSPMARKSURFACE
	{
		public static int ByteSize = 4;

		public ushort value;

		public BSPMARKSURFACE( ushort value )
		{
			this.value = value;
		}

		public static BSPMARKSURFACE FromBytes( byte[] bytes, int offset )
		{
			ushort value = BitConverter.ToUInt16( bytes, offset );

			return new BSPMARKSURFACE( value );
		}

		public int GetByteSize()
		{
			return ByteSize;
		}
	}
}
