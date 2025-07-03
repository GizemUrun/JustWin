using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deadly : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag =="Player")
        {
            FindObjectOfType<AudioManager>().AudioTrigger(AudioManager.SoundFXCat.Lava, transform.position, 0.75f);
            collision.gameObject.transform.position = Manager.lastCheckPoint;
            Manager.AddLives(-1);
        }
    
    }
}
