using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarShip : MonoBehaviour
{
    [Header("StarShip Parametrs")]
    public float maxSpeed;
    Vector3 speed;
    public float acceleration;
    public float turnSpeed;

    
    int health;
    bool immunety;
    [Space]
    public float immunetyTime;
    float immunetyPastTime;


    [Header("Fire Parametrs")]
    public int rateOfFire;
    public float bulletSpeed;
    public Color bulletColor;
    public GameObject bulletPrefab;
    GameObject shotPoint;
    bool shot;
    float timeAfterShot;

    [Header("UI Components")]
    public GameObject[] healthImages;

    [Header("Containers")]
    public GameObject bulletsContainer;
        

    bool keyboardControl = true;


    SpriteRenderer sr;
    
    GameControler gc;
    Camera mainCam;


    void Start()
    {
        health = 3;

        shotPoint = transform.GetChild(0).gameObject;
        shot = true;

        sr = GetComponent<SpriteRenderer>();

        mainCam = Camera.main;
        gc = mainCam.GetComponent<GameControler>();
    }

    
    void Update()
    {
        if (keyboardControl)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                speed = new Vector3(Mathf.Clamp(speed.x + acceleration * Time.deltaTime * transform.up.x, -maxSpeed, maxSpeed),
                                     Mathf.Clamp(speed.y + acceleration * Time.deltaTime * transform.up.y, -maxSpeed, maxSpeed), 0);
            }

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                transform.Rotate(0, 0, turnSpeed * Time.deltaTime);

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                transform.Rotate(0, 0, -turnSpeed * Time.deltaTime);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, Mathf.MoveTowardsAngle(transform.eulerAngles.z, LookAt(mainCam.ScreenToWorldPoint(Input.mousePosition)).z - 90, turnSpeed * Time.deltaTime));

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetMouseButton(1))
            {
                speed = new Vector3(Mathf.Clamp(speed.x + acceleration * Time.deltaTime * transform.up.x, -maxSpeed, maxSpeed),
                                     Mathf.Clamp(speed.y + acceleration * Time.deltaTime * transform.up.y, -maxSpeed, maxSpeed), 0);
            }
        }


        if (!shot)
        {
            timeAfterShot += Time.deltaTime;

            if (timeAfterShot >= 1f / rateOfFire)
            {
                shot = true;
                timeAfterShot = 0;
            }
        }

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) && !keyboardControl) && shot )
        {
            Shot();
            shot = false;
        }


        if (immunety)
            if (immunetyPastTime < immunetyTime)
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Mathf.PingPong(immunetyPastTime*4, 1));
                immunetyPastTime += Time.deltaTime;
            }
            else
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1);
                immunety = false;
            }


        transform.position += speed * Time.deltaTime;
    }


    Vector3 LookAt(Vector2 point)
    {
        point.x = point.x - transform.position.x;
        point.y = point.y - transform.position.y;


        float rot_z = Mathf.Atan2(point.y, point.x) * Mathf.Rad2Deg;

        return new Vector3(0, 0, rot_z);
    }


    void Shot()
    {
        GameObject bullet = Instantiate(bulletPrefab, shotPoint.transform.position, transform.rotation, bulletsContainer.transform);
        bullet.GetComponent<Bullet>().BulletInitialize(bulletColor, bulletSpeed, mainCam.ScreenToWorldPoint(new Vector3(Screen.width,0,0)).x);
    }

    void Damage()
    {
        health--;
        immunety = true;
        immunetyPastTime = 0;

        transform.position = new Vector3();
        transform.eulerAngles = new Vector3();
        speed = new Vector3();

        healthImages[health].SetActive(false);
        if (health == 0)
        {
            gc.GameOver();
            gameObject.SetActive(false);
        }
    }

    public void ChangeControl(Text buttonText)
    {
        keyboardControl = !keyboardControl;


        if (keyboardControl)
            buttonText.text = "”правление: клавиатура";
        else
            buttonText.text = "”правление: клавиатура + мышь";
    }


    public void ResumeGame()
    {
        if (keyboardControl)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.Confined;
    }
    

    public void NewGame()
    {
        gameObject.SetActive(true);

        transform.position = new Vector3();
        transform.eulerAngles = new Vector3();
        speed = new Vector3();

        health = 3;
        for (int i = 0; i< 3; i++)
        {
            healthImages[i].SetActive(true);
        }
        immunety = false;
    }



    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!immunety && col.CompareTag("Asteroid"))
        {
            col.GetComponent<Asteroid>().CollisionWithShip();

            Damage();
        }

        if (!immunety && col.CompareTag("UFO Bullet"))
        {
            Damage();
            Destroy(col.gameObject);
        }

        if (!immunety && col.CompareTag("UFO"))
        {
            Damage();
            Destroy(col.gameObject);
        }
    }
}