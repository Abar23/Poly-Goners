using UnityEngine;
using UnityEngine.UI;

public class DisableOnStart : MonoBehaviour
{
    void Start()
    {
        GetComponent<Image>().enabled = false;    
    }
}
