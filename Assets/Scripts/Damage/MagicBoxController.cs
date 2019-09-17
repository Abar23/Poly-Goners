using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBoxController : MonoBehaviour
{

    public MagicBox Box;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Box.FireMagic(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Box.FireMagic(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Box.FireMagic(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Box.FireMagic(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Box.FireMagic(4);
        }
    }
}
