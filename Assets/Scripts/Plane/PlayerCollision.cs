using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private ParticleSystem planeExplosionSystem;
    //[SerializeField] private ParticleSystem obstacleExplosionSystem;

    [SerializeField] private Timer timer;
    [SerializeField] private CameraMovement cameraMovement;
    [SerializeField] private GameObject shieldPrefab;
    [SerializeField] private GameObject speedShieldPrefab;
    //[SerializeField] private GameObject bulletPrefab;

    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource planeExplosionSFX;
    //[SerializeField] private AudioSource obstacleExplosionSFX;
    //[SerializeField] private AudioSource shootSFX;

    private GameObject shieldInstance;
    private static bool destroyed = false;
    private static bool planeExploded = false;
    //private static bool obstacleExploded = false;

    private bool weakShieldActive = false;
    //private bool bulletActive = false;
    private PlaneMovement playerMovement;
    private SpawnManager spawnManager;
    private ParticleSystem planeExplosion;
    //private GameObject bullet;

    public static bool Destroyed {
        get { return destroyed; }
    }

    private float weakShieldTimer = 0;
    private float immunityTimer = 0;
    private float explodedTimer = 0;
    //private float bulletTimer = 0;

    void Awake()
    {
        destroyed = false;
        planeExploded = false;
        playerMovement = GetComponent<PlaneMovement>();
        
        //spawnManager = GameObject.FindGameObjectsWithTag("SpawnManager")[0].GetComponent<SpawnManager>();
    }

    void Update()
    {

        if (planeExploded)
        {
            if (explodedTimer < 0)
            {
                StartCoroutine(LoadMainMenu());
                Destroy(this.gameObject);
                PlaneMovement.Die();
            }
            else
                explodedTimer -= Time.deltaTime;

        }

        if (weakShieldActive)
        {
            if (weakShieldTimer < 0)
            {
                DestroyWeakShield();
                weakShieldTimer = 0;
            }
            else
                weakShieldTimer -= Time.deltaTime;
        }

        immunityTimer -= Time.deltaTime;
        if (immunityTimer < 0 && shieldInstance != null && weakShieldTimer <= 0)
        {
            Destroy(shieldInstance);
        }

        //shoot bullet
        //if (bulletActive)
        //{
        //    if (bulletTimer < 0)
        //        bulletActive = false;
        //    else
        //    {
        //        if (Input.GetKeyDown(KeyCode.X))
        //        {
        //            bulletTimer -= Time.deltaTime;
        //            //bullet.Emit(1);
        //        }
        //    }

        //}

    }

    private void OnTriggerEnter(Collider other)
    {
        //var obstacleExplosion = Instantiate(obstacleExplosionSystem);

        if (immunityTimer > 0)
            return;

        if (other.tag.Equals("Powerup"))
        {
            Powerup p = other.gameObject.GetComponent<Powerup>();
            Debug.Log(p.Name);
            Debug.Log(p.Name.Equals("Shield"));
            switch (p.Name)
            {
                case "Shield":
                    ActivateWeakShield();
                    break;
                case "SpeedShield":
                    DestroyWeakShield();
                    immunityTimer = 10f;
                    shieldInstance = Instantiate(speedShieldPrefab, transform.position, Quaternion.identity, transform);
                    playerMovement.SpeedShield(10f);
                    break;
                case "Bullet":

                    break;
                default:
                    break;
            }

            Destroy(other.gameObject);

            //var obstacleExplosion = Instantiate(obstacleExplosionParticle);
            //obstacleExplosionParticle.transform.position = other.transform.position;
            //obstacleExplosion.Play();
            //if (obstacleExplosion.isStopped)
            //{
            //    Destroy(other.gameObject);
            //}

        }
        else if (other.tag.Equals("End"))
        {
            SceneManager.LoadScene("StartScene");
        }
        else if (other.tag.Equals("SpawnTrigger"))
        {
            //spawnManager.SpawnTriggerEntered();
        }
        else if (!(other.tag.Equals("PassThrough")))
        {
            if (weakShieldActive && !other.tag.Equals("Wall"))
                DestroyWeakShield();
            else
            {
                if (!planeExploded)
                {
                    planeExplosion = Instantiate(planeExplosionSystem);
                    planeExplosion.transform.position = this.gameObject.transform.position;
                    planeExplosion.Play();
                    music.volume = 0f;
                    planeExplosionSFX.Play();
                    playerMovement.setSpeed(0);
                    planeExploded = true;
                    destroyed = true;
                    explodedTimer = planeExplosion.main.startLifetime.constantMax * Time.deltaTime;
                }
            }
        }
    }

    IEnumerator LoadMainMenu()
    {
        destroyed = true;
        MeshRenderer[] render = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer r in render)
            r.enabled = false;

        yield return new WaitForSeconds(3);
        
        //SceneManager.LoadScene("EndMenu");
    } 

    private void ActivateWeakShield()
    {
        if (!weakShieldActive)
            shieldInstance = Instantiate(shieldPrefab, transform.position, Quaternion.identity, transform);
        weakShieldTimer = 20f;
        weakShieldActive = true;
    }

    private void DestroyWeakShield()
    {
        weakShieldActive = false;
        weakShieldTimer = 0;
        Destroy(shieldInstance);
        immunityTimer = 1;
    }

    //private void DestroyWeakShield()
    //{
    //    weakShieldActive = false;
    //    weakShieldTimer = 0;
    //    Destroy(shieldInstance);
    //    immunityTimer = 1;
    //}

}
