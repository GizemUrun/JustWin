using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum SoundFXCat {FootStepConcrete, Jump, CheckPoint, OpenDoor, PickupKey, PickupCoin, Ladder, Gateway,
        Chains, Lava, Win}
    public GameObject audioObject;
    //arrays
    public AudioClip[] footsteps;
    public AudioClip[] ladders;
    public AudioClip[] jumpAudio;
    public AudioClip[] checkPoint;
    public AudioClip[] openDoorAudio;
    public AudioClip[] keyClips;
    public AudioClip[] coinClips;

    public AudioClip[] gateway;
    public AudioClip[] chains;
    public AudioClip[] lava;
    public AudioClip[] win;



    public void AudioTrigger (SoundFXCat audioType, Vector3 audioPosition, float volume)
    {
        GameObject newAudio = GameObject.Instantiate(audioObject, audioPosition, Quaternion.identity);
        ControlAudio ca = newAudio.GetComponent<ControlAudio>();
        switch (audioType)
        {
            case (SoundFXCat.FootStepConcrete):
                ca.myClip = footsteps[Random.Range(0, footsteps.Length)];
                break;
            case (SoundFXCat.Jump):
                ca.myClip = jumpAudio[Random.Range(0, jumpAudio.Length)];
                break;
            case (SoundFXCat.CheckPoint):
                ca.myClip = checkPoint[Random.Range(0, checkPoint.Length)];
                break;
            case (SoundFXCat.OpenDoor):
                ca.myClip = openDoorAudio[Random.Range(0, openDoorAudio.Length)];
                break;
            case (SoundFXCat.PickupKey):
                ca.myClip = keyClips[Random.Range(0, keyClips.Length)];
                break;
            case (SoundFXCat.PickupCoin):
                ca.myClip = coinClips[Random.Range(0, coinClips.Length)];
                break;
            case (SoundFXCat.Ladder):
                ca.myClip = ladders[Random.Range(0, ladders.Length)];
                break;
            case (SoundFXCat.Gateway):
                ca.myClip = gateway[Random.Range(0, gateway.Length)];
                break;
            case (SoundFXCat.Chains):
                ca.myClip = chains[Random.Range(0, chains.Length)];
                break;
            case (SoundFXCat.Lava):
                ca.myClip = lava[Random.Range(0, lava.Length)];
                break;
            case (SoundFXCat.Win):
                ca.myClip = win[Random.Range(0, win.Length)];
                break;

        }
        ca.volume = volume;
        ca.StartAudio();
    }

}
