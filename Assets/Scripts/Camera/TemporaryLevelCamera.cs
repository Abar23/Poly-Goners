using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryLevelCamera : MonoBehaviour
{
    public float cameraSpeed;

    // Update is called once per frame
    void Update() {
    if(Input.GetKey(KeyCode.RightArrow))
        transform.Translate(new Vector3(cameraSpeed * Time.deltaTime, 0, 0), Space.World);
    if(Input.GetKey(KeyCode.LeftArrow))
        transform.Translate(new Vector3(-cameraSpeed * Time.deltaTime, 0, 0), Space.World);
    if(Input.GetKey(KeyCode.DownArrow))
        transform.Translate(new Vector3(0, 0, -cameraSpeed * Time.deltaTime), Space.World);
    if(Input.GetKey(KeyCode.UpArrow))
        transform.Translate(new Vector3(0, 0, cameraSpeed * Time.deltaTime), Space.World);    
    }
}
