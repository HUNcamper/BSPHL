using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.BSP.data
{
	internal class BSPFACE
	{
		public static int ByteSize = 2 + 2 + 4 + 2 + 2 + (1 * 4) + 4;

		public ushort iPlane;               // Plane the face is parallel to
		public ushort nPlaneSide;           // Set if different normals orientation
		public uint iFirstEdge;             // Index of the first surfedge
		public ushort nEdges;               // Number of consecutive surfedges
		public ushort iTextureInfo;         // Index of the texture info structure
		public byte[] nStyles;              // Specify lighting styles, max 4
		public uint nLightMapOffset;        // Offsets into the raw lightmap data

		public BSPFACE( ushort iPlane, ushort nPlaneSide, uint iFirstEdge, ushort nEdges, ushort iTextureInfo, byte[] nStyles, uint nLightMapOffset )
		{
			this.iPlane = iPlane;
			this.nPlaneSide = nPlaneSide;
			this.iFirstEdge = iFirstEdge;
			this.nEdges = nEdges;
			this.iTextureInfo = iTextureInfo;
			this.nStyles = nStyles;
			this.nLightMapOffset = nLightMapOffset;
		}

		public static BSPFACE FromBytes( byte[] bytes, int offset )
		{
			ushort iPlane = BitConverter.ToUInt16( bytes, offset );
			ushort nPlaneSide = BitConverter.ToUInt16( bytes, offset + 2 );
			uint iFirstEdge = BitConverter.ToUInt32( bytes, offset + 4 );
			ushort nEdges = BitConverter.ToUInt16( bytes, offset + 8 );
			ushort iTextureInfo = BitConverter.ToUInt16( bytes, offset + 10 );
			byte[] nStyles = new byte[4];

			for ( int i = 0; i < nStyles.Length; i++ )
			{
				nStyles[i] = bytes[offset + 12 + i];
			}

			uint nLightMapOffset = BitConverter.ToUInt32( bytes, offset + 16 );

			return new BSPFACE( iPlane, nPlaneSide, iFirstEdge, nEdges, iTextureInfo, nStyles, nLightMapOffset );
		}

		public int GetByteSize()
		{
			return ByteSize;
		}
	}
}
