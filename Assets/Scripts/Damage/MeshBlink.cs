using UnityEngine;

public class MeshBlink : MonoBehaviour
{
    private float meshBlinkTimer = 0.0f;
    public float meshBlinkMiniDuration = 0.10f;
    private float meshBlinkTotalTimer = 0.0f;
    public float meshBlinkTotalDuration = 1.25f;
    private bool startBlinking = false;

    public bool BlinkMesh = false;

    // Update is called once per frame
    void Update()
    {
        if (startBlinking)
            BlinkEffect();
    }

    private void BlinkEffect()
    {
        meshBlinkTotalTimer += Time.deltaTime;
        if (meshBlinkTotalTimer >= meshBlinkTotalDuration)
        {
            startBlinking = false;
            meshBlinkTotalTimer = 0.0f;
            this.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
            return;
        }

        meshBlinkTimer += Time.deltaTime;
        if (meshBlinkTimer >= meshBlinkMiniDuration)
        {
            meshBlinkTimer = 0.0f;
            if (this.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled == true)
            {
                if (BlinkMesh)
                    this.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            }
            else
            {
                this.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true; 
            }
        }
    }

    public void StartBlinking()
    {
        startBlinking = true;
    }

    public bool IsInvincible()
    {
        return startBlinking;
    }

}
