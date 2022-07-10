using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
{
    [Header("UFO Paremetrs")]
    float speed;
    int direction;

    [Header("Fire Paramters")]
    public float bulletSpeed;
    public Color bulletColor;
    public GameObject bulletPrefab;
    public float minTimeToShot;
    public float maxTimeToShot;
    float timeToNextShot;
    float timeAfterShot;

    

    //Containers
    GameObject bulletsContainer;

    //Cashe
    Camera mainCam;
    GameControler gc;
    GameObject starShip;


    void Start()
    {
        timeToNextShot = Random.Range(minTimeToShot, maxTimeToShot);
        mainCam = Camera.main;
        gc = mainCam.GetComponent<GameControler>();
        starShip = gc.starShip;
        bulletsContainer = mainCam.GetComponent<SpawnControler>().bulletsContainer;
    }

    
    void Update()
    {
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        timeAfterShot += Time.deltaTime;


        if (timeAfterShot >= timeToNextShot)
        {
            Shot();

            timeAfterShot = 0;
            timeToNextShot = Random.Range(minTimeToShot, maxTimeToShot); 
        }
    }

    void Shot()
    {
        if (starShip)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(LookAt(starShip.transform.position)), bulletsContainer.transform);
            bullet.GetComponent<Bullet>().BulletInitialize(bulletColor, bulletSpeed, mainCam.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x);
        }
    }

    Vector3 LookAt(Vector2 point)
    {
        point.x = point.x - transform.position.x;
        point.y = point.y - transform.position.y;


        float rot_z = Mathf.Atan2(point.y, point.x) * Mathf.Rad2Deg;

        return new Vector3(0, 0, rot_z - 90);
    }

    public void Initialize(int direction, float speed)
    {
        this.direction = direction;
        this.speed = speed;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Bullet"))
        {
            Destroy(col.gameObject);

            mainCam.GetComponent<GameControler>().UFODestroed();
            Destroy(gameObject);
        }

        if(col.CompareTag("Asteroid"))
        {
            col.GetComponent<Asteroid>().CollisionWithShip();
            Destroy(gameObject);
        }
    }
}