using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public Maze[] mazes;
    public int width = 30;
    public int depth = 30;

    void Start()
    {
        int level = 0;
        foreach(Maze m in mazes)
        {
            m.width = width;
            m.depth = depth;
            //m.level = level++;
            m.Build();
        }       
    }
}
