using UnityEngine;
using UnityEngine.UI;

public class IconManager : MonoBehaviour
{
    public Image icon;

    private void Start()
    {
        icon.enabled = false;
    }

    public void EnableIcon(GameObject itemWithIcon)
    {
        icon.enabled = true;
        icon.sprite = itemWithIcon.GetComponent<ItemIcon>().icon;
    }

    public void DisableCurrentIcon()
    {
        if (icon != null)
        {
            icon.enabled = false;
        }
    }

}
