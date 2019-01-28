using UnityEngine;

public class ObjectMovement : MonoBehaviour {

    public static float defaultSpeed=7f,moveSpeed=7f;
    float destroyTime = 3f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (!GameManager.isPaused)
        {
            transform.Translate(-Vector3.up * moveSpeed * Time.deltaTime);
        }

        if (transform.position.y < -6)
        {
            Destroy(gameObject);
        }
	}

}
