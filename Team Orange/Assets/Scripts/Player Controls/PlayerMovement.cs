using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const int REVERSE = -1;

    [SerializeField] private float speed;

    [SerializeField] private float turnSpeed;
    [SerializeField] private float tiltSpeed;
    [SerializeField] private float maxTurnAngle;
    [SerializeField] private float minTurnAngle;

    [SerializeField] private float straight;

    [SerializeField] private float offset;

    [SerializeField] SpawnManager spawnManager;

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (transform.eulerAngles.y - turnSpeed * Time.deltaTime > minTurnAngle)
            {
                transform.eulerAngles -= new Vector3(tiltSpeed * Time.deltaTime, turnSpeed * Time.deltaTime, 0);
            }
        }

        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (transform.eulerAngles.y + turnSpeed * Time.deltaTime < maxTurnAngle)
            {
                transform.eulerAngles += new Vector3(tiltSpeed * Time.deltaTime, turnSpeed * Time.deltaTime, 0);
            }
        }

        else if (transform.eulerAngles.y < straight - offset)
        {
            this.transform.eulerAngles += new Vector3(tiltSpeed * Time.deltaTime, turnSpeed * Time.deltaTime, 0);
        }

        else if (transform.eulerAngles.y > straight + offset)
        {
            this.transform.eulerAngles -= new Vector3(tiltSpeed * Time.deltaTime, turnSpeed * Time.deltaTime, 0);
        }

        transform.position += transform.right * speed * Time.deltaTime * REVERSE;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpawnTrigger"))
        {
            spawnManager.SpawnTriggerEntered();
        }
    }
}
