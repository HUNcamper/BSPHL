using Sandbox.BSP.data;
using Sandbox.BSP.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.BSP
{
	public class BSPParser
	{
		List<VECTOR3D> vertexList = new();
		List<BSPEDGE> edgeList = new();
		List<BSPSURFEDGE> surfEdgeList = new();
		List<BSPPLANE> planeList = new();
		List<BSPFACE> faceList = new();

		public static BSPMapEntity CreateMapEntity(string filename)
		{
			BSPParser parser = new BSPParser();
			parser.LoadFile(filename);
			
			return parser.CreateMapEntity();
		}

		public BSPMapEntity CreateMapEntity()
		{
			BSPMapEntity mapEntity = new()
			{
				FaceList = faceList,
				SurfEdgeList = surfEdgeList,
				EdgeList = edgeList,
				VertexList = vertexList,
			};

			mapEntity.Load();

			return mapEntity;
		}

		public void Clear()
		{
			vertexList = new();
			edgeList = new();
			surfEdgeList = new();
			planeList = new();
			faceList = new();
		}

		public void LoadFile(string filename)
		{
			Clear();

			byte[] bytes = FileSystem.Data.ReadAllBytes( filename ).ToArray();

			BSPHEADER BSPHeader = BSPHEADER.FromBytes( bytes, 0 );

			long vertices_offset = BSPHeader.GetLump( LumpType.LUMP_VERTICES ).nOffset;
			long vertices_length = BSPHeader.GetLump( LumpType.LUMP_VERTICES ).nLength / VECTOR3D.ByteSize;

			for ( int i = 0; i < vertices_length; i++ )
			{
				VECTOR3D vector3d = VECTOR3D.FromBytes( bytes, Convert.ToInt32( vertices_offset ) + (i * VECTOR3D.ByteSize) );
				vertexList.Add( vector3d );
			}

			long edges_offset = BSPHeader.GetLump( LumpType.LUMP_EDGES ).nOffset;
			long edges_length = BSPHeader.GetLump( LumpType.LUMP_EDGES ).nLength / BSPEDGE.ByteSize;

			for ( int i = 0; i < edges_length; i++ )
			{
				BSPEDGE bspEdge = BSPEDGE.FromBytes( bytes, Convert.ToInt32( edges_offset ) + (i * BSPEDGE.ByteSize) );
				edgeList.Add( bspEdge );
			}

			long surfedges_offset = BSPHeader.GetLump( LumpType.LUMP_SURFEDGES ).nOffset;
			long surfedges_length = BSPHeader.GetLump( LumpType.LUMP_SURFEDGES ).nLength / BSPSURFEDGE.ByteSize;

			for ( int i = 0; i < surfedges_length; i++ )
			{
				BSPSURFEDGE bspSurfEdge = BSPSURFEDGE.FromBytes( bytes, Convert.ToInt32( surfedges_offset ) + (i * BSPSURFEDGE.ByteSize) );
				surfEdgeList.Add( bspSurfEdge );
			}

			long planes_offset = BSPHeader.GetLump( LumpType.LUMP_PLANES ).nOffset;
			long planes_length = BSPHeader.GetLump( LumpType.LUMP_PLANES ).nLength / BSPPLANE.ByteSize;

			for ( int i = 0; i < planes_length; i++ )
			{
				BSPPLANE bspPlane = BSPPLANE.FromBytes( bytes, Convert.ToInt32( planes_offset ) + (i * BSPPLANE.ByteSize) );
				planeList.Add( bspPlane );
			}

			long faces_offset = BSPHeader.GetLump( LumpType.LUMP_FACES ).nOffset;
			long faces_length = BSPHeader.GetLump( LumpType.LUMP_FACES ).nLength / BSPFACE.ByteSize;

			for ( int i = 0; i < faces_length; i++ )
			{
				BSPFACE bspFace = BSPFACE.FromBytes( bytes, Convert.ToInt32( faces_offset ) + (i * BSPFACE.ByteSize) );
				faceList.Add( bspFace );
			}
		}
	}
}
