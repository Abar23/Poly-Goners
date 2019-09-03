using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterTileOld : MonoBehaviour
{
    public AudioClip audioClip;
    public AudioSource audioSource;

    //     Teaching moment
    // Bugs: This class is used on Prefabs. Would like to aviod having to edit it to use it.
    //   1. Having each Prefab have an audio source (below) leads to an audio source per tile.
    //   2. Audio clip is fixed in the prefab and cannot be changed without editing the prefab tile.
    //   3. Audio clip is the same for all instances of the prefab.
    //   4. Other "beahaviors / events" cannot be added or controlled across the tiling.
    // Solution: The prefab should coordinate with a GameManager. Could find this on the parent.
    //
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //listener.Play();
        audioSource.clip = audioClip;
        audioSource.loop = true;
        audioSource.Play();
    }
    private void OnTriggerExit(Collider other)
    {
        audioSource.loop = false;// Stop();
    }
}
