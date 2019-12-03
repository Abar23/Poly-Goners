using UnityEngine;
using UnityEngine.UI;

public class IconManager : MonoBehaviour
{
    public Image icon;

    private void Awake()
    {
        //icon.enabled = false;
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

    public void FlipIcon()
    {
        icon.transform.rotation = new Quaternion(icon.transform.rotation.x, 180, icon.transform.rotation.z, 0);
    }

}
