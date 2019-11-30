using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeAlpha : MonoBehaviour
{
    public Image Tint;
    public float Alpha;

    private void OnEnable()
    {
        Tint.color = new Color(Tint.color.r, Tint.color.g, Tint.color.b, Alpha);
    }

    private void OnDisable()
    {
        Tint.color = new Color(Tint.color.r, Tint.color.g, Tint.color.b, 0);
    }
}
