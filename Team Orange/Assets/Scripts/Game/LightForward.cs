using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightForward : MonoBehaviour
{
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find(Menu.plane.ToString()).transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, player.position + new Vector3(0, 0, 5), 20);
        Debug.Log(transform.position);
    }
}
