using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {

    public AudioClip positiveHit, negativeHit;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        if (other.tag == "Collectible")
        {
            //Debug.Log("object" + other.tag);
            audioSource.PlayOneShot(positiveHit);
            other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            GameManager.instance.Invoke("IncreaseScore",0f);
            Destroy(other.gameObject);
        }
        if (other.tag == "Obstacle")
        {
            //Debug.Log("object" + other.tag);
            //audioSource.PlayOneShot(negativeHit);
            other.gameObject.GetComponent<ObjectMovement>().enabled = false;
            GameManager.instance.GameOver();
        }
        if (other.tag == "AdvancedObstacle")
        {
            //Debug.Log("object" + other.tag);
            audioSource.PlayOneShot(negativeHit);
            other.gameObject.GetComponent<AdvancedObstacleMovement>().enabled = false;
            GameManager.instance.GameOver();
        }
    }
}
