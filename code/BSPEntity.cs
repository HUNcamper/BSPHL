using Sandbox.BSP.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox
{
	public partial class BSPEntity : RenderEntity
	{
		[Net] public Material Material { get; set; }

		public List<BSPFACE> faceList;
		public List<BSPSURFEDGE> surfEdgeList;
		public List<BSPEDGE> edgeList;
		public List<VECTOR3D> vertexList;
		public VertexBuffer vertexBuffer;

		public BSPEntity() { }

		public BSPEntity( List<BSPFACE> faceList, List<BSPSURFEDGE> surfEdgeList, List<BSPEDGE> edgeList, List<VECTOR3D> vertexList )
		{
			this.faceList = faceList;
			this.surfEdgeList = surfEdgeList;
			this.edgeList = edgeList;
			this.vertexList = vertexList;

			Material = Material.Load( "materials/shiny_white.vmat" );
			GetMesh();
			// Model = GetModel();
		}

		/* public Model GetModel()
		{
			var modelBuilder = new ModelBuilder();

			modelBuilder.AddMesh( GetMesh() );

			return modelBuilder.Create();
		} */

		public void GetMesh()
		{
			// var vertices = new BSPVertex[vertexList.Count];
			// var indices = new int[vertexList.Count];

			// Mesh mesh = new Mesh( Material );
			vertexBuffer = new VertexBuffer();
			vertexBuffer.Init( true );
			int counter = 0;
			foreach ( BSPFACE face in faceList )
			{
				// Log.Info( "Adding vertex" );
				for ( int i = 0; i < face.nEdges; i++ )
				{
					int vertexIndex = surfEdgeList[Convert.ToInt32( face.iFirstEdge ) + i].GetVertexIndex( edgeList );
					Vector3 vertexPos = vertexList[vertexIndex].GetVector3();
					vertexPos += Position;
					Vertex vert = new Vertex( vertexPos, new Vector4(), Color.Red );
					// Vertex vert = new Vertex( new Vector3(i*1000, i*1000, i*1000), new Vector4(), Color.Red );
					vertexBuffer.AddRawIndex( i );
					// Log.Info( vert.Position );

					vertexBuffer.Add( vert );
				}

				if (counter++ > 100)
				{
					break;
				}

				//int vertexIndex1 = surfEdgeList[Convert.ToInt32( face.iFirstEdge )].GetVertexIndex( edgeList );
				//Vector3 vertexPos1 = vertexList[vertexIndex1].GetVector3();
				//Vertex vert1 = new Vertex( vertexPos1, new Vector4( 0, 0, 0, 0 ), Color32.White );

				//int vertexIndex2 = surfEdgeList[Convert.ToInt32( face.iFirstEdge ) + 1].GetVertexIndex( edgeList );
				//Vector3 vertexPos2 = vertexList[vertexIndex2].GetVector3();
				//Vertex vert2 = new Vertex( vertexPos2, new Vector4( 0, 0, 0, 0 ), Color32.White );

				//int vertexIndex3 = surfEdgeList[Convert.ToInt32( face.iFirstEdge ) + 2].GetVertexIndex( edgeList );
				//Vector3 vertexPos3 = vertexList[vertexIndex3].GetVector3();
				//Vertex vert3 = new Vertex( vertexPos3, new Vector4( 0, 0, 0, 0 ), Color32.White );

				//int vertexIndex4 = surfEdgeList[Convert.ToInt32( face.iFirstEdge ) + 3].GetVertexIndex( edgeList );
				//Vector3 vertexPos4 = vertexList[vertexIndex4].GetVector3();
				//Vertex vert4 = new Vertex( vertexPos4, new Vector4( 0, 0, 0, 0 ), Color32.White );

				// vertexBuffer.AddQuad( vert1, vert2, vert3, vert4 );

				// vertexBuffer.AddQuad( vert1, vert2, vert3, vert4 );
				// BSPVertex bSPVertex = new BSPVertex();
				// mesh.CreateVertexBuffer<BSPVertex>( vertexBuffer );
			}
			// mesh.CreateBuffers( vertexBuffer );
			// return mesh;
			Log.Info( vertexBuffer );
		}

		public override void DoRender( SceneObject obj )
		{
			Render();
		}

		public void Render()
		{
			DebugOverlay.Text( $"WORLD", Position, 0f );

			// Graphics.Attributes.Set( "TextureLightmap", lightmap );
			// Graphics.Attributes.Set( "Opacity", 1.0 );

			Log.Info( vertexBuffer );
			Log.Info( Material );
			vertexBuffer.Draw( Material.Load( "materials/shiny_white.vmat" ) );

			// for ( var i = 0; i < vertexBufferCount; i++ )
			// {
			// 	var vertices = vertexBuffer[i];
			// 	Graphics.Attributes.Set( "TextureDiffuse", vertices.Item2 );
			// 	vertices.Item1.Draw( renderMat );
			// }
			// DebugOverlay.Box( Mins, Maxs, Color.Red );
		}
	}
}
