using System.Collections;
using UnityEngine;

public class Transparency : MonoBehaviour
{
    private bool isTransparent = false;
    private Shader oldShader = null;
    private Texture objectTexture = null;
    private Renderer gameObjectRenderer = null;
    private float transparency = 1.0f;

    void OnEnable()
    {
        this.gameObjectRenderer = this.GetComponent<Renderer>();
        this.objectTexture = this.gameObjectRenderer.material.GetTexture("_MainTex");
        this.oldShader = this.gameObjectRenderer.material.shader;
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
        this.gameObjectRenderer.material.shader = Shader.Find("Custom/TransparencyShader");
        this.gameObjectRenderer.material.SetTexture("_MainTex", this.objectTexture);
        this.gameObjectRenderer.material.SetFloat("_Alpha", 1.0f);

        while (this.transparency > 0.3f)
        {
            yield return new WaitForSeconds(0.1f);
            this.transparency -= 0.125f;
            this.gameObjectRenderer.material.SetFloat("_Alpha", this.transparency);
        }
    }

    IEnumerator MakeNonTransparent()
    {
        StopCoroutine("MakeTransparent");
        while (this.transparency < 1.0f)
        {
            yield return new WaitForSeconds(0.1f);
            this.transparency += 0.125f;
            this.gameObjectRenderer.material.SetFloat("_Alpha", this.transparency);
        }

        this.gameObjectRenderer.material.shader = this.oldShader;
        this.gameObjectRenderer.material.SetTexture("_MainTex", this.objectTexture);
    }
}
