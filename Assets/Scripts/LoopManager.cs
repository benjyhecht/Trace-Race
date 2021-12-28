using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopManager : MonoBehaviour
{
    [SerializeField] Maze maze;
    [SerializeField] int mazeSize = 2;
    [SerializeField] int scale = 3;
    [SerializeField] int width = 30;
    [SerializeField] int depth = 30;
    [SerializeField] GameObject car;
    [SerializeField] GameObject wall;
    [SerializeField] GameObject[] straights;
    [SerializeField] GameObject[] corners;
    [SerializeField] GameObject[] starts;

    List<MapLocation> groundLocations = new List<MapLocation>();
    List<int> xLocations = new List<int>();
    List<int> zLocations = new List<int>();

    GameObject[] straightLoops = new GameObject[8];
    GameObject[] cornerLoops = new GameObject[8];
    GameObject[] startLoops = new GameObject[8];

    void Start()
    {
        for (int i = 0; i < corners.Length; i++)
        {
            cornerLoops[i] = corners[i];
            cornerLoops[i + 4] = corners[i];
            startLoops[i] = starts[i];
            startLoops[i + 4] = starts[i];
        }

        for (int i = 0; i < straights.Length; i++)
        {
            straightLoops[i] = straights[i];
            straightLoops[i + 2] = straights[i];
            straightLoops[i + 4] = straights[i];
            straightLoops[i + 6] = straights[i];
        }

        BuildMaze1();
        BuildMaze2();
        BuildMaze3();
        BuildMaze4();
        AnalyzeHorizontalSpaces(xLocations[0], xLocations[3], 0);
        AnalyzeHorizontalSpaces(xLocations[1], xLocations[2], 0);
        AnalyzeVerticalSpaces(zLocations[0], zLocations[1], 0);
        AnalyzeVerticalSpaces(zLocations[3], zLocations[2], 0);
        FillInHoles();
        car.transform.position += new Vector3(-48, 0, 0);

    }

    public void BuildMaze1()
    {
        Maze createdMaze = InstantiateMaze();
        updatePrefabs(createdMaze, 0);
        createdMaze.FPC = car;
        createdMaze.Build();
        createdMaze.name = "maze 1";
        createdMaze.transform.Rotate(new Vector3(0, 0, 0));
        createdMaze.transform.position = new Vector3(-(width + 1) * scale, 0, 0);
        xLocations.Add(createdMaze.startLocation.x - 16);
        zLocations.Add(createdMaze.endLocation.z);        
    }

    public void BuildMaze2()
    {
        Maze createdMaze = InstantiateMaze();
        updatePrefabs(createdMaze, 1);
        createdMaze.endpieceOpenDown = new Maze.Module();
        createdMaze.endpieceOpenDown.prefab = straightLoops[1];
        createdMaze.endpieceOpenDown.rotation = new Vector3(0, 180, 0);

        createdMaze.endpieceOpenUp = new Maze.Module();
        createdMaze.endpieceOpenUp.prefab = startLoops[1];
        createdMaze.endpieceOpenUp.rotation = new Vector3(0, 270, 0);

        createdMaze.Build();
        createdMaze.name = "maze 2";
        createdMaze.transform.position = new Vector3(0, 0, (depth + 1) * scale);
        createdMaze.transform.localEulerAngles = new Vector3(0, 90, 0);
        zLocations.Add(16 - createdMaze.startLocation.x);
        xLocations.Add(createdMaze.endLocation.z);
    }

    public void BuildMaze3()
    {
        Maze createdMaze = InstantiateMaze();
        updatePrefabs(createdMaze, 2);
        createdMaze.name = "maze 3";
        createdMaze.endpieceOpenUp = new Maze.Module();
        createdMaze.endpieceOpenUp.prefab = startLoops[2];
        createdMaze.endpieceOpenUp.rotation = new Vector3(0, 180, 0);

        createdMaze.endpieceOpenDown = new Maze.Module();
        createdMaze.endpieceOpenDown.prefab = straightLoops[2];
        createdMaze.endpieceOpenDown.rotation = new Vector3(0, 90, 0);

        createdMaze.Build();
        createdMaze.transform.position = new Vector3((width + 1) * scale, 0, 0);
        createdMaze.transform.localEulerAngles = new Vector3(0, 180, 0);
        xLocations.Add(16 - createdMaze.startLocation.x);
        zLocations.Add(-createdMaze.endLocation.z);
    }

    public void BuildMaze4()
    {
        Maze createdMaze = InstantiateMaze();
        //updatePrefabs(createdMaze, 3);
        createdMaze.name = "maze 4";
        createdMaze.endpieceOpenUp = new Maze.Module();
        createdMaze.endpieceOpenUp.prefab = startLoops[3];
        createdMaze.endpieceOpenUp.rotation = new Vector3(0, 90, 0);

        createdMaze.endpieceOpenDown = new Maze.Module();
        createdMaze.endpieceOpenDown.prefab = straightLoops[3];
        createdMaze.endpieceOpenDown.rotation = new Vector3(0, 0, 0);

        createdMaze.Build();
        createdMaze.transform.position = new Vector3(0, 0, -(depth + 1) * scale);
        createdMaze.transform.localEulerAngles = new Vector3(0, 270, 0);
        zLocations.Add(createdMaze.startLocation.x - 16);
        xLocations.Add(-createdMaze.endLocation.z);
    }

    public Maze InstantiateMaze()
    {
        Maze createdMaze = Instantiate(maze, Vector3.zero, Quaternion.identity, this.transform);
        createdMaze.width = width;
        createdMaze.depth = depth;
        return createdMaze;
    }

    public void AnalyzeHorizontalSpaces(int loc1, int loc2, int addition)
    {
        int min = loc1;
        int max = loc2;
        groundLocations.Add(new MapLocation(loc1, 0));
        if (loc1 != loc2)
        {
            groundLocations.Add(new MapLocation(loc2, 0));
        }
        if (loc1 < loc2)
        {
            GameObject corner1 = Instantiate(cornerLoops[0 + addition], new Vector3(loc1 * scale, 0, 0), Quaternion.Euler(0, 0, 0), this.transform);
            corner1.name = "corner" + loc1;
            GameObject corner2 = Instantiate(cornerLoops[2 + addition], new Vector3(loc2 * scale, 0, 0), Quaternion.Euler(0, 0, 0), this.transform);
            corner2.name = "corner" + loc2;
        }
        else if (loc1 > loc2)
        {
            min = loc2;
            max = loc1;
            GameObject corner1 = Instantiate(cornerLoops[3 + addition], new Vector3(loc1 * scale, 0, 0), Quaternion.Euler(0, 0, 0), this.transform);
            corner1.name = "corner" + loc1;
            GameObject corner2 = Instantiate(cornerLoops[1 + addition], new Vector3(loc2 * scale, 0, 0), Quaternion.Euler(0, 0, 0), this.transform);
            corner2.name = "corner" + loc2;
        }
        else if (loc1 == loc2)
        {
            GameObject hor = Instantiate(straightLoops[1 + addition], new Vector3(loc1 * scale, 0, 0), Quaternion.Euler(0, 0, 0), this.transform);
            hor.name = "hor" + loc1;
        }

        if (min != max)
        {
            for (int index = min + 1; index < max; index ++)
            {
                GameObject hor = Instantiate(straightLoops[0 + addition], new Vector3(index * scale, 0, 0), Quaternion.Euler(0, 0, 0), this.transform);
                hor.name = "hor" + index;
                groundLocations.Add(new MapLocation(index, 0));
            }
        }
    }

    public void AnalyzeVerticalSpaces(int loc1, int loc2, int addition)
    {
        int min = loc1;
        int max = loc2;
        groundLocations.Add(new MapLocation(0, loc1));
        if (loc1 != loc2)
        {
            groundLocations.Add(new MapLocation(0, loc2));
        }
        if (loc1 < loc2)
        {
            GameObject corner1 = Instantiate(cornerLoops[3 + addition], new Vector3(0, 0, loc1 * scale), Quaternion.Euler(0, 0, 0), this.transform);
            corner1.name = "corner" + loc1;
            GameObject corner2 = Instantiate(cornerLoops[1 + addition], new Vector3(0, 0, loc2 * scale), Quaternion.Euler(0, 0, 0), this.transform);
            corner2.name = "corner" + loc2;
        }
        else if (loc1 > loc2)
        {
            min = loc2;
            max = loc1;
            GameObject corner1 = Instantiate(cornerLoops[2 + addition], new Vector3(0, 0, loc1 * scale), Quaternion.Euler(0, 0, 0), this.transform);
            corner1.name = "corner" + loc1;
            GameObject corner2 = Instantiate(cornerLoops[0 + addition], new Vector3(0, 0, loc2 * scale), Quaternion.Euler(0, 0, 0), this.transform);
            corner2.name = "corner" + loc2;
        }
        else if (loc1 == loc2)
        {
            GameObject vert = Instantiate(straightLoops[0 + addition], new Vector3(0, 0, loc1 * scale), Quaternion.Euler(0, 0, 0), this.transform);
            vert.name = "vert" + loc1;
        }

        if (min != max)
        {
            for (int index = min + 1; index < max; index++)
            {
                GameObject vert = Instantiate(straightLoops[1 + addition], new Vector3(0, 0, index * scale), Quaternion.Euler(0, 0, 0), this.transform);
                vert.name = "vert" + index;
                groundLocations.Add(new MapLocation(0, index));
            }
        }
    }

    public void FillInHoles()
    {
        for (int index = -width; index <= width; index++)
        {
            if (!groundLocations.Contains(new MapLocation(index, 0)))
            {
                GameObject hWall = Instantiate(wall, new Vector3(index * scale, 0, 0), Quaternion.Euler(0, 90, 0), this.transform);
                hWall.name = "hWall" + index;
                groundLocations.Add(new MapLocation(index, 0));
            }
        }

        for (int index = -depth; index <= depth; index++)
        {
            if (!groundLocations.Contains(new MapLocation(0, index)))
            {
                GameObject vWall = Instantiate(wall, new Vector3(0, 0, index * scale), Quaternion.Euler(0, 90, 0), this.transform);
                vWall.name = "vWall" + index;
                groundLocations.Add(new MapLocation(0, index));
            }
        }
    }

    public void updatePrefabs(Maze maze, int rotation)
    {
        maze.cornerLeftDown = new Maze.Module();
        maze.cornerLeftDown.prefab = cornerLoops[rotation + 2];
        maze.cornerLeftDown.rotation = new Vector3(0, -rotation * 90, 0);

        maze.cornerLeftUp = new Maze.Module();
        maze.cornerLeftUp.prefab = cornerLoops[rotation + 3];
        maze.cornerLeftUp.rotation = new Vector3(0, -rotation * 90, 0);

        maze.cornerRightUp = new Maze.Module();
        maze.cornerRightUp.prefab = cornerLoops[rotation];
        maze.cornerRightUp.rotation = new Vector3(0, -rotation * 90, 0);

        maze.cornerRightDown = new Maze.Module();
        maze.cornerRightDown.prefab = cornerLoops[rotation + 1];
        maze.cornerRightDown.rotation = new Vector3(0, -rotation * 90, 0);

        maze.straightVertical = new Maze.Module();
        maze.straightVertical.prefab = straightLoops[rotation + 1];
        maze.straightVertical.rotation = new Vector3(0, -rotation * 90, 0);

        maze.straightHorizontal = new Maze.Module();
        maze.straightHorizontal.prefab = straightLoops[rotation];
        maze.straightHorizontal.rotation = new Vector3(0, -rotation * 90, 0);

        maze.wall.prefab = wall;
        //maze.wall.rotation = new Vector3(0, -rotation * 90, 0);
    }
}
