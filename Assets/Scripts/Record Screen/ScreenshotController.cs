using System.IO;
using UnityEngine;

public class ScreenshotController : MonoBehaviour
{
    private string screenshotPath = "./Screenshots";

    void Start()
    {
        bool filePathExists = Directory.Exists(this.screenshotPath);
        if(!filePathExists)
        {
            Directory.CreateDirectory(this.screenshotPath);
        }
    }

    void LateUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            string screenshotNumber = Directory.GetFiles("./Screenshots").Length.ToString();
            ScreenCapture.CaptureScreenshot("./Screenshots/Screenshot_" + screenshotNumber + ".png");
        }
    }
}
