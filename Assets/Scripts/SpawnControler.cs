using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnControler : MonoBehaviour
{
    [Header("Spwan Parametrs")]
    public int minAstSpeed;
    public int maxAstSpeed;
    [Space]
    public int countAstSpawnOnStart;
    [Space]
    public float maxUFOSpawnTime;
    public float minUFOSpawnTime;
    float nextUFOSpawnTime;
    float timeAfterUFOSpawn;

    [Header("Prefabs")]
    public GameObject asteroidPref;
    public GameObject UFOPrefab;

    [Header("Containers")]
    public GameObject asteroidsContainer;
    public GameObject UFOContainer;
    public GameObject bulletsContainer;

    //Parametrs
    int curLevel;
    bool isLevelEnd;
    int countOfAsteroidsOnLevel;
    int countOfAsteroidsDestroed;

    
    BoxCollider2D bc2D;
    


    void Start()
    {
        bc2D = GetComponent<BoxCollider2D>();
        bc2D.size = GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width, Screen.height)) * 2;
    }


    void Update()
    {
        if (isLevelEnd)
        {
            for (int i = 0; i < curLevel + countAstSpawnOnStart; i++)
                SpawnAsteroid();

            isLevelEnd = false;
        }

        //UFO
        timeAfterUFOSpawn += Time.deltaTime;        

        if(timeAfterUFOSpawn >= nextUFOSpawnTime)
        {
            SpawnUFO();

            timeAfterUFOSpawn = 0;
            nextUFOSpawnTime = Random.Range(minUFOSpawnTime, maxUFOSpawnTime);
        }
    }

    void SpawnAsteroid()
    {
        
        Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

        float rand_sign = Mathf.Sign(Random.Range(-1f, 1f));
        Vector2 spawnPosition = new Vector2(rand_sign == 1 ? -direction.x * (bc2D.size.x / 2 -1) : -Mathf.Sign(direction.x) * Random.Range(bc2D.size.x / 2 - 0.5f, bc2D.size.x / 2 + 0.5f),
                                           rand_sign == -1 ? -direction.y * (bc2D.size.y / 2 -1) : -Mathf.Sign(direction.y) * Random.Range(bc2D.size.y / 2 - 0.5f, bc2D.size.y / 2 + 0.5f));

        SpawnAsteroid((Asteroid.AsteroidSize)2, direction.normalized, spawnPosition);
    }


    public void SpawnAsteroid(Asteroid.AsteroidSize size, Vector2 direction, Vector2 spawnPosition)
    {
        GameObject ast = Instantiate(asteroidPref, spawnPosition, Quaternion.identity, asteroidsContainer.transform);


        if (Mathf.Abs(ast.transform.position.x) > bc2D.size.x / 2)
        {
            ast.transform.position = new Vector3(-Mathf.Sign(direction.x) * Mathf.Abs(ast.transform.position.x), ast.transform.position.y, ast.transform.position.z);
        }
        if (Mathf.Abs(ast.transform.position.y) > bc2D.size.y / 2)
        {
            ast.transform.position = new Vector3(ast.transform.position.x, -Mathf.Sign(direction.y) * Mathf.Abs(ast.transform.position.y), ast.transform.position.z);
        }
        

        ast.GetComponent<Asteroid>().Initialize(size, Random.Range(minAstSpeed,maxAstSpeed), direction, this);

        countOfAsteroidsOnLevel++;
    }


    void SpawnUFO()
    {
        int direction = (int)Mathf.Sign(Random.Range(-1f, 1f));
        Vector2 spawnPosition = new Vector2(-direction * bc2D.size.x / 2 - 1, Random.Range(-bc2D.size.y / 2 * 0.8f, bc2D.size.y / 2 * 0.8f));
        float speed = bc2D.size.x / 10;

        GameObject ufo = Instantiate(UFOPrefab, spawnPosition, Quaternion.identity, UFOContainer.transform);

        ufo.GetComponent<UFO>().Initialize(direction, speed);
    }


    public void AsteroidIsDestroy()
    {
        countOfAsteroidsDestroed++;

        if (countOfAsteroidsDestroed == countOfAsteroidsOnLevel)
        {
            curLevel++;
            isLevelEnd = true;
            countOfAsteroidsDestroed = 0;
            countOfAsteroidsOnLevel = 0;
        }
    }


    public void NewGame()
    {
        DestroyAllChild(asteroidsContainer.transform);
        DestroyAllChild(UFOContainer.transform);
        DestroyAllChild(bulletsContainer.transform);


        curLevel = 0;
        countOfAsteroidsDestroed = 0;
        countOfAsteroidsOnLevel = 0;

        for (int i = 0; i < curLevel + countAstSpawnOnStart; i++)
            SpawnAsteroid();

        

        //UFO
        nextUFOSpawnTime = Random.Range(minUFOSpawnTime, maxUFOSpawnTime);
        timeAfterUFOSpawn = 0;
    }


    void DestroyAllChild(Transform transform)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        

        if (col.CompareTag("Player"))
        {
            if (col.transform.position.x > bc2D.size.x / 2 || col.transform.position.x < -bc2D.size.x / 2)
                col.transform.position = new Vector3(-col.transform.position.x, col.transform.position.y, col.transform.position.z);

            if (col.transform.position.y > bc2D.size.y / 2 || col.transform.position.y < -bc2D.size.y / 2)
                col.transform.position = new Vector3(col.transform.position.x, -col.transform.position.y, col.transform.position.z);
        }

        if (col.CompareTag("Asteroid"))
        {
            if (col.transform.position.x > bc2D.size.x / 2 || col.transform.position.x < -bc2D.size.x / 2)
                col.transform.position = new Vector3(-col.transform.position.x, col.transform.position.y, col.transform.position.z);

            if (col.transform.position.y > bc2D.size.y / 2 || col.transform.position.y < -bc2D.size.y / 2)
                col.transform.position = new Vector3(col.transform.position.x, -col.transform.position.y, col.transform.position.z);
        }

        if (col.CompareTag("Bullet"))
        {
            if (col.transform.position.x > bc2D.size.x / 2 || col.transform.position.x < -bc2D.size.x / 2)
                col.transform.position = new Vector3(-col.transform.position.x, col.transform.position.y, col.transform.position.z);

            if (col.transform.position.y > bc2D.size.y / 2 || col.transform.position.y < -bc2D.size.y / 2)
                col.transform.position = new Vector3(col.transform.position.x, -col.transform.position.y, col.transform.position.z);
        }

        if (col.CompareTag("UFO"))
        {
            Destroy(col.gameObject);
        }
    }
}