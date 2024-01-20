using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class PlaneMovement : MonoBehaviour
{
    private const int REVERSE_MOVEMENT = -1;
    private const int ROTATION = 180;

    [SerializeField] private int speed;

    [SerializeField] private int turnAngle;
    [SerializeField] private int maxTotalTurnAngle;
    [SerializeField] private int minTotalTurnAngle;
    [SerializeField] private float maxHorizontalDistance;
    [SerializeField] private float xShift = 500f;
    [SerializeField] private float height = 15f;

    [SerializeField] private int tiltAngle;
    [SerializeField] private SpawnManager spawnManager;

    [SerializeField] private AudioSource mainTheme;
    [SerializeField] private Camera cam;

    private float speedShieldTimer = 0f;
    private static float speedShieldSpeed = 1f;
    public static float SpeedShieldSpeed { get { return speedShieldSpeed; }}

    public static void Die() {
        // Wait for die animation to happen first
        // Then send to game over menu
        Timer.PauseTimer();
        if(PlayerPrefs.GetInt("BestScore") < (int)Mathf.Floor(Timer.finalScore)) {
            PlayerPrefs.SetInt("BestScore", (int)Mathf.Floor(Timer.finalScore));
        }
        
        SceneManager.LoadScene("EndMenu");
        //
    }

    private void Update()
    {

        SpeedShieldUpdate();

        float totalTurnAngle;
        
        if (Input.GetKey(KeyCode.LeftArrow) 
        || (transform.position.x - xShift > maxHorizontalDistance && this.transform.eulerAngles.y > ROTATION))
        {
            totalTurnAngle = this.transform.eulerAngles.y + (turnAngle * Time.deltaTime * REVERSE_MOVEMENT);

            if (totalTurnAngle > minTotalTurnAngle)
            {
                this.transform.eulerAngles += new Vector3(0, turnAngle * Time.deltaTime * REVERSE_MOVEMENT, tiltAngle * Time.deltaTime * REVERSE_MOVEMENT);
            }
        }
        
        else if (Input.GetKey(KeyCode.RightArrow) 
        || (transform.position.x - xShift < -maxHorizontalDistance && this.transform.eulerAngles.y < ROTATION))
        {
            totalTurnAngle = this.transform.eulerAngles.y + (turnAngle * Time.deltaTime);

            if (totalTurnAngle < maxTotalTurnAngle)
            {
                this.transform.eulerAngles += new Vector3(0, turnAngle * Time.deltaTime, tiltAngle * Time.deltaTime);
            }
        }
        else {
            totalTurnAngle = this.transform.eulerAngles.y;

            if (totalTurnAngle > 181) {
                this.transform.eulerAngles += new Vector3(0, turnAngle * Time.deltaTime * REVERSE_MOVEMENT, tiltAngle * Time.deltaTime * REVERSE_MOVEMENT);
            } else if (totalTurnAngle < 179) {
                this.transform.eulerAngles += new Vector3(0, turnAngle * Time.deltaTime, tiltAngle * Time.deltaTime);
            } else {
                this.transform.eulerAngles = new Vector3(0, 180, 0);
            }
            Quaternion target = Quaternion.Euler(0, 180, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 5);
            
        }

        Vector3 movementDirection = transform.forward;

        if (Mathf.Abs(transform.position.x - xShift) >= maxHorizontalDistance && (transform.position.x-xShift) * (transform.eulerAngles.y-ROTATION) > 0)
            movementDirection = Vector3.back;

        transform.position += movementDirection * speed * speedShieldSpeed * Time.deltaTime * REVERSE_MOVEMENT;

        transform.position = new Vector3(transform.position.x, height, transform.position.z);

        
    }

    void SpeedShieldUpdate()
    {
        speedShieldTimer -= Time.deltaTime;

        if (speedShieldTimer > 0)
        {
            if (speedShieldTimer > 8f)
            {
                speedShieldSpeed += 2f * Time.deltaTime;
                mainTheme.pitch += (Mathf.Sqrt(2) - 1) / 2f * Time.deltaTime;
                cam.fieldOfView += 5f * Time.deltaTime;
            }
            if (speedShieldTimer < 2f)
            {
                speedShieldSpeed -= 2f * Time.deltaTime;
                mainTheme.pitch -= (Mathf.Sqrt(2) - 1) / 2f * Time.deltaTime;
                cam.fieldOfView -= 5f * Time.deltaTime;
            }
        }
        else
        {
            speedShieldSpeed = 1f;
            mainTheme.pitch = 1f;
            cam.fieldOfView = 60f;
        }
    }

    public void SpeedShield(float time)
    {
        speedShieldTimer = time;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpawnTrigger"))
        {
            spawnManager.SpawnTriggerEntered();
        }
    }



    public static void lightsOn() {
        Transform player = GameObject.Find("Players").transform.Find(Menu.plane.ToString());
        if (player != null) player.Find("Spot Light").gameObject.GetComponent<Light>().intensity = 10.0f;
    }

    public static void lightsOff() {
        Transform player = GameObject.Find("Players").transform.Find(Menu.plane.ToString());

        if (player != null)
            player.Find("Spot Light").gameObject.GetComponent<Light>().intensity = 0.0f;
        if (player != null && player.Find("Spot Light (1)"))
            player.Find("Spot Light (1)").gameObject.GetComponent<Light>().intensity = 0.0f;
        if (player != null && player.Find("Spot Light (2)")) 
            player.Find("Spot Light (2)").gameObject.GetComponent<Light>().intensity = 0.0f;
    }

    public void setSpeed(int speed)
    {
        this.speed = speed;
    }
 
}
