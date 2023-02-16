using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.BSP.data
{
	public class VECTOR3D : BSPData
	{
		public static int ByteSize = 4 + 4 + 4;

		public float x;
		public float y;
		public float z;
		
		public VECTOR3D(float x, float y, float z)
        {
			this.x = x;
			this.y = y;
			this.z = z;
        }

		public static VECTOR3D FromBytes(byte[] bytes, int offset)
        {
			float x = BitConverter.ToSingle(bytes, offset);
			float y = BitConverter.ToSingle(bytes, offset + 4);
			float z = BitConverter.ToSingle(bytes, offset + 8);

			return new VECTOR3D(x, y, z);
		}

		public Vector3 GetVector3()
		{
			return new Vector3(x, y, z);
		}

		public int GetByteSize()
        {
			return ByteSize;
        }
	}
}
