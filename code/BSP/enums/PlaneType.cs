using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.BSP.enums
{
    public enum PlaneType
    {
		PLANE_X,            // Plane is perpendicular to given axis
		PLANE_Y,
		PLANE_Z,
		PLANE_ANYX,         // Non-axial plane is snapped to the nearest
		PLANE_ANYY,
		PLANE_ANYZ
    }
}
