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

		[HideInEditor]
		public List<BSPFACE> FaceList { get; set; }

		[HideInEditor]
		public List<BSPSURFEDGE> SurfEdgeList { get; set; }

		[HideInEditor]
		public List<BSPEDGE> EdgeList { get; set; }

		[HideInEditor]
		public List<VECTOR3D> VertexList { get; set; }

		private List<VertexBuffer> vertexBufferList;

		public BSPEntity()
		{
			this.vertexBufferList = new();

			RenderBounds = new BBox( Position, 5000f );
			Material = Material.Load( "materials/dev/debug_physics.vmat" );
		}

		public void Load()
		{
			GenerateVertexBuffers();
		}

		private void GenerateVertexBuffers()
		{
			foreach ( BSPFACE face in FaceList )
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
					int vertexIndex = SurfEdgeList[edgeIndex].GetVertexIndex( EdgeList );
					Vector3 vertexPos = VertexList[vertexIndex].GetVector3();

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
