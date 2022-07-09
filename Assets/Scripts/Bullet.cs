using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed;

    float distance;
    float maxDistance;
    float deltaDistance;

    

    void Update()
    {
        deltaDistance = speed * Time.deltaTime;
        transform.Translate(Vector2.up * deltaDistance);
        distance += deltaDistance;

        if (distance > maxDistance)
            Destroy(gameObject);
    }

    public void BulletInitialize(Color32 color, float speed, float maxDistance)
    {
        this.speed = speed;
        this.maxDistance = maxDistance;
        GetComponent<SpriteRenderer>().color = color;
    }


    /*private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Asteroid"))
        {
            Destroy(gameObject);
        }
    }*/
}