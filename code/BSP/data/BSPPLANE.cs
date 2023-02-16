using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.BSP.data
{
	internal class BSPPLANE : BSPData
	{
		public static int ByteSize = VECTOR3D.ByteSize + 4 + 4;

		public VECTOR3D vNormal;
		public float fDist;
		public int nType;

		public int GetByteSize()
		{
			return ByteSize;
		}
	}
}
