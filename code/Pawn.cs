using Sandbox;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using Sandbox.BSP.data;
using System.Collections.Generic;
using Sandbox.BSP.enums;

namespace Sandbox;

partial class Pawn : AnimatedEntity
{
	/// <summary>
	/// Called when the entity is first created 
	/// </summary>
	public override void Spawn()
	{
		base.Spawn();

		//
		// Use a watermelon model
		//
		SetModel( "models/sbox_props/watermelon/watermelon.vmdl" );

		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;
	}

	// An example BuildInput method within a player's Pawn class.
	[ClientInput] public Vector3 InputDirection { get; protected set; }
	[ClientInput] public Angles ViewAngles { get; set; }

	List<VECTOR3D> vertexList = new List<VECTOR3D>();
	List<BSPEDGE> edgeList = new List<BSPEDGE>();
	List<BSPSURFEDGE> surfEdgeList = new List<BSPSURFEDGE>();
	List<BSPPLANE> planeList = new List<BSPPLANE>();
	List<BSPFACE> faceList = new List<BSPFACE>();

	public override void BuildInput()
	{
		InputDirection = Input.AnalogMove;

		var look = Input.AnalogLook;

		var viewAngles = ViewAngles;
		viewAngles += look;
		ViewAngles = viewAngles.Normal;
	}

	/// <summary>
	/// Called every tick, clientside and serverside.
	/// </summary>
	public override void Simulate( IClient cl )
	{
		base.Simulate( cl );

		Rotation = ViewAngles.ToRotation();

		// build movement from the input values
		var movement = InputDirection.Normal;

		// rotate it to the direction we're facing
		Velocity = Rotation * movement;

		// apply some speed to it
		Velocity *= Input.Down( InputButton.Run ) ? 1000 : 200;

		// apply it to our position using MoveHelper, which handles collision
		// detection and sliding across surfaces for us
		MoveHelper helper = new MoveHelper( Position, Velocity );
		helper.Trace = helper.Trace.Size( 16 );
		if ( helper.TryMove( Time.Delta ) > 0 )
		{
			Position = helper.Position;
		}

		// If we're running serverside and Attack1 was just pressed, spawn a ragdoll
		if ( Game.IsServer && Input.Pressed( InputButton.PrimaryAttack ) )
		{			
			byte[] bytes = FileSystem.Data.ReadAllBytes("c1a0.bsp").ToArray();

			BSPHEADER BSPHeader = BSPHEADER.FromBytes( bytes, 0 );

			vertexList = new List<VECTOR3D>();
			edgeList = new List<BSPEDGE>();
			surfEdgeList = new List<BSPSURFEDGE>();
			planeList = new List<BSPPLANE>();
			faceList = new List<BSPFACE>();

			// 113332 for snark_pit.bsp
			long vertices_offset = BSPHeader.GetLump( LumpType.LUMP_VERTICES ).nOffset;
			// 3812 for snark_pit.bsp
			long vertices_length = BSPHeader.GetLump( LumpType.LUMP_VERTICES ).nLength / VECTOR3D.ByteSize;

			for (int i = 0; i < vertices_length; i++)
			{
				VECTOR3D vector3d = VECTOR3D.FromBytes(bytes, Convert.ToInt32(vertices_offset) + (i * VECTOR3D.ByteSize));
				vertexList.Add(vector3d);
			}

			// 380464 for snark_pit.bsp
			long edges_offset = BSPHeader.GetLump( LumpType.LUMP_EDGES ).nOffset;
			// 3365 for snark_pit.bsp
			long edges_length = BSPHeader.GetLump( LumpType.LUMP_EDGES ).nLength / BSPEDGE.ByteSize;

			for ( int i = 0; i < edges_length; i++ )
			{
				BSPEDGE bspEdge = BSPEDGE.FromBytes(bytes, Convert.ToInt32( edges_offset) + (i * BSPEDGE.ByteSize));
				edgeList.Add(bspEdge);
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

			BSPEntity bspEntity = new BSPEntity(faceList, surfEdgeList, edgeList, vertexList);
			bspEntity.Position = Position;
			bspEntity.Spawn();
		}

		if ( Game.IsServer && Input.Down( InputButton.SecondaryAttack ) )
		{
			foreach ( VECTOR3D vertex in vertexList )
			{
				vertex.z += 1;
			}
		}

		foreach ( VECTOR3D vertex in vertexList )
		{
			// DebugOverlay.Sphere( vertex.GetVector3(), 1, Color.Red );
		}

		foreach ( BSPEDGE bspEdge in edgeList )
		{
			int index1 = bspEdge.iVertex[0];
			int index2 = bspEdge.iVertex[1];

			VECTOR3D vertex1 = vertexList[index1];
			VECTOR3D vertex2 = vertexList[index2];

			DebugOverlay.Line( vertex1.GetVector3(), vertex2.GetVector3() );
		}
	}

	/// <summary>
	/// Called every frame on the client
	/// </summary>
	public override void FrameSimulate( IClient cl )
	{
		base.FrameSimulate( cl );

		// Update rotation every frame, to keep things smooth
		Rotation = ViewAngles.ToRotation();

		Camera.Position = Position;
		Camera.Rotation = Rotation;

		// Set field of view to whatever the user chose in options
		Camera.FieldOfView = Screen.CreateVerticalFieldOfView( Game.Preferences.FieldOfView );

		// Set the first person viewer to this, so it won't render our model
		Camera.FirstPersonViewer = this;
	}
}
