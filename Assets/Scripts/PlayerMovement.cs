using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public GameObject player;
    public Animator playerAC;
    public float moveSpeed,transitionSpeed=5f;
    public bool isOnFirstPos = true, advancedPosition=false, isOnAdFirstPos=false;
    public bool positionFirst=true;

    public void Start()
    {
        playerAC = GetComponent<Animator>();
        isOnFirstPos = positionFirst;
        playerAC.SetBool("OnFirstPos", isOnFirstPos);
        
    }

    public void OnClick()
    {
        if (advancedPosition)
        {
            isOnAdFirstPos = !isOnAdFirstPos;
            playerAC.SetBool("OnAdFirstPos", isOnAdFirstPos);
        }
        else
        {
            isOnFirstPos = !isOnFirstPos;
            playerAC.SetBool("OnFirstPos", isOnFirstPos);
        }
        Debug.Log(gameObject.name + " advancedPosition" + advancedPosition + ", isOnAdFirstPos" + isOnAdFirstPos + ", isOnFirstPos" + isOnFirstPos);
    }   

}
