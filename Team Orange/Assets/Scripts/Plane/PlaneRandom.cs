using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneRandom : MonoBehaviour
{
    [SerializeField] private float speed;

    void Awake()
    {
        float randomRotation = Random.Range(0f, 90f);
        float yRotation = Random.Range(0, 2) < 1 ? randomRotation : 360f - randomRotation;

        transform.eulerAngles = new Vector3(0, yRotation, 0);

        transform.position -= transform.forward * speed * -12;
    }

    // To prevent planes having wild motion when they collide with other obstacles
    void OnCollisionEnter()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= transform.forward * speed * Time.deltaTime;
    }
}
