using UnityEngine;

public class AdvanceSpawner : MonoBehaviour {

    public GameObject advancedObstacle;
    public float firstPosition, secondPosition, zPos = -10f;
    Vector3 fPos, sPos;
    public float timeLvl, maxSpawnTime = 2f, minSpawnTime = 1f, decreaseFactor = 0.2f, spawnTime,
        advancedLvlTime = 20f;


    void Start()
    {
        timeLvl = Random.Range(10f, 20f);
        fPos = new Vector3(firstPosition, 0, zPos);
        sPos = new Vector3(secondPosition, 0, zPos);
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
        //Debug.Log("advanced spawner enabled");
        //InvokeRepeating("SpawnObject", 2f, spawnTime);
        //InvokeRepeating("ChangeToTStandard",advancedLvlTime, 10f);
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

    public void SpawnObject(float speed)
    {
        AdvancedObstacleMovement.moveSpeed = speed;
        GameObject objectToInstantiate=advancedObstacle;
        if (transform.position == fPos || transform.position == sPos)
        {
            //Debug.Log("Advanced obj spawn");
            Instantiate(objectToInstantiate, transform.position, Quaternion.identity);
        }
        
    }
}
