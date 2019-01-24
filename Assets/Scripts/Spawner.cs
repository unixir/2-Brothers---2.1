﻿using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject collectible, obstacle, advancedObstacle;
    public float firstPosition, secondPosition,zPos=0f;
    Vector3 fPos, sPos;
    public float timeLvl, maxSpawnTime=2f, minSpawnTime = 1f,decreaseFactor=0.2f,spawnTime,
        advancedLvlTime=30f;
    // Use this for initialization
	void Start () {
        timeLvl = Random.Range(10f, 20f);

        float ratio = Screen.height * 1f / Screen.width;
        Debug.Log(ratio);

        fPos = new Vector3(firstPosition,6,zPos);
        sPos = new Vector3(secondPosition,6,zPos);
        maxSpawnTime = 2f;
        minSpawnTime = 1f;
        //spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        //InvokeRepeating("SpawnObject", 2f, spawnTime);
	}

    private void OnEnable()
    {
        timeLvl = Random.Range(10f, 20f);
        maxSpawnTime = 2f;
        minSpawnTime = 1f;
        ObjectMovement.moveSpeed = 7f;
        spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        //Debug.Log(gameObject.name + " Spawnobject, min,max,spawntime=" + minSpawnTime +","+ maxSpawnTime + "," + spawnTime);
        //InvokeRepeating("SpawnObject", 2f, spawnTime);
        //InvokeRepeating("ChangeToTStandard", advancedLvlTime, advancedLvlTime);
    }

    void ChangeToTStandard()
    {
        this.OnDisable();
        GameManager.ChangeGameMode(GameMode.transitionToStandard);
    }

    void FixedUpdate()
    {
        if (Random.Range(0, 2) == 1)
        {
            transform.position = fPos;
        }
        else
        {
            transform.position = sPos;
        }
    }

    void SpawnObject()
    {
        
        GameObject objectToInstantiate;

        if (Random.Range(0, 2) == 1)
        {
            objectToInstantiate = obstacle;
        }
        else
        {
            objectToInstantiate = collectible;
        }
        if (GameManager.GetGameMode() == GameMode.standard)
        {
            if (transform.position == fPos || transform.position == sPos)
                Instantiate(objectToInstantiate, transform.position, Quaternion.identity);
        }
        if (Time.timeSinceLevelLoad > timeLvl)
        {
            //Debug.Log("inside if");
            CancelInvoke("SpawnObject");
            if (spawnTime > 0.5f)
            {
                maxSpawnTime = maxSpawnTime - decreaseFactor;
                minSpawnTime = minSpawnTime - decreaseFactor;
            }
            timeLvl += 10f;
            ObjectMovement.moveSpeed += 0.2f; 
            spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
            Debug.Log(gameObject.name + " Spawnobject, min,max,spawntime, moveSpeed=" + minSpawnTime + ", " + maxSpawnTime + ", " + spawnTime+", "+ObjectMovement.moveSpeed);
            InvokeRepeating("SpawnObject", 1f, spawnTime);
        }

        
    }

    public void SpawnObject(float speed)
    {
        GameObject objectToInstantiate;
        ObjectMovement.moveSpeed = speed;

        if (Random.Range(0, 2) == 1)
        {
            objectToInstantiate = obstacle;
        }
        else
        {
            objectToInstantiate = collectible;
        }
        if (transform.position == fPos || transform.position == sPos)
        {
            //Debug.Log("here");
            Instantiate(objectToInstantiate, transform.position, Quaternion.identity);
        }
            
        
    }

    private void OnDisable()
    {
        //GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        //GameObject[] collectibles = GameObject.FindGameObjectsWithTag("Collectible");
        //if (obstacles.Length != 0 || collectibles.Length == 0)
        //{
        //    foreach (GameObject gameObject in obstacles)
        //    {
        //        Destroy(gameObject);
        //    }
        //    foreach (GameObject gameObject in collectibles)
        //    {
        //        Destroy(gameObject);
        //    }
        //}
        ObjectMovement.moveSpeed = ObjectMovement.defaultSpeed;
        timeLvl = Random.Range(10f, 20f);
        maxSpawnTime = 2f;
        minSpawnTime = 1f;
        spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        CancelInvoke("SpawnObject");
    }
}
