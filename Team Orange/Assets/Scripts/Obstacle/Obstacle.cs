using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Transform player;
    private static float THRESHOLD = 300f;

    // Start is called before the first frame update
    void Awake()
    {
        player = ObstacleSpawner.player;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerCollision.Destroyed)
            return;

        if (player.position.z - transform.position.z > THRESHOLD)
            {
                Destroy(gameObject);
            }
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag.Equals("Wall"))
            Destroy(gameObject);
    }
}
