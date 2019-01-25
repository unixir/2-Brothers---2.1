using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StripScript : MonoBehaviour
{

    public float velocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.isGamePlaying)
        {
            transform.Translate(-Vector3.up * velocity * Time.deltaTime);
        } else
        {
            Destroy(gameObject);
        }

        if (transform.position.y < -8)
        {
            Destroy(gameObject);
        }
    }
}
