using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private Color originalColor;
    bool mousedOver = false;

    void Update() {
        if (mousedOver && Input.GetMouseButtonDown(0))
            Destroy(this.gameObject);
    }

    void OnMouseEnter() {
        // highlight object
        originalColor = GetComponent<Renderer>().material.color;
        GetComponent<Renderer>().material.color = Color.green;
        mousedOver = true;
    }

    void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = originalColor;
        mousedOver = false;
    }
}
