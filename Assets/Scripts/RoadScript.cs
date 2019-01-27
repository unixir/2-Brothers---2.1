using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadScript : MonoBehaviour
{

    private bool hasCreatedPrev;

    public float yPos;

    public GameObject roadPrev;

    public bool hasPrevRoad;

    public GameObject road;

    // Start is called before the first frame update
    void Start()
    {
        hasCreatedPrev = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.isGamePlaying)
        {
            transform.Translate(-Vector3.up * GameManager.variableObjectSpeed * Time.deltaTime);

            if (transform.position.y <= 3 && !hasCreatedPrev) 
            {
                hasCreatedPrev = true;
                GameObject roadClone = Instantiate(road, new Vector3(0, yPos, 0), Quaternion.identity);
                roadClone.GetComponent<RoadScript>().yPos = yPos;
                roadClone.GetComponent<RoadScript>().roadPrev = gameObject;
                roadClone.GetComponent<RoadScript>().hasPrevRoad = true;
                roadClone.GetComponent<RoadScript>().road = road;
            }

            if (hasPrevRoad && !roadPrev.ToString().Equals("null"))
            {
                transform.position = new Vector3(0, roadPrev.transform.position.y + (yPos - 3), 0);
            }

            if (transform.position.y < -10.7)
            {
                Destroy(gameObject);
            }
        }
    }
}
