using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float bobbingPeriod;
    [SerializeField] private float bobbingRange;
    [SerializeField] private Mesh[] meshes;

    private float timer = 0;
    private float initialPosition;
    private Vector3 direction;

    void Start()
    {
        MeshFilter filter = GetComponent<MeshFilter>();
        filter.mesh = meshes[Random.Range(0,meshes.Length)];

        initialPosition = transform.position.y;
        transform.eulerAngles = new Vector3(0, Random.Range(0, 360f), 0);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        float bobbingPosition = Mathf.Sin(6.2832f / bobbingPeriod * timer) * bobbingRange;
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, bobbingPosition + initialPosition, transform.position.z);
    }
}
