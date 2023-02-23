# BSPHL
BSPHL - BSP HotLoader for S&amp;box

## Goals:
This will be a way to open and enjoy GoldSrc (and eventually Source) maps in S&box natively.

## Current state:
The state of this S&box addon is extremely early, thus only proof-of-concept features are implemented.

It reads information from BSP files using S&box's [FileSystem API](https://wiki.facepunch.com/sbox/FileSystem).

Using these details, it constructs a mesh using VertexBuffers:

![sbox-dev_zZp9kvvz7m](https://user-images.githubusercontent.com/21970287/221029959-432bf148-e27b-4e92-bc5c-696f656bdb31.png)

## Loading a map
Note: for now, the map name is hardcoded into [Pawn.cs](code/Pawn.cs#L78).
1. Navigate to `<steam>/common/sbox/data/local/bsphl#local` (create folders if they don't exist)
2. Drop a HL1 bsp file in there
3. Change the map file name in [Pawn.cs](code/Pawn.cs#L75)
4. Launch the gamemode and press left click to spawn the map on your position

## Footnotes
HUGE thanks to the [hlbsp project](https://hlbsp.sourceforge.net/index.php?content=bspdef) for documenting the structure of the HL1 BSP format
