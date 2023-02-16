using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.BSP.data
{
	public class BSPEDGE : BSPData
	{
		public static int ByteSize = (2 * 2);

		public ushort[] iVertex; // Indices into vertex array, max 2

		public BSPEDGE( ushort index1, ushort index2 )
		{
			this.iVertex = new ushort[2];
			this.iVertex[0] = index1;
			this.iVertex[1] = index2;
		}

		public static BSPEDGE FromBytes( byte[] bytes, int offset )
		{
			ushort index1 = BitConverter.ToUInt16( bytes, offset );
			ushort index2 = BitConverter.ToUInt16( bytes, offset + 2 );

			return new BSPEDGE( index1, index2 );
		}

		public int GetByteSize()
		{
			return ByteSize;
		}
	}
}
