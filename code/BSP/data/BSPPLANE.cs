using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.BSP.enums;

namespace Sandbox.BSP.data
{
	internal class BSPPLANE : BSPData
	{
		public static int ByteSize = VECTOR3D.ByteSize + 4 + 4;

		public VECTOR3D vNormal;    // The planes normal vector
		public float fDist;			// Plane equation is: vNormal * X = fDist
		public PlaneType nType;     // Plane type

		public BSPPLANE(VECTOR3D vNormal, float fDist, PlaneType nType)
		{
			this.vNormal = vNormal;
			this.fDist = fDist;
			this.nType = nType;
		}

		public static BSPPLANE FromBytes( byte[] bytes, int offset )
		{
			VECTOR3D vNormal = VECTOR3D.FromBytes( bytes, offset );
			float fDist = BitConverter.ToSingle( bytes, offset + VECTOR3D.ByteSize );
			int nTypeInt = BitConverter.ToInt32( bytes, offset + VECTOR3D.ByteSize + 4 );
			PlaneType nType = (PlaneType)nTypeInt;

			return new BSPPLANE( vNormal, fDist, nType );
		}

		public int GetByteSize()
		{
			return ByteSize;
		}
	}
}
