using System.IO;
using UnityEngine;

public class ScreenshotController : MonoBehaviour
{
    void LateUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            string screenshotNumber = Directory.GetFiles("./Screenshots").Length.ToString();
            ScreenCapture.CaptureScreenshot("./Screenshots/Screenshot_" + screenshotNumber + ".png");
        }
    }
}
