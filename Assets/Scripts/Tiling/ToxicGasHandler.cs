using CSE5912;
using System.Collections;
using UnityEngine;

public class ToxicGasHandler : MonoBehaviour
{
    public Collider target;
    public AudioClip audioClip;
    public AudioSource audioSource;
    private string triggerEnteredName = "ToxicGasEntered";
    private string triggerExitName = "ToxicGasExited";
    private bool inTrigger = false;
    void Start()
    {
        // Registering could be a function of the TileSet.
        TilingGameManager.Instance.RegisterNewTrigger(triggerEnteredName);
        TilingGameManager.Instance.SubscribeToTrigger(triggerEnteredName, OnToxicGasEntered);
        TilingGameManager.Instance.RegisterNewTrigger(triggerExitName);
        TilingGameManager.Instance.SubscribeToTrigger(triggerExitName, OnToxicGasExited);
    }
    private void OnToxicGasEntered(string triggerName, UnityTile tile, Collider collider)
    {
        if (collider == target)
        {
            // Play Cough
            audioSource.clip = audioClip;
            audioSource.loop = true;
            audioSource.Play();
            // Take Damage
            inTrigger = true;
            StartCoroutine(ContinuousToxicGasDamage(collider.gameObject));
        }
    }
    private IEnumerator ContinuousToxicGasDamage(GameObject gameObject)
    {
        while (inTrigger)
        {
            DamageManager.Instance.TakeDamage(gameObject, DamageManager.DamageType.GAS);
            yield return new WaitForSeconds(GameConstants.Instance.GasDamageDuration);
        }
    }
    private void OnToxicGasExited(string triggerName, UnityTile tile, Collider collider)
    {
        if (collider == target)
        {
            inTrigger = false;
            // Let the audio finish and then stop it.
            audioSource.loop = false;
        }
    }
}
