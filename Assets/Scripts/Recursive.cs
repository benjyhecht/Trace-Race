using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recursive : Maze
{
    public override void Generate()
    {
        Generate(5, 5);
    }

    void Generate(int x, int z)
    {
        if (CountSquareNeighbours(x, z) >= 2)
        {
            return;
        }
        map[x, z] = 0;

        List<MapLocation> directions = new List<MapLocation>() { new MapLocation(1, 0), new MapLocation(-1, 0), new MapLocation(0, 1), new MapLocation(0, -1) };
        while (directions.Count > 0)
        {
            int rDirection = Random.Range(0, directions.Count);
            Generate(x + directions[rDirection].x, z + directions[rDirection].z);
            directions.RemoveAt(rDirection);
        }
    }
}
