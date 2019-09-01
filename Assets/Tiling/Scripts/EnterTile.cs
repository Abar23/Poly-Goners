using UnityEngine;

public class EnterTile : MonoBehaviour
{
    public string triggerEnterName;
    public string triggerExitName;
    private UnityTile tile;
    void Start()
    {
        if( triggerEnterName != null && triggerEnterName != string.Empty)
            TilingGameManager.Instance.RegisterNewTrigger(triggerEnterName);
        // Todo: (optional) figure a way to get the current tile (which is not a Monobehavior).
        //     Or delete the UnityTile from the callback signatures.
        tile = null; // gameObject.GetComponent<UnityTile>();
    }

    private void OnTriggerEnter(Collider other)
    {
        TilingGameManager.Instance.PublishTrigger(triggerEnterName, tile, other);
    }
    private void OnTriggerExit(Collider other)
    {
        TilingGameManager.Instance.PublishTrigger(triggerExitName, tile, other);
    }
}
