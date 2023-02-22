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
			vertexBuffer = new VertexBuffer();
			vertexBuffer.Init( true );

			int baseVertex = 0;

			List<int> renderIndicesList = new();

			foreach ( BSPFACE face in faceList )
			{
				for ( int i = 0; i < face.nEdges - 2; i++ )
				{
					renderIndicesList.Add( baseVertex + 0 );
					renderIndicesList.Add( baseVertex + i + 1 );
					renderIndicesList.Add( baseVertex + i + 2 );
				}

				baseVertex += face.nEdges;
			}

			// renderIndicesList.Reverse();

			List<Vertex> renderVertexList = new();

			foreach ( BSPFACE face in faceList )
			{
				for ( int i = 0; i < face.nEdges; i++ )
				{
					int vertexIndex = surfEdgeList[Convert.ToInt32( face.iFirstEdge ) + i].GetVertexIndex( edgeList );
					Vector3 vertexPos = vertexList[vertexIndex].GetVector3();
					vertexPos += Position;
					Vertex vert = new Vertex( vertexPos, Vector3.Zero, Vector3.Left, new Vector4() );

					renderVertexList.Add( vert );
				}
			}

			int counter = 0;
			foreach ( Vertex vert in renderVertexList )
			{
				vertexBuffer.Add( new( vert.Position, vert.Normal, vert.Tangent, new Vector4() ) );
				if (counter++ > 700)
				{
					break;
				}
			}

			counter = 0;
			foreach ( int index in renderIndicesList )
			{
				vertexBuffer.AddRawIndex( index );
				if ( counter++ > 700 )
				{
					break;
				}
			}
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

			// Log.Info( vertexBuffer );
			// Log.Info( Material );
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
