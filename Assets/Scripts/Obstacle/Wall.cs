using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    void Awake()
    {
        transform.position = new Vector3(Random.Range(0,2) < 1 ? 75 : -75, transform.position.y, transform.position.z);
        transform.eulerAngles = Random.Range(0,5) < 4 ? Vector3.zero : new Vector3(0, 90, 0);
    }
}
