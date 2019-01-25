using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StripGeneratorScript : MonoBehaviour
{

    public GameObject strip;

    public float waitBeforeSpawn;

    public static bool shouldGenerateStrip;

    public float[] xValues;

    // Start is called before the first frame update
    void Start()
    {
        shouldGenerateStrip = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.isGamePlaying)
        {
            if (shouldGenerateStrip)
            {
                shouldGenerateStrip = false;
                StartCoroutine(WaitAndSpawnStrip());
            }
        }
        else
        {
            StopAllCoroutines();
        }
    }

    IEnumerator WaitAndSpawnStrip()
    {
        yield return new WaitForSeconds(waitBeforeSpawn);

        Instantiate(strip, new Vector2(xValues[0], 8), Quaternion.identity).GetComponent<StripScript>().velocity = GameManager.variableObjectSpeed;
        Instantiate(strip, new Vector2(xValues[2], 8), Quaternion.identity).GetComponent<StripScript>().velocity = GameManager.variableObjectSpeed;

        yield return new WaitForSeconds(waitBeforeSpawn);

        Instantiate(strip, new Vector2(xValues[1], 8), Quaternion.identity).GetComponent<StripScript>().velocity = GameManager.variableObjectSpeed;
        Instantiate(strip, new Vector2(xValues[3], 8), Quaternion.identity).GetComponent<StripScript>().velocity = GameManager.variableObjectSpeed;

        shouldGenerateStrip = true;
    }
}
