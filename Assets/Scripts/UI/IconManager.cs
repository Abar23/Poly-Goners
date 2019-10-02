using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconManager : MonoBehaviour
{
    public List<Image> icons;

    private Image currentIcon;

    private void Start()
    {
        foreach (Image icon in icons)
        {
            icon.enabled = false;
        }
    }

    public void EnableIcon(string name)
    {
        DisableCurrentIcon();
        foreach (Image icon in icons)
        {
            if (name == icon.name)
            {
                icon.enabled = true;
                currentIcon = icon;
            }
        }
    }

    public void DisableCurrentIcon()
    {
        if (currentIcon != null)
        {
            currentIcon.enabled = false;
        }
    }

}
