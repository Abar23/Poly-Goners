using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDTransparency : MonoBehaviour
{
    public float alpha;

    void Start()
    {
        CanvasGroup canvas = GetComponent<CanvasGroup>();
        canvas.alpha = alpha;
    }
}
