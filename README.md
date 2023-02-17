# BSPHL
BSPHL - BSP HotLoader for S&amp;box

## Goals:
This will be a way to open and enjoy Half-Life 1 maps in S&box natively.

## Current progress:
The state of this S&box addon is extremely early, thus only proof-of-concept features are implemented.

It reads the following information from BSP files (using S&box's [FileSystem API](https://wiki.facepunch.com/sbox/FileSystem)):
- Vertices
- Edges

Using these details, it plots the geometry using DebugOverlay:

![](preview_snark_pit.gif)

## Loading a map
Note: for now, the map name is hardcoded into [Pawn.cs](code/Pawn.cs#L78).
1. Navigate to `<steam>/common/sbox/data/local/bsphl#local` (create folders if they don't exist)
2. Drop a HL1 bsp file in there
3. Change the map file name in [Pawn.cs](code/Pawn.cs#L78)
4. Launch the gamemode and press left click to load, and hold right click to raise the geometry above ground

## Footnotes
HUGE thanks to the [hlbsp project](https://hlbsp.sourceforge.net/index.php?content=bspdef) for documenting the structure of the HL1 BSP format
