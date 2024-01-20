using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truck : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float speed;
    [SerializeField] private Vector3 forwardRotation;
    private Vector3 direction;

    void Awake()
    {
        transform.eulerAngles = forwardRotation;
        SetDirection(Random.Range(0,2) < 1);
        
        // Make sure the car and bus are on the road
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    public void SetDirection(bool forward)
    {
        direction = forward ? Vector3.forward : Vector3.back;
        transform.position += forward ? offset : -offset;
        if (!forward) transform.eulerAngles += new Vector3(0f, 180f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag.Equals("Water"))
            Destroy(gameObject);
    }
}
