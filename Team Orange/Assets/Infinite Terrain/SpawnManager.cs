using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public RoadSpawner1 roadSpawner;

    void Start()
    {
        roadSpawner = GetComponent<RoadSpawner1>();
    }

    public void SpawnTriggerEntered()
    {
        roadSpawner.MoveTile();
    }
}
