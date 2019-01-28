using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public GameObject player;
    public Animator playerAC;
    public float moveSpeed,transitionSpeed=5f;
    public bool isOnFirstPos = true, advancedPosition=false, isOnAdFirstPos=false;
    public bool positionFirst;

    public void Start()
    {
        playerAC = GetComponent<Animator>();
        Reset();
        //playerAC.SetBool("OnFirstPos", isOnFirstPos);
        
    }

    public void Reset()
    {

        isOnFirstPos = positionFirst;
        if (isOnFirstPos)
        {
            transform.position = new Vector3(0 + transform.parent.position.x, transform.position.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(1.25f + transform.parent.position.x, transform.position.y, transform.position.z);
        }
        transform.rotation = Quaternion.identity;
    }

    public void OnClick()
    {
        if (advancedPosition)
        {
            isOnAdFirstPos = !isOnAdFirstPos;
            playerAC.SetBool("OnAdFirstPos", isOnAdFirstPos);
            playerAC.SetTrigger("shouldAnimate");
        }
        else
        {
            isOnFirstPos = !isOnFirstPos;
            playerAC.SetBool("OnFirstPos", isOnFirstPos);
            playerAC.SetTrigger("shouldAnimate");
        }
    }   

}
