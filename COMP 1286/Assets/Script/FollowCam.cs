using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public GameObject thePlayer;
    

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(thePlayer.transform.position.x, thePlayer.transform.position.y,transform.position.z); 
        //following the location of the player
    }
}
