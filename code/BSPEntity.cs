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

		public List<BSPFACE> faceList;
		public List<BSPSURFEDGE> surfEdgeList;
		public List<BSPEDGE> edgeList;
		public List<VECTOR3D> vertexList;
		public List<FaceMesh> faceMeshList;
		public List<VertexBuffer> vertexBufferList = new();

		public BSPEntity() { }

		public BSPEntity( List<BSPFACE> faceList, List<BSPSURFEDGE> surfEdgeList, List<BSPEDGE> edgeList, List<VECTOR3D> vertexList )
		{
			this.faceList = faceList;
			this.surfEdgeList = surfEdgeList;
			this.edgeList = edgeList;
			this.vertexList = vertexList;

			this.faceMeshList = new();

			Material = Material.Load( "materials/dev/debug_physics.vmat" );
			// Model = GetModel();
			// Model = GenerateModel();
			// EnableDrawing = true;
			GenerateModel();
		}

		/* public Model GetModel()
		{
			var modelBuilder = new ModelBuilder();

			modelBuilder.AddMesh( GetMesh() );

			return modelBuilder.Create();
		} */

		public struct FaceMesh
		{
			public List<Vertex> vertices;
			public List<int> indices;
		}

		public Model GenerateModel()
		{
			// VertexBuffer vertexBuffer = new VertexBuffer();
			// vertexBuffer.Init( true );

			int baseVertex = 0;

			foreach ( BSPFACE face in faceList )
			{
				List<int> renderIndicesList = new();
				List<Vertex> renderVertexList = new();
				VertexBuffer vertexBuffer = new();
				vertexBuffer.Init( true );

				int dst_temp = 0;
				for ( int i = 0; i < face.nEdges - 2; i++ )
				{
					renderIndicesList.Insert( dst_temp++, 0 );
					renderIndicesList.Insert( dst_temp++, i + 1 );
					renderIndicesList.Insert( dst_temp++, i + 2 );
				}

				// Log.Info( $"Expected index count: {(face.nEdges - 2) * 3}" );
				// Log.Info( $"Got: {renderIndicesList.Count}" );

				renderIndicesList.Reverse();

				foreach ( int index in renderIndicesList )
				{
					vertexBuffer.AddRawIndex( index );
				}

				for ( int i = 0; i < face.nEdges; i++ )
				{
					int vertexIndex = surfEdgeList[Convert.ToInt32( face.iFirstEdge ) + i].GetVertexIndex( edgeList );
					Vector3 vertexPos = vertexList[vertexIndex].GetVector3();
					// vertexPos += Position;
					Vertex vert = new Vertex( vertexPos, Vector3.Zero, Vector3.Left, Vector4.One );

					renderVertexList.Add( vert );
					vertexBuffer.Add( vert );
				}

				FaceMesh faceMesh = new()
				{
					vertices = renderVertexList,
					indices = renderIndicesList
				};

				vertexBufferList.Add( vertexBuffer );


				faceMeshList.Add( faceMesh );

				baseVertex += face.nEdges;
			}

			// renderIndicesList.Reverse();

			var modelBuilder = Model.Builder;

			foreach ( FaceMesh faceMesh in faceMeshList )
			{
				// Log.Info( "Drawing mesh: " );
				// Log.Info( $"Vertex count: {faceMesh.vertices.Count}" );
				// Log.Info( $"Index count: {faceMesh.indices.Count}" );
				Mesh mesh = new();
				mesh.CreateVertexBuffer( faceMesh.vertices.Count, Vertex.Layout, faceMesh.vertices );
				mesh.CreateIndexBuffer( faceMesh.indices.Count, faceMesh.indices.ToArray() );

				modelBuilder.AddMesh( mesh );
				// break;

				// if (counter++ > 10)
				// {
				// 	break;
				// }
			}

			Model createdModel = modelBuilder.Create();

			Log.Info( createdModel.MeshCount );
			Log.Info( createdModel.IsError );
			Log.Info( createdModel.IsProcedural );

			return createdModel;

			/*int count = 0;
			for ( int i = 0; i < renderVertexList.Count; i++ )
			{
				Vertex vert = renderVertexList[i];
				int index = renderIndicesList[i];

				vertexBuffer.Add( vert );
				vertexBuffer.AddRawIndex( index );

				if (count++ > 600)
				{
					count = 0;
					vertexBuffers.Add( vertexBuffer );

					vertexBuffer = new VertexBuffer();
					vertexBuffer.Init( true );
				}
			}*/

			/* int counter = 0;
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
			} */
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
			// vertexBuffer.Draw( Material.Load( "materials/shiny_white.vmat" ) );

			// for ( var i = 0; i < vertexBufferCount; i++ )
			// {
			// 	var vertices = vertexBuffer[i];
			// 	Graphics.Attributes.Set( "TextureDiffuse", vertices.Item2 );
			// 	vertices.Item1.Draw( renderMat );
			// }
			// DebugOverlay.Box( Mins, Maxs, Color.Red );

			foreach ( VertexBuffer vertexBuffer in vertexBufferList )
			{
				vertexBuffer.Draw( Material );
			}
		}
	}
}
