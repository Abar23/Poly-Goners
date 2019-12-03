using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrapController : MonoBehaviour
{
    public float arrowSpeed;
    public float shootingDelay;
    public Transform beginningPoint;
    public Transform endPoint;
    public GameObject arrow;

    private float interpolationAmount;
    private Vector3 shootDirection;
    private TrailRenderer arrowTrailRenderer;
    private bool shouldShoot;

    // Start is called before the first frame update
    void Start()
    {
        this.shouldShoot = true;
        this.interpolationAmount = 0.0f;
        this.shootDirection = this.endPoint.position - this.beginningPoint.position;
        this.arrow.transform.rotation = Quaternion.LookRotation(shootDirection);
        this.arrowTrailRenderer = this.arrow.GetComponent<TrailRenderer>();
    }

    void Update()
    {
        if(this.interpolationAmount <= 1.0f && this.shouldShoot)
        {
            this.interpolationAmount += Time.deltaTime * arrowSpeed;
            this.arrow.transform.position = Vector3.Lerp(this.beginningPoint.position, this.endPoint.position, this.interpolationAmount);
        }
        else if(this.shouldShoot)
        {
            this.arrowTrailRenderer.enabled = false;
            this.interpolationAmount = 0.0f;
            this.arrow.transform.position = this.beginningPoint.position;
            this.shouldShoot = false;
            StartCoroutine(FiringDelay());
        }
    }

    IEnumerator FiringDelay()
    {
        yield return new WaitForSeconds(this.shootingDelay);
        this.shouldShoot = true;
        this.arrowTrailRenderer.enabled = true;
    }
}
