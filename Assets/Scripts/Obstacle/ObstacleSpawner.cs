using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public static Transform player;
    [SerializeField] private SpawnProperties spawnProperties;
    [SerializeField] private Transform _player;

    void Awake()
    {
        player = _player;
    }


    void Update()
    {
        if (PlayerCollision.Destroyed)
            return;

        foreach (SpawnPropertiesObject spawn in spawnProperties.spawnProperties)
        {
            if (spawn.CountDown(Timer.Score))
            {
                SpawnObject(spawn.Spawn, spawn);
            }
        }
    }

    private void SpawnObject(GameObject obj, SpawnPropertiesObject prop)
    {
        float spawnForward = spawnProperties.AverageSpawnDistance + spawnProperties.RandomSpawnDistance * Random.Range(-1f, 1f);
        float xPosition = prop.XRandom ? Random.Range(-100f, -15f) * (1 - 2 * Random.Range(0,2)) : Random.Range(-10f, 10f);

        Vector3 spawnLocation = new Vector3(xPosition, player.position.y, player.position.z + spawnForward);

        Instantiate(obj, spawnLocation + prop.ExtraSpawnTranslate, Quaternion.identity, this.transform);
    }
}