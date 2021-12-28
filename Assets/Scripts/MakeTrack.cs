using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class PathMarker
{
    public MapLocation location;
    public float G;
    public float H;
    public float F;

    public PathMarker parent;

    public PathMarker(MapLocation l, float g, float h, float f, PathMarker p)
    {
        location = l;
        G = g;
        H = h;
        F = f;
        parent = p;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            return location.x == ((PathMarker)obj).location.x && location.z == ((PathMarker)obj).location.z;
        }
    }

    public override int GetHashCode()
    {
        return 0;
    }
}

public class MakeTrack : MonoBehaviour
{
    Maze maze;

    List<PathMarker> open = new List<PathMarker>();
    List<PathMarker> closed = new List<PathMarker>();
    List<MapLocation> pathLocations = new List<MapLocation>();

    PathMarker goalNode;
    PathMarker startNode;
    PathMarker lastPos;
    bool done = false;

    public void BeginSearch()
    {
        List<MapLocation> locations = new List<MapLocation>();
        for (int z = 0; z < maze.depth - 1; z++)
        {
            for (int x = 0; x < maze.width - 1; x++)
            {
                if (maze.map[x,z] != 1)
                {
                    locations.Add(new MapLocation(x, z));
                }
            }
        }

        startNode = new PathMarker(maze.startLocation, 0, 0, 0, null);

        goalNode = new PathMarker(maze.endLocation, 0, 0, 0, null);

        open.Clear();
        closed.Clear();
        pathLocations.Clear();

        open.Add(startNode);
        lastPos = startNode;
        //print(startNode.location.x + ", " + startNode.location.z);
        //print(goalNode.location.x + ", " + goalNode.location.z);
    }

    void Search(PathMarker thisNode)
    {
        if (thisNode.Equals(goalNode))
        {
            done = true;
            //print("Done!");
            return;
        }

        List<MapLocation> directions = new List<MapLocation>() { new MapLocation(0, 1), new MapLocation(1, 0), new MapLocation(0, -1), new MapLocation(-1, 0) };

        foreach(MapLocation dir in directions)
        {
            MapLocation neighbor = dir + thisNode.location;
            if (maze.map[neighbor.x, neighbor.z] == 1) continue;
            if (neighbor.x < 0 || neighbor.z < 0 || neighbor.x > maze.width || neighbor.z > maze.depth) continue;
            if (IsClosed(neighbor)) continue;

            float G = Vector2.Distance(thisNode.location.ToVector(), neighbor.ToVector()) + thisNode.G;
            float H = Vector2.Distance(goalNode.location.ToVector(), neighbor.ToVector());
            float F = G + H;

            if (!UpdateMarker(neighbor, G, H, F, thisNode))
            {
                open.Add(new PathMarker(neighbor, G, H, F, thisNode));
            }
        }

        open = open.OrderBy(p => p.F).ToList();
        PathMarker pm = open.ElementAt(0);
        closed.Add(pm);
        open.RemoveAt(0);
        lastPos = pm;
    }

    bool UpdateMarker(MapLocation pos, float g, float h, float f, PathMarker prt)
    {
        foreach(PathMarker p in open)
        {
            if (p.location.Equals(pos))
            {
                p.G = g;
                p.H = h;
                p.F = f;
                p.parent = prt;
                return true;
            }
        }
        return false;
    }

    bool IsClosed(MapLocation marker)
    {
        foreach(PathMarker p in closed)
        {
            if (p.location.Equals(marker))
            {
                return true;
            }
        }
        return false;
    }

    public List<MapLocation> Build(Maze maze)
    {
        this.maze = maze;
        BeginSearch();
        int loopCount = 0;
        while (!done && loopCount < 15000)
        {
            Search(lastPos);
            loopCount++;
        }
        if (loopCount == 15000)
        {
            print("Loop count reached for search");
        }

        MarkPath();

        return pathLocations;
    }

    void MarkPath()
    {
        PathMarker currentPath = lastPos;
        int loopCount = 0;
        while (!currentPath.Equals(startNode) && loopCount < 5000)
        {
            maze.map[currentPath.location.x, currentPath.location.z] = 0;
            pathLocations.Add(currentPath.location);
            currentPath = currentPath.parent;
            loopCount++;
        }
        if (loopCount == 5000)
        {
            print("Loop count reached marking path");
        }
        maze.map[startNode.location.x, startNode.location.z] = 0;
    }
}
