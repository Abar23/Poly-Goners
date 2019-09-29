using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    
    public IEnumerator CameraShakeEffect(float maganitude, float duration)
    {
        
        float timeElapse = 0f;
        while (timeElapse < duration)
        {
            float x = Random.Range(-maganitude, maganitude);
            float y = Random.Range(-maganitude, maganitude);
            float z = Random.Range(-maganitude, maganitude);

            transform.position = new Vector3(x, y, z) + transform.position;
            timeElapse += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

}
