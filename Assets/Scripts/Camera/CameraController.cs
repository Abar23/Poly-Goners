using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target;

    public float cameraZOffset;

    void Start()
    {
        this.TrackTarget();
    }

    void Update()
    {
        this.TrackTarget();
    }

    private void TrackTarget()
    {
        Vector3 targetPosition = this.target.transform.position;
        this.transform.position = new Vector3(targetPosition.x, this.transform.position.y, targetPosition.z + cameraZOffset);
    }
}
