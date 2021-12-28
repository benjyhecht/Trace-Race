using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapLocation       
{
    public int x;
    public int z;

    public MapLocation(int _x, int _z)
    {
        x = _x;
        z = _z;
    }

    public Vector2 ToVector()
    {
        return new Vector2(x, z);
    }

    public override bool Equals(object obj)
    {
        if (obj == null || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            return x == ((MapLocation)obj).x && z == ((MapLocation)obj).z;
        }
    }

    public static MapLocation operator +(MapLocation a, MapLocation b) => new MapLocation(a.x + b.x, a.z + b.z);

    public override int GetHashCode()
    {
        return 0;
    }
}

public class Maze : MonoBehaviour
{
    public int width = 30; //x length
    public int depth = 30; //z length
    public byte[,] map;
    public int scale = 6;

    public float xOffset = 0;
    public float zOffset = 0;

    [System.Serializable]
    public struct Module
    {
        public GameObject prefab;
        public Vector3 rotation;
    }

    public Module wall;
    public Module straightHorizontal;
    public Module straightVertical;
    public Module crossroad;
    public Module cornerLeftDown;
    public Module cornerLeftUp;
    public Module cornerRightUp;
    public Module cornerRightDown;
    public Module tIntersectionUp;
    public Module tIntersectionRight;
    public Module tIntersectionDown;
    public Module tIntersectionLeft;
    public Module endpieceOpenDown;
    public Module endpieceOpenLeft;
    public Module endpieceOpenUp;
    public Module endpieceOpenRight;

    public GameObject FPC;

    public int level = 0;
    public float levelDistance = 2.0f;

    public MapLocation startLocation;
    public MapLocation endLocation;
    MapLocation tempStartingLocation;
    MapLocation tempEndingLocation;
    Vector3 startingRotation = Vector3.zero;
    bool isLoop = false;

    public enum PieceType {Straight_Horizontal,
            Straight_Vertical,
            Corner_Right_Up,
            Corner_Right_Down,
            Corner_Left_Up,
            Corner_Left_Down,
            Junction_Up,
            Junction_Right,
            Junction_Down,
            Junction_Left,
            Crossroad,
            Deadend_Open_Down,
            Deadend_Open_Left,
            Deadend_Open_Up,
            Deadend_Open_Right,
            Wall,
            Room,
            Manhole}

    public struct Pieces
    {
        public PieceType pieceType;
        public GameObject model;

        public Pieces(PieceType pt, GameObject md)
        {
            pieceType = pt;
            model = md;
        }
    }

    public Pieces[,] piecePlaces;
    public List<MapLocation> locations = new List<MapLocation>();
    List<MapLocation> pathLocations = new List<MapLocation>();

    public void Build()
    {
        InitialiseMap();
        Generate();

        byte[,] oldMap = map;
        int oldWidth = width;
        int oldDepth = depth;

        width += 2;
        depth += 2;

        map = new byte[width, depth];
        InitialiseMap();

        for (int z = 0; z < oldDepth; z++)
        {
            for (int x = 0; x < oldWidth; x++)
            {
                map[x + 1, z + 1] = oldMap[x, z];
            }
        }

        PlaceGoal();

        FillInDeadEnds();

        DrawMap();
        if (FPC != null)
        {
            PlaceFPC();
        }
    }

