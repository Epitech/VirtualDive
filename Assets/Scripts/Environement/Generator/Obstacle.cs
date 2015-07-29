using UnityEngine;
using System.Collections;

[System.Serializable]
public struct AngleTable
{
    public int min;
    public int max;
}

[System.Serializable]
public class Obstacle
{
    public GameObject prefab;

    public AngleTable[] spawnAngles;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject Generate()
    {

        return (null);
    }
}
