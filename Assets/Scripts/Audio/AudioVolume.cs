using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVolume : MonoBehaviour
{
    public string AudioType;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (PlayerPrefs.HasKey(AudioType))
            GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat(AudioType);
    }
}
