using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScrollScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.isGamePlaying && !GameManager.isPaused)
        {
            transform.Translate(Vector3.up * GameManager.variableObjectSpeed * Time.deltaTime);
        }
    }
}
