using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float speed;
    [SerializeField] private Vector3 forwardRotation;
    private Vector3 direction;

    void Awake()
    {
        transform.eulerAngles = forwardRotation;
        SetDirection(Random.Range(0, 2) < 1);

        // Spawn on both sides of road
        if (Random.Range(0f, 1f) > 0.5) {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(-transform.position.x, 0, transform.position.z);
        }
    }

    public void SetDirection(bool forward)
    {
        direction = forward ? Vector3.forward : Vector3.back;
        offset = new Vector3(Random.Range(50,offset.x), offset.y, offset.z);
        transform.position += forward ? offset : -offset;
        if (!forward) transform.eulerAngles += new Vector3(0f, 180f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("SpawnTrigger"))
        {
            GameObject terrain = other.gameObject.transform.parent.gameObject;
            if (!terrain.tag.Equals("Water"))
                Destroy(gameObject);
        }
    }
}