    public void InitialiseMap()
    {
        map = new byte[width,depth];
        piecePlaces = new Pieces[width, depth];
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                map[x, z] = 1;     //1 = wall  0 = corridor
            }
        }
    }

    public void PlaceGoal()
    {
        //right horizontal corridor
        int xpos = width - 2;
        int zpos = Random.Range(depth / 2, depth - 2);
        endLocation = new MapLocation(xpos, zpos);
        if (isLoop)
        {
            endLocation = tempEndingLocation;
        }
        while (map[xpos, zpos] != 0 && xpos > 1)
        {
            map[xpos, zpos] = 0;
            xpos--;
        }

        //Bottom vertical corridor
        xpos = Random.Range(2, width / 2);
        zpos = 1;
        startLocation = new MapLocation(xpos, zpos);
        if (isLoop)
        {
            startLocation = tempStartingLocation;
        }
        while (map[xpos, zpos] != 0 && zpos < depth - 1)
        {
            map[xpos, zpos] = 0;
            zpos++;
        }
    }

    public void FillInDeadEnds()
    {
        MakeTrack trackMaker = GetComponent<MakeTrack>();
        if (trackMaker)
        {
            pathLocations = trackMaker.Build(this);
            //print(pathLocations.Count);
        }
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                MapLocation testPiece = new MapLocation(x, z);

                if (pathLocations.Contains(testPiece))
                {
                    map[x, z] = 0;     //1 = wall  0 = corridor
                }
                else
                {
                    map[x, z] = 1;
                }
            }
        }
        map[startLocation.x, startLocation.z] = 0;
        map[endLocation.x, endLocation.z] = 0;
        //print("Start: " + startLocation.x + ", " + startLocation.z);
        //print("End: " + endLocation.x + ", " + endLocation.z);
    }

    public virtual void PlaceFPC()
    {
        FPC.transform.position = new Vector3((startLocation.x) * scale, .05f, (startLocation.z) * scale);
        FPC.transform.Rotate(startingRotation);
    }

    public virtual void Generate()
    {
        for (int z = 1; z < depth - 1; z++)
        {
            for (int x = 1; x < width - 1; x++)
            {
                if (Random.Range(0, 100) < 50)
                {
                    //map[x, z] = 0;
                }
            }
        }
    }

    public void DrawMap()
    {
        int height = level * scale * 2;

        if (straightVertical.prefab == null)
        {
            straightVertical.prefab = straightHorizontal.prefab;
            straightVertical.rotation = straightHorizontal.rotation + new Vector3(0, 90, 0);
        }

        if (cornerLeftUp.prefab == null)
        {
            cornerLeftUp.prefab = cornerLeftDown.prefab;
            cornerLeftUp.rotation = cornerLeftDown.rotation + new Vector3(0, 90, 0);
        }
        if (cornerRightUp.prefab == null)
        {
            cornerRightUp.prefab = cornerLeftDown.prefab;
            cornerRightUp.rotation = cornerLeftDown.rotation + new Vector3(0, 180, 0);
        }
        if (cornerRightDown.prefab == null)
        {
            cornerRightDown.prefab = cornerLeftDown.prefab;
            cornerRightDown.rotation = cornerLeftDown.rotation + new Vector3(0, 270, 0);
        }

        tIntersectionRight.prefab = tIntersectionUp.prefab;
        tIntersectionRight.rotation = tIntersectionUp.rotation + new Vector3(0, 90, 0);
        tIntersectionDown.prefab = tIntersectionUp.prefab;
        tIntersectionDown.rotation = tIntersectionUp.rotation + new Vector3(0, 180, 0);
        tIntersectionLeft.prefab = tIntersectionUp.prefab;
        tIntersectionLeft.rotation = tIntersectionUp.rotation + new Vector3(0, 270, 0);

        if (endpieceOpenLeft.prefab == null)
        {
            endpieceOpenLeft.prefab = endpieceOpenDown.prefab;
            endpieceOpenLeft.rotation = endpieceOpenDown.rotation + new Vector3(0, 90, 0);
        }
        if (endpieceOpenUp.prefab == null)
        {
            endpieceOpenUp.prefab = endpieceOpenDown.prefab;
            endpieceOpenUp.rotation = endpieceOpenDown.rotation + new Vector3(0, 180, 0);
        }
        if (endpieceOpenRight.prefab == null)
        {
            endpieceOpenRight.prefab = endpieceOpenDown.prefab;
            endpieceOpenRight.rotation = endpieceOpenDown.rotation + new Vector3(0, 270, 0);
        }

        for (int z = 1; z < depth - 1; z++)
        {
            for (int x = 1; x < width - 1; x++)
            {
                Vector3 pos = new Vector3((x + transform.position.x) * scale, height, (z + transform.position.z) * scale);
                if (map[x, z] == 1)
                {
                    CreatePiece(x, z, wall, pos, PieceType.Wall, "Wall");
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 1, 5, 1, 5 })) //end piece open left
                {
                    CreatePiece(x, z, endpieceOpenLeft, pos, PieceType.Deadend_Open_Left, "Deadend Open Left");
                    if (startLocation.x == x && (startLocation.z) == z)
                    {
                        startingRotation = new Vector3(0, 90, 0);
                    }
                }
                else if (Search2D(x, z, new int[] { 5, 0, 5, 1, 0, 1, 5, 1, 5 })) //end piece open up
                {
                    CreatePiece(x, z, endpieceOpenUp, pos, PieceType.Deadend_Open_Up, "Deadend Open Up");
                    if (startLocation.x == x && (startLocation.z) == z)
                    {
                        startingRotation = new Vector3(0, 180, 0);
                    }
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 1, 0, 0, 5, 1, 5 })) //end piece open right
                {
                    CreatePiece(x, z, endpieceOpenRight, pos, PieceType.Deadend_Open_Right, "Deadend Open Right");
                    if (startLocation.x == x && (startLocation.z) == z)
                    {
                        startingRotation = new Vector3(0, 270, 0);
                    }
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 1, 0, 1, 5, 0, 5 })) //end piece open down
                {
                    CreatePiece(x, z, endpieceOpenDown, pos, PieceType.Deadend_Open_Down, "Deadend Open Down");
                    if (startLocation.x == x && (startLocation.z) == z)
                    {
                        startingRotation = new Vector3(0, 0, 0);
                    }
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 0, 5, 1, 5 })) //horizontal straight
                {
                    CreatePiece(x, z, straightHorizontal, pos, PieceType.Straight_Horizontal, "Straight Horizontal");
                }
                else if (Search2D(x, z, new int[] { 5, 0, 5, 1, 0, 1, 5, 0, 5 })) //vertical straight
                {
                    CreatePiece(x, z, straightVertical, pos, PieceType.Straight_Vertical, "Straight Vertical");
                }
                else if (Search2D(x, z, new int[] { 5, 0, 5, 0, 0, 0, 5, 0, 5 })) //crossroad
                {
                    CreatePiece(x, z, crossroad, pos, PieceType.Crossroad, "Crossroads");
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 1, 5, 0, 5 })) //bottom left corner
                {
                    CreatePiece(x, z, cornerLeftDown, pos, PieceType.Corner_Left_Down, "Corner Left Down");
                }
                else if (Search2D(x, z, new int[] { 5, 0, 5, 0, 0, 1, 5, 1, 5 })) //upper left corner
                {
                    CreatePiece(x, z, cornerLeftUp, pos, PieceType.Corner_Right_Up, "Corner Left Up");
                }
                else if (Search2D(x, z, new int[] { 5, 0, 5, 1, 0, 0, 5, 1, 5 })) //upper right corner
                {
                    CreatePiece(x, z, cornerRightUp, pos, PieceType.Corner_Right_Up, "Corner Right Up");
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 1, 0, 0, 5, 0, 5 })) //lower right corner
                {
                    CreatePiece(x, z, cornerRightDown, pos, PieceType.Corner_Right_Down, "Corner Right Down");
                }
                else if (Search2D(x, z, new int[] { 5, 0, 5, 0, 0, 0, 5, 1, 5 })) //tjunc closed bottom
                {
                    CreatePiece(x, z, tIntersectionUp, pos, PieceType.Junction_Up, "Junction Up");
                }
                else if (Search2D(x, z, new int[] { 5, 0, 5, 1, 0, 0, 5, 0, 5 })) //tjunc closed left
                {
                    CreatePiece(x, z, tIntersectionRight, pos, PieceType.Junction_Right, "Junction Right");
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 0, 5, 0, 5 })) //tjunc closed top
                {
                    CreatePiece(x, z, tIntersectionDown, pos, PieceType.Junction_Down, "Junction Down");
                }
                else if (Search2D(x, z, new int[] { 5, 0, 5, 0, 0, 1, 5, 0, 5 })) //tjunc closed right
                {
                    CreatePiece(x, z, tIntersectionLeft, pos, PieceType.Junction_Left, "Junction Left");
                }
                else
                {
                    GameObject block = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    block.transform.localScale = new Vector3(scale, scale, scale);
                    block.transform.position = pos;
                    block.transform.SetParent(this.gameObject.transform);
                    piecePlaces[x, z].pieceType = PieceType.Room;
                    piecePlaces[x, z].model = block;
                    block.name = "Room";
                }
            }
        }
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                if (map[x,z] != 1)
                {
                    locations.Add(new MapLocation(x, z));
                }
            }
        }
    }

    public void CreatePiece(int x, int z, Module mod, Vector3 blockPosition, PieceType pieceType, string blockName)
    {
        GameObject block = Instantiate(mod.prefab);
        block.transform.SetParent(this.gameObject.transform);
        block.transform.transform.position = blockPosition;
        block.transform.Rotate(mod.rotation);
        piecePlaces[x, z].pieceType = pieceType;
        piecePlaces[x, z].model = block;
        block.name = blockName;
        if (endLocation.x == x && (endLocation.z) == z)
        {
            if (block.GetComponent<DeadEndCollisionEnabler>())
            {
                block.GetComponent<DeadEndCollisionEnabler>().SetColliderOn();
            }
        }
    }

    bool Search2D(int c, int r, int[] pattern)
    {
        int count = 0;
        int pos = 0;
        for (int z = 1; z > -2; z--)
        {
            for (int x = -1; x < 2; x++)
            {
                if (pattern[pos] == map[c + x, r + z] || pattern[pos] == 5)
                {
                    count++;
                }
                pos++;
            }
        }
        return (count == 9);
    }

    public int CountSquareNeighbours(int x, int z)
    {
        int count = 0;
        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1)
        {
            return 5;
        }
        if (map[x - 1, z] == 0)
        {
            count++;
        }
        if (map[x + 1, z] == 0)
        {
            count++;
        }
        if (map[x, z + 1] == 0)
        {
            count++;
        }
        if (map[x, z - 1] == 0)
        {
            count++;
        }
        return count;
    }

    public int CountDiagonalNeighbours(int x, int z)
    {
        int count = 0;
        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1)
        {
            return 5;
        }
        if (map[x - 1, z - 1] == 0)
        {
            count++;
        }
        if (map[x + 1, z + 1] == 0)
        {
            count++;
        }
        if (map[x - 1, z + 1] == 0)
        {
            count++;
        }
        if (map[x + 1, z - 1] == 0)
        {
            count++;
        }
        return count;
    }

    public int CountAllNeighbours(int x, int z)
    {
        return CountSquareNeighbours(x,z) + CountDiagonalNeighbours(x,z);
    }
}
