using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class MagicSlider : MonoBehaviour
{

    public MagicBox MagicBox;
    private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }
    
    void Update()
    {
        slider.value = MagicBox.GetMagicPointRatio();
    }
}
