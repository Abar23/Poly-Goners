using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public string Type;

    void Start()
    {
        if (PlayerPrefs.HasKey(Type))
            GetComponent<Slider>().value = PlayerPrefs.GetFloat(Type);
    }
}
