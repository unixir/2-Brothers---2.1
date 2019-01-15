using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedObstacleMovement : MonoBehaviour {

    public static float moveSpeed = 10f;
    float destroyTime = 2f;

    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
}
