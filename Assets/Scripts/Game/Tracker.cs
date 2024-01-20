using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    [SerializeField] private float speed = 20f;

    private Transform player;

    void Awake()
    {
        player = ObstacleSpawner.player;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerCollision.Destroyed)
            return;

        Vector3 direction = (player.position - this.transform.position).normalized;
        this.transform.position += direction * speed * Time.deltaTime;

        transform.LookAt(player, Vector3.up);

        if (this.transform.position.z + 5 < player.transform.position.z)
            Destroy(this.gameObject);
    }
}
