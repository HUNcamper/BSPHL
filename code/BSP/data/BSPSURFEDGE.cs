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

		// If the value of the surfedge is positive,
		// the first vertex of the edge is used as vertex for rendering the face,
		// otherwise, the value is multiplied by -1 and the second vertex of the indexed edge is used. 
		public int GetVertexIndex( BSPEDGE[] edgeList )
		{
			if (value > 0)
			{
				return edgeList[value].iVertex[0];
			}
			else
			{
				return edgeList[-value].iVertex[1];
			}
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
