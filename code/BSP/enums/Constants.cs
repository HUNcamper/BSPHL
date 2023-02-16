using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.BSP.enums
{
    public enum Constants
    {
		MAX_MAP_HULLS			= 4,
		MAX_MAP_MODELS			= 400,
		MAX_MAP_ENTITIES		= 1024,
		MAX_MAP_ENTSTRING		= 128 * 1024,
		
		MAX_MAP_PLANES			= 32767,
		MAX_MAP_NODES			= 32767,
		MAX_MAP_CLIPNODES		= 32767,
		MAX_MAP_LEAFS			= 8192,
		MAX_MAP_VERTS			= 65535,
		MAX_MAP_FACES			= 65535,
		MAX_MAP_MARKSURFACES	= 65535,
		MAX_MAP_TEXINFO			= 8192,
		MAX_MAP_EDGES			= 256000,
		MAX_MAP_SURFEDGES		= 512000,
		MAX_MAP_TEXTURES		= 512,
		MAX_MAP_MIPTEX			= 0x200000,
		MAX_MAP_LIGHTING		= 0x200000,
		MAX_MAP_VISIBILITY		= 0x200000,

		MAX_MAP_PORTALS			= 65536
	}
}
