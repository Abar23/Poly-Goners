using UnityEngine;

public class CameraBillboard : MonoBehaviour
{
    public bool BillboardX = true;
    public bool BillboardY = true;
    public bool BillboardZ = true;
    public float OffsetToCamera;
    protected Vector3 localStartPosition;

    // Use this for initialization
    void Start()
    {
        localStartPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        SplitScreen splitScreen = Camera.main.GetComponent<SplitScreen>();
        if(splitScreen != null)
        {
            transform.LookAt(transform.position + splitScreen.GetMainCamera().transform.rotation * Vector3.forward,
                                                       splitScreen.GetComponent<SplitScreen>().GetMainCamera().transform.rotation * Vector3.up);
            if (!BillboardX || !BillboardY || !BillboardZ)
                transform.rotation = Quaternion.Euler(BillboardX ? transform.rotation.eulerAngles.x : 0f, BillboardY ? transform.rotation.eulerAngles.y : 0f, BillboardZ ? transform.rotation.eulerAngles.z : 0f);
            transform.localPosition = localStartPosition;
            transform.position = transform.position + transform.rotation * Vector3.forward * OffsetToCamera;
        }
    }
}