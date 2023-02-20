using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.BSP.data
{
	internal class BSPSURFEDGE
	{
		public static int ByteSize = 4;

		public int value;

		public BSPSURFEDGE(int value)
		{
			this.value = value;
		}

		public static BSPSURFEDGE FromBytes( byte[] bytes, int offset )
		{
			int value = BitConverter.ToInt32( bytes, offset );

			return new BSPSURFEDGE( value );
		}
		public int GetByteSize()
		{
			return ByteSize;
		}
	}
}
