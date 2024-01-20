using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform targetFront;
    [SerializeField] private Transform targetBack;

    [SerializeField] private float followSpeed;
    [SerializeField] private Vector3 offset;

    public void Start() {
        Transform players = GameObject.Find("Players").transform;
        foreach(Transform player in players) {
            player.gameObject.SetActive(false);
        }
        Transform selectedPlane = players.Find(Menu.plane.ToString());
        selectedPlane.gameObject.SetActive(true);
        ObstacleSpawner.player = selectedPlane;

        targetFront = selectedPlane;
        targetBack = selectedPlane;
    }
    private void LateUpdate()
    {
        if (PlayerCollision.Destroyed)
            return;

        Vector3 desiredPosition = targetBack.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, followSpeed);
        transform.position = smoothedPosition;
        transform.LookAt(targetFront.position);
    }
}