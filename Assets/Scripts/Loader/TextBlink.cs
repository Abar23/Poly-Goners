using UnityEngine;
using UnityEngine.UI;

public class TextBlink : MonoBehaviour
{
    
    [Range(0, 10)]
    public float Speed;

    public Text Self;

    private bool FadeIn = false;

    void Update()
    {
        Color color = Self.color;
        if (FadeIn)
        {
            color.a = Mathf.Clamp01(color.a + Speed * Time.deltaTime);
            FadeIn = color.a < 1;
        }
        else
        {
            color.a = Mathf.Clamp01(color.a - Speed * Time.deltaTime);
            FadeIn = color.a <= 0;
        }
        Self.color = color;
    }
}
