
//------------------------------//
//  TerrainAlphamaps.js         //
//  Written by Alucard Jay      //
//  11/10/2013                  //
//------------------------------//
 
//  based on Terrain Toolkit ( TerrainToolkit.cs ) by sixtimesnothing
 
 
#pragma strict
 
 
#if UNITY_EDITOR
 
@ContextMenu( "Generate AlphaMap" )
 
function GenerateAlphaMap()
{
    // Load or find the terrain and get Terrain Data
    GetTerrainData();
   
    // Texture the Terrain
    switch ( textureBy )
    {
        case TextureMethod.UsingHeights :
            TextureTerrainUsingHeights();
        break;
       
        case TextureMethod.UsingSlopeAngles :
            TextureTerrainUsingSlopeAngles();
        break;
    }
}
 
#endif
 
 
//  Script Management
//  ----------------------------------------------------------------------------
 
 
enum TextureMethod
{
    UsingHeights,
    UsingSlopeAngles
}
 
public var textureBy : TextureMethod;
 
//  Terrain Data
//  ----------------------------------------------------------------------------
 
 
public var terrain : Terrain;
private var terrainData : TerrainData;
 
private var heightmapWidth : int;
private var heightmapHeight : int;
 
 
function GetTerrainData()
{
    if ( !terrain )
    {
        terrain = Terrain.activeTerrain;
    }
   
    terrainData = terrain.terrainData;
   
    heightmapWidth = terrain.terrainData.heightmapWidth;
    heightmapHeight = terrain.terrainData.heightmapHeight;
}
 
 
//  ============================================================================
 
 
//  ============================================================================
//  
//  
//  below is written based from sixtimesnothing Terrain Toolkit ( TerrainToolkit.cs )
//  
//  http://www.sixtimesnothing.com/terraintoolkit
//  
//  http://sixtimesnothing.wordpress.com/
//  
//  http://code.google.com/p/unityterraintoolkit/
//  
//
//  ============================================================================
 
 
//  Terrain Texturing
//  ----------------------------------------------------------------------------
 
 
// -- Original using Heights --
 
 
private var splatPrototypes : SplatPrototype[];
 
public var slopeBlendMinAngle : float = 32.0;
public var slopeBlendMaxAngle : float = 48.0;
 
