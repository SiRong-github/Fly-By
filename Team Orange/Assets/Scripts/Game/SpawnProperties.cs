using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnProperties
{
    [Header("Spawn Distance")]
    [SerializeField] private float averageSpawnDistance = 600f;
    [SerializeField] private float randomSpawnDistance = 200f;

    [Header("Obstacle Spawn Rates")]
    public SpawnPropertiesObject[] spawnProperties;

    public float AverageSpawnDistance { get { return averageSpawnDistance; } }
    public float RandomSpawnDistance { get { return randomSpawnDistance; } }
}

[System.Serializable]
public class SpawnPropertiesObject
{
    [SerializeField] private GameObject spawn;
    public SpawnPeriod[] spawnPeriods;
    [SerializeField] private float countDownSize = 100;
    [SerializeField] private float randomFactor = 1;
    [SerializeField] private bool xRandom = true;
    [SerializeField] private Vector3 extraSpawnTranslate = Vector3.zero;

    private float countDown = -100;

    public GameObject Spawn { get { return spawn; } }

    public float SpawnRate(float score)
    {
        float spawnRate = 0;

        foreach (SpawnPeriod period in spawnPeriods)
        {
            if (score >= period.PeriodStart && (score < period.PeriodEnd || period.PeriodEnd == -1))
                spawnRate += period.SpawnRate;
        }

        return Mathf.Max(spawnRate, 0);
    }

    public bool CountDown(float score)
    {
        if (countDown == -100)
        {
            countDown = Random.Range(1f, randomFactor) * countDownSize;
            return false;
        }

        float countDownRate = SpawnRate(score) * Time.deltaTime * PlaneMovement.SpeedShieldSpeed;
        countDown -= countDownRate;

        if (countDown <= 0)
        {
            countDown = Random.Range(1f, randomFactor) * countDownSize;
            return true;
        }

        return false;
    }

    public bool XRandom { get { return xRandom; } }
    public Vector3 ExtraSpawnTranslate { get { return extraSpawnTranslate; }}
}

[System.Serializable]
public class SpawnPeriod
{
    // The period in which the object spawns at this rate
    [SerializeField] private float periodStart;
    [SerializeField] private float periodEnd; // Set to -1 if indefinite
    
    // The rate at which the object spawns
    [SerializeField] private float spawnRate;

    public float PeriodStart { get { return periodStart; }}
    public float PeriodEnd { get { return periodEnd; }}
    public float SpawnRate { get { return spawnRate; }}
}