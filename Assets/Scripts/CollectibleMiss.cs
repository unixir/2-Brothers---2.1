using UnityEngine;

public class CollectibleMiss : MonoBehaviour {

    public AudioClip negativeHit;
    public AudioSource audioSource;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Collectible")
        {
            GameManager.instance.GameOver();
            audioSource.PlayOneShot(negativeHit);

        }
    }
}
