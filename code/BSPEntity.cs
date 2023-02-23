using Sandbox.BSP.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sandbox
{
	public partial class BSPEntity : RenderEntity
	{
		public Material Material { get; set; }

		private readonly List<BSPFACE> faceList;
		private readonly List<BSPSURFEDGE> surfEdgeList;
		private readonly List<BSPEDGE> edgeList;
		private readonly List<VECTOR3D> vertexList;
		private List<VertexBuffer> vertexBufferList;

		public BSPEntity( List<BSPFACE> faceList, List<BSPSURFEDGE> surfEdgeList, List<BSPEDGE> edgeList, List<VECTOR3D> vertexList )
		{
			this.faceList = faceList;
			this.surfEdgeList = surfEdgeList;
			this.edgeList = edgeList;
			this.vertexList = vertexList;
			this.vertexBufferList = new();

			RenderBounds = new BBox( Position, 5000f );
			Material = Material.Load( "materials/dev/debug_physics.vmat" );

			GenerateVertexBuffers();
		}

		public void GenerateVertexBuffers()
		{
			foreach ( BSPFACE face in faceList )
			{
				VertexBuffer vertexBuffer = new();
				vertexBuffer.Init( true );

				for ( int i = 2; i < face.nEdges; i++ )
				{
					vertexBuffer.AddRawIndex( i );
					vertexBuffer.AddRawIndex( i - 1 );
					vertexBuffer.AddRawIndex( 0 );
				}

				for ( int i = 0; i < face.nEdges; i++ )
				{
					int edgeIndex = Convert.ToInt32(face.iFirstEdge) + i;
					int vertexIndex = surfEdgeList[edgeIndex].GetVertexIndex( edgeList );
					Vector3 vertexPos = vertexList[vertexIndex].GetVector3();

					vertexBuffer.Add( new( vertexPos, Vector3.Zero, Vector3.Left, Vector4.One ) );
				}

				vertexBufferList.Add( vertexBuffer );
			}
		}

		public override void DoRender( SceneObject obj )
		{
			DebugOverlay.Text( "ORIGIN", Position );

			foreach ( VertexBuffer vertexBuffer in vertexBufferList )
			{
				vertexBuffer.Draw( Material );
			}
		}
	}
}
