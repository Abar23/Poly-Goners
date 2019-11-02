using System.Collections;
using UnityEngine;

public class Transparency : MonoBehaviour
{
    private Shader oldShader = null;
    private Texture objectTexture = null;
    private MeshRenderer renderer;
    private float transparency = 1.0f;

    void Start()
    {
        this.renderer = this.GetComponent<MeshRenderer>();
        this.objectTexture = this.renderer.material.GetTexture("_MainTex");
        this.oldShader = this.renderer.material.shader;
    }

    void Update()
    {

    }

    IEnumerator MakeTransparent()
    {
        while(this.transparency > 0.3f)
        {
            yield return new WaitForSeconds(0.1f);
            this.transparency -= 0.05f;
            this.renderer.material.SetFloat("_Alpha", this.transparency);
        }
    }

    IEnumerator MakeNonTransparent()
    {
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