public var heightBlendPoints : float[];
 
 
function TextureTerrainUsingHeights()
{
    // store a reference to the terrain textures
    splatPrototypes = terrain.terrainData.splatPrototypes;
   
   
    // check there are 4 textures
    var nTextures : int = splatPrototypes.Length;
   
    if ( nTextures != 4 )
    {
        Debug.LogError( "Error: You must assign 4 textures." );
        return;
    }
   
    // check there are 4 values in the blend points arrays
    var nHeights : int = heightBlendPoints.Length;
   
    if ( nHeights != 4 )
    {
        Debug.LogError( "Error: You must assign 4 values to the Height BlendPoints array." );
        //return;
       
        // example of how arrays should be populated
       
        // heights are normalized values
        heightBlendPoints = new float[4];
        heightBlendPoints[0] = 0.2;
        heightBlendPoints[1] = 0.35;
        heightBlendPoints[2] = 0.5;
        heightBlendPoints[3] = 0.65;
    }
   
    // --
   
   
    var Tw : int = terrainData.heightmapWidth - 1;
    var Th : int = terrainData.heightmapHeight - 1;
   
    var heightMapTexData : float[,] = new float[Tw, Th];
    var slopeMapData : float[,] = new float[Tw, Th];
    var splatMapData : float[,,];
   
    terrainData.alphamapResolution = Tw;
    splatMapData = terrainData.GetAlphamaps(0, 0, Tw, Tw);
   
   
    // Angles to difference...
    var terSize : Vector3 = terrainData.size;
    var slopeBlendMinimum : float = ((terSize.x / Tw) * Mathf.Tan(slopeBlendMinAngle * Mathf.Deg2Rad)) / terSize.y;
    var slopeBlendMaximum : float = ((terSize.x / Tw) * Mathf.Tan(slopeBlendMaxAngle * Mathf.Deg2Rad)) / terSize.y;
   
   
    // ----
   
    var greatestHeight : float = 0.0;
    var xNeighbours : int;
    var yNeighbours : int;
    var xShift : int;
    var yShift : int;
    var xIndex : int;
    var yIndex : int;
    var heightMap : float[,] = terrainData.GetHeights( 0, 0, Tw, Th );
   
    var slopeAngles : float[,] = new float[ Tw, Th ];
   
   
    for (var Ty : int = 0; Ty < Th; Ty++)
    {
        // y...
        if (Ty == 0)
        {
            yNeighbours = 2;
            yShift = 0;
            yIndex = 0;
        }
        else if (Ty == Th - 1)
        {
            yNeighbours = 2;
            yShift = -1;
            yIndex = 1;
        }
        else
        {
            yNeighbours = 3;
            yShift = -1;
            yIndex = 1;
        }
       
        for (var Tx : int = 0; Tx < Tw; Tx++)
        {
            // x...
            if (Tx == 0)
            {
                xNeighbours = 2;
                xShift = 0;
                xIndex = 0;
            }
            else if (Tx == Tw - 1)
            {
                xNeighbours = 2;
                xShift = -1;
                xIndex = 1;
            }
            else
            {
                xNeighbours = 3;
                xShift = -1;
                xIndex = 1;
            }
           
           
            // ----
           
           
            // Get height...
            var h : float = heightMap[ Tx + xIndex + xShift, Ty + yIndex + yShift ];
           
            if (h > greatestHeight)
            {
                greatestHeight = h;
                //Debug.Log( h );
            }
           
            // ...and apply to height map...
            heightMapTexData[Tx, Ty] = h;
           
           
            // ----
           
           
            // Calculate slope...
            var tCumulative : float = 0.0f;
            var nNeighbours : float = xNeighbours * yNeighbours - 1;
            var Ny : int;
            var Nx : int;
            var t : float;
           
            for (Ny = 0; Ny < yNeighbours; Ny++)
            {
                for (Nx = 0; Nx < xNeighbours; Nx++)
                {
                    // Ignore the index...
                    if (Nx != xIndex || Ny != yIndex)
                    {
                        t = Mathf.Abs( h - heightMap[ Tx + Nx + xShift, Ty + Ny + yShift ] );
                        tCumulative += t;
                    }
                }
            }
           
            var tAverage : float = tCumulative / nNeighbours;
           
            // ...and apply to the slope map...
            slopeMapData[Tx, Ty] = tAverage;
        }
       
        // Show progress...
        // float completePoints = Ty * Th;
        // float totalPoints = Tw * Th;
        // float percentComplete = (completePoints / totalPoints) * 0.6f;
        // textureProgressDelegate("Procedural Terrain Texture", "Generating height and slope maps. Please wait.", percentComplete);
    }
   
    // Blend slope...
    var sBlended : float;
    var Px : int;
    var Py : int;
   
    for (Py = 0; Py < Th; Py++)
    {
        for (Px = 0; Px < Tw; Px++)
        {
            sBlended = slopeMapData[Px, Py];
           
            if (sBlended < slopeBlendMinimum)
            {
                sBlended = 0;
            }
            else if (sBlended < slopeBlendMaximum)
            {
                sBlended = (sBlended - slopeBlendMinimum) / (slopeBlendMaximum - slopeBlendMinimum);
            }
            else if (sBlended > slopeBlendMaximum)
            {
                sBlended = 1;
            }
           
            slopeMapData[Px, Py] = sBlended;
            splatMapData[Px, Py, 0] = sBlended;
        }
    }
   
    // Blend height maps...
    for (var i : int = 1; i < nTextures; i++)
    {
        for (Py = 0; Py < Th; Py++)
        {
            for (Px = 0; Px < Tw; Px++)
            {
                var hBlendInMinimum : float = 0;
                var hBlendInMaximum : float = 0;
                var hBlendOutMinimum : float = 1;
                var hBlendOutMaximum : float = 1;
                var hValue : float;
                var hBlended : float = 0;
               
                // --
               
                // using heights
               
                if (i > 1)
                {
                    hBlendInMinimum = parseFloat( heightBlendPoints[i * 2 - 4] );
                    hBlendInMaximum = parseFloat( heightBlendPoints[i * 2 - 3] );
                }
               
                if (i < nTextures - 1)
                {
                    hBlendOutMinimum = parseFloat( heightBlendPoints[i * 2 - 2] );
                    hBlendOutMaximum = parseFloat( heightBlendPoints[i * 2 - 1] );
                }
               
                hValue = heightMapTexData[ Px, Py ];
               
                // --
               
                if (hValue >= hBlendInMaximum && hValue <= hBlendOutMinimum)
                {
                    // Full...
                    hBlended = 1;
                }
                else if (hValue >= hBlendInMinimum && hValue < hBlendInMaximum)
                {
                    // Blend in...
                    hBlended = (hValue - hBlendInMinimum) / (hBlendInMaximum - hBlendInMinimum);
                }
                else if (hValue > hBlendOutMinimum && hValue <= hBlendOutMaximum)
                {
                    // Blend out...
                    hBlended = 1 - ((hValue - hBlendOutMinimum) / (hBlendOutMaximum - hBlendOutMinimum));
                }
               
                // --
               
                // Subtract slope...
                var sValue : float = slopeMapData[ Px, Py ];
                hBlended -= sValue;
               
                if (hBlended < 0)
                {
                    hBlended = 0;
                }
               
                splatMapData[ Px, Py, i ] = hBlended;
               
                // --
            }
        }
    }
   
   
    // Generate splat maps...
    terrainData.SetAlphamaps( 0, 0, splatMapData );
   
   
    // Clean up...
    heightMapTexData = null;
    slopeMapData = null;
    splatMapData = null;
   
    // ----
   
    Debug.Log( "Splatmap Updated using Heights" );
}
 
 
//  ============================================================================
 
 
// -- Modified using Slope Angles --
 
 
public var slopeBlendPoints : float[];
 
 
function TextureTerrainUsingSlopeAngles()
{
    // store a reference to the terrain textures
    splatPrototypes = terrain.terrainData.splatPrototypes;
   
   
    // check there are 4 textures
    var nTextures : int = splatPrototypes.Length;
   
    if ( nTextures != 4 )
    {
        Debug.LogError( "Error: You must assign 4 textures." );
        return;
    }
   
    // check there are 4 values in the blend points arrays
    var nSlopes : int = slopeBlendPoints.Length;
   
    if ( nSlopes != 4 )
    {
        Debug.LogError( "Error: You must assign 4 values to the Slope BlendPoints array." );
        //return;
       
        // example of how arrays should be populated
       
        // slopes are angles in degrees
        slopeBlendPoints = new float[4];
        slopeBlendPoints[0] = 10.0;
        slopeBlendPoints[1] = 15.0;
        slopeBlendPoints[2] = 27.0;
        slopeBlendPoints[3] = 32.0;
    }
   
    // --
   
   
    var Tw : int = terrainData.heightmapWidth - 1;
    var Th : int = terrainData.heightmapHeight - 1;
   
    var heightMapTexData : float[,] = new float[Tw, Th];
    var slopeMapData : float[,] = new float[Tw, Th];
    var splatMapData : float[,,];
   
    terrainData.alphamapResolution = Tw;
    splatMapData = terrainData.GetAlphamaps(0, 0, Tw, Tw);
   
   
    // Angles to difference...
    var terSize : Vector3 = terrainData.size;
    var slopeBlendMinimum : float = ((terSize.x / Tw) * Mathf.Tan(slopeBlendMinAngle * Mathf.Deg2Rad)) / terSize.y;
    var slopeBlendMaximum : float = ((terSize.x / Tw) * Mathf.Tan(slopeBlendMaxAngle * Mathf.Deg2Rad)) / terSize.y;
   
   
    // ----
   
    var greatestHeight : float = 0.0;
    var xNeighbours : int;
    var yNeighbours : int;
    var xShift : int;
    var yShift : int;
    var xIndex : int;
    var yIndex : int;
    var heightMap : float[,] = terrainData.GetHeights( 0, 0, Tw, Th );
   
    var slopeAngles : float[,] = new float[ Tw, Th ];
   
   
    for (var Ty : int = 0; Ty < Th; Ty++)
    {
        // y...
        if (Ty == 0)
        {
            yNeighbours = 2;
            yShift = 0;
            yIndex = 0;
        }
        else if (Ty == Th - 1)
        {
            yNeighbours = 2;
            yShift = -1;
            yIndex = 1;
        }
        else
        {
            yNeighbours = 3;
            yShift = -1;
            yIndex = 1;
        }
       
        for (var Tx : int = 0; Tx < Tw; Tx++)
        {
            // x...
            if (Tx == 0)
            {
                xNeighbours = 2;
                xShift = 0;
                xIndex = 0;
            }
            else if (Tx == Tw - 1)
            {
                xNeighbours = 2;
                xShift = -1;
                xIndex = 1;
            }
            else
            {
                xNeighbours = 3;
                xShift = -1;
                xIndex = 1;
            }
           
           
            // ----
           
           
            // Get height...
            var h : float = heightMap[ Tx + xIndex + xShift, Ty + yIndex + yShift ];
           
            if (h > greatestHeight)
            {
                greatestHeight = h;
                //Debug.Log( h );
            }
           
            // ...and apply to height map...
            heightMapTexData[Tx, Ty] = h;
           
           
            // --
           
            // get slope angle instead
           
            var slopeX : float = ( Tx + xIndex + xShift );
            var slopeY : float = ( Ty + yIndex + yShift );
           
            var readPos : Vector2 = new Vector2( slopeX / Tx, slopeY / Ty );
           
            slopeAngles[ Tx, Ty ] = terrainData.GetSteepness( readPos.y, readPos.x ); // y, x : reversed as heightmap is reversed
           
           
            // ----
           
           
            // Calculate slope...
            var tCumulative : float = 0.0f;
            var nNeighbours : float = xNeighbours * yNeighbours - 1;
            var Ny : int;
            var Nx : int;
            var t : float;
           
            for (Ny = 0; Ny < yNeighbours; Ny++)
            {
                for (Nx = 0; Nx < xNeighbours; Nx++)
                {
                    // Ignore the index...
                    if (Nx != xIndex || Ny != yIndex)
                    {
                        t = Mathf.Abs( h - heightMap[ Tx + Nx + xShift, Ty + Ny + yShift ] );
                        tCumulative += t;
                    }
                }
            }
           
            var tAverage : float = tCumulative / nNeighbours;
           
            // ...and apply to the slope map...
            slopeMapData[Tx, Ty] = tAverage;
        }
       
        // Show progress...
        // float completePoints = Ty * Th;
        // float totalPoints = Tw * Th;
        // float percentComplete = (completePoints / totalPoints) * 0.6f;
        // textureProgressDelegate("Procedural Terrain Texture", "Generating height and slope maps. Please wait.", percentComplete);
    }
   
    // Blend slope...
    var sBlended : float;
    var Px : int;
    var Py : int;
   
    for (Py = 0; Py < Th; Py++)
    {
        for (Px = 0; Px < Tw; Px++)
        {
            sBlended = slopeMapData[Px, Py];
           
            if (sBlended < slopeBlendMinimum)
            {
                sBlended = 0;
            }
            else if (sBlended < slopeBlendMaximum)
            {
                sBlended = (sBlended - slopeBlendMinimum) / (slopeBlendMaximum - slopeBlendMinimum);
            }
            else if (sBlended > slopeBlendMaximum)
            {
                sBlended = 1;
            }
           
            slopeMapData[Px, Py] = sBlended;
            splatMapData[Px, Py, 0] = sBlended;
        }
    }
   
    // Blend height maps...
    for (var i : int = 1; i < nTextures; i++)
    {
        for (Py = 0; Py < Th; Py++)
        {
            for (Px = 0; Px < Tw; Px++)
            {
                var hBlendInMinimum : float = 0;
                var hBlendInMaximum : float = 0;
                var hBlendOutMinimum : float = 1;
                var hBlendOutMaximum : float = 1;
                var hValue : float;
                var hBlended : float = 0;
               
                // --
               
                /*
               
                // using heights
               
                if (i > 1)
                {
                    hBlendInMinimum = parseFloat( heightBlendPoints[i * 2 - 4] );
                    hBlendInMaximum = parseFloat( heightBlendPoints[i * 2 - 3] );
                }
               
                if (i < nTextures - 1)
                {
                    hBlendOutMinimum = parseFloat( heightBlendPoints[i * 2 - 2] );
                    hBlendOutMaximum = parseFloat( heightBlendPoints[i * 2 - 1] );
                }
               
                */
               
                // --
               
                // using slope angles
               
                if (i > 1)
                {
                    hBlendInMinimum = parseFloat( slopeBlendPoints[i * 2 - 4] );
                    hBlendInMaximum = parseFloat( slopeBlendPoints[i * 2 - 3] );
                }
               
                if (i < nTextures - 1)
                {
                    hBlendOutMinimum = parseFloat( slopeBlendPoints[i * 2 - 2] );
                    hBlendOutMaximum = parseFloat( slopeBlendPoints[i * 2 - 1] );
                }
               
               
                // --
               
                //using heights
                //hValue = heightMapTexData[ Px, Py ];
               
                // now using slope angles
                hValue = slopeAngles[ Px, Py ];
               
                // normalize all angle values to 90 degrees
                var maxAngle : float = 90.0;
               
                hBlendInMinimum /= maxAngle;
                hBlendInMaximum /= maxAngle;
                hBlendOutMinimum /= maxAngle;
                hBlendOutMaximum /= maxAngle;
               
                hValue /= maxAngle;
               
               
                // --
               
               
                if (hValue >= hBlendInMaximum && hValue <= hBlendOutMinimum)
                {
                    // Full...
                    hBlended = 1;
                }
                else if (hValue >= hBlendInMinimum && hValue < hBlendInMaximum)
                {
                    // Blend in...
                    hBlended = (hValue - hBlendInMinimum) / (hBlendInMaximum - hBlendInMinimum);
                }
                else if (hValue > hBlendOutMinimum && hValue <= hBlendOutMaximum)
                {
                    // Blend out...
                    hBlended = 1 - ((hValue - hBlendOutMinimum) / (hBlendOutMaximum - hBlendOutMinimum));
                }
               
                // --
               
                // Subtract slope...
                var sValue : float = slopeMapData[ Px, Py ];
                hBlended -= sValue;
               
                if (hBlended < 0)
                {
                    hBlended = 0;
                }
               
                splatMapData[ Px, Py, i ] = hBlended;
               
                // --
            }
        }
    }
   
   
    // Generate splat maps...
    terrainData.SetAlphamaps( 0, 0, splatMapData );
   
   
    // Clean up...
    slopeMapData = null;
    splatMapData = null;
    slopeAngles = null;
   
    // ----
   
    Debug.Log( "Splatmap Updated using Slope Angles" );
}
 
 
// =================================================================================