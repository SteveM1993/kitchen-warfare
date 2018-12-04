//////////////////////////////
///<summary>
///Author: HyunJin, Seol
///Date Created: 11/30/2018
///Describe what it does here
///This class is for controling the background music volume using the slider UI.
///This needs an audio source to input it into the Main Menu UI.
///</summary>
//////////////////////////////
using UnityEngine;

public class VolumeValueChange : MonoBehaviour {

    private AudioSource audioScr;
    private float musicVolume = 1f;

    //////////////////////////////
    ///<summary> 
    ///Void type method
    ///this method takes the audio sources
    ///</summary>
    //////////////////////////////
    void Start () {
        audioScr = GetComponent<AudioSource>();
	}

    //////////////////////////////
    ///<summary> 
    ///Void type method
    ///Whenever a user modifies the volume bar,
    ///this method change the musicVolume value.
    ///</summary>
    //////////////////////////////
    void Update () {
        audioScr.volume = musicVolume;
	}

    //////////////////////////////
    ///<summary> 
    ///Void type method
    ///Whenever a user modifies the volume bar,
    ///this method adjusts the size of a background music sounds
    ///</summary>
    ///<param name="vol">
    ///float value parameter. The value of the size of volume
    ///</param>
    //////////////////////////////
    public void SetVolume(float vol)
    {
        musicVolume = vol;
    }
}
