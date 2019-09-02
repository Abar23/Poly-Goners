using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource effectSource;
    public AudioSource musicSource;
    public static AudioManager instance = null;
    
    // Start is called before the first frame update
    void Awake()
    {
        if (instance = null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
