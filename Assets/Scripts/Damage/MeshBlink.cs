using UnityEngine;

public class MeshBlink : MonoBehaviour
{
    private float meshBlinkTimer = 0.0f;
    private float meshBlinkMiniDuration = 0.15f;
    private float meshBlinkTotalTimer = 0.0f;
    private float meshBlinkTotalDuration = 1.5f;
    private bool startBlinking = false;

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
