using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

    public float flyawayAngle;
    AsteroidSize size;
    Vector2 direction;
    float speed;

    SpawnControler spCtrl;


    public enum AsteroidSize
    {
        Small = 0,
        Middle = 1,
        Big = 2
    }
    
    void Start()
    {

    }

    
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log(Physics2D.IsTouching(spCtrl.GetComponent<BoxCollider2D>(), GetComponent<PolygonCollider2D>()));
        }
    }


    public void Initialize(AsteroidSize size, float speed, Vector2 direction, SpawnControler spCtrl)
    {
        this.size = size;
        this.speed = speed;
        this.direction = direction;
        this.spCtrl = spCtrl;

        switch (size)
        {
            case AsteroidSize.Big:
                transform.localScale = new Vector3(1, 1, 1);
                break;
            case AsteroidSize.Middle:
                transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                break;
            case AsteroidSize.Small:
                transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                break;
        }
    }

    public void AsteroidIsDestroed()
    {
        if (size != AsteroidSize.Small)
        {
            float angle = Mathf.Asin(direction.x / direction.magnitude) * Mathf.Rad2Deg;
            Vector2 dir_1 = new Vector2(Mathf.Sin((angle + flyawayAngle) * Mathf.Deg2Rad), Mathf.Sign(direction.y) * Mathf.Cos((angle + flyawayAngle) * Mathf.Deg2Rad));
            Vector2 dir_2 = new Vector2(Mathf.Sin((angle - flyawayAngle) * Mathf.Deg2Rad), Mathf.Sign(direction.y) * Mathf.Cos((angle - flyawayAngle) * Mathf.Deg2Rad));

            spCtrl.SpawnAsteroid(size - 1, dir_1, transform.position);
            spCtrl.SpawnAsteroid(size - 1, dir_2, transform.position);
        }

        spCtrl.AsteroidIsDestroy();

        Destroy(gameObject);
    }


    public void CollisionWithShip()
    {
        spCtrl.AsteroidIsDestroy();

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Bullet"))
        {
            AsteroidIsDestroed();

            spCtrl.GetComponent<GameControler>().AsteroidDestroed(size);

            Destroy(col.gameObject);
        }

        /*if(col.CompareTag("UFO Bullet"))
        {
            AsteroidIsDestroed();
        }*/
    }
}