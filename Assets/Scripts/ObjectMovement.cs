using UnityEngine;

public class ObjectMovement : MonoBehaviour {

    public static float defaultSpeed=7f,moveSpeed=7f;
    float destroyTime = 3f;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, destroyTime);
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
	}

}
