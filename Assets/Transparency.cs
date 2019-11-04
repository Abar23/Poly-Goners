using System.Collections;
using UnityEngine;

public class Transparency : MonoBehaviour
{
    private bool isTransparent = false;
    private Shader oldShader = null;
    private Texture objectTexture = null;
    private Renderer renderer = null;
    private float transparency = 1.0f;

    void OnEnable()
    {
        this.renderer = this.GetComponent<Renderer>();
        this.objectTexture = this.renderer.material.GetTexture("_MainTex");
        this.oldShader = this.renderer.material.shader;
    }

    public void TurnOnTransparency()
    {
        if (!this.isTransparent)
        {
            StartCoroutine("MakeTransparent");
            this.isTransparent = true;
        }
    }

    public void TurnOffTransparency()
    {
        if (this.isTransparent)
        {
            StartCoroutine("MakeNonTransparent");
            this.isTransparent = false;
        }
    }

    IEnumerator MakeTransparent()
    {
        StopCoroutine("MakeNonTransparent");
        this.renderer.material.shader = Shader.Find("Custom/TransparencyShader");
        this.renderer.material.SetTexture("_MainTex", this.objectTexture);
        this.renderer.material.SetFloat("_Alpha", 1.0f);

        while (this.transparency > 0.3f)
        {
            yield return new WaitForSeconds(0.1f);
            this.transparency -= 0.05f;
            this.renderer.material.SetFloat("_Alpha", this.transparency);
        }
    }

    IEnumerator MakeNonTransparent()
    {
        StopCoroutine("MakeTransparent");
        while (this.transparency < 1.0f)
        {
            yield return new WaitForSeconds(0.1f);
            this.transparency += 0.05f;
            this.renderer.material.SetFloat("_Alpha", this.transparency);
        }

        this.renderer.material.shader = this.oldShader;
        this.renderer.material.SetTexture("_MainTex", this.objectTexture);
    }
}
