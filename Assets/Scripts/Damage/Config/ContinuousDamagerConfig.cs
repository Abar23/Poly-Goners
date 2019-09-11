using UnityEngine;

[CreateAssetMenu(fileName = "Damager Data", menuName = "ScriptableObjects/ContinuousDamagerConfig", order = 1)]
public class ContinuousDamagerConfig : DamagerConfig
{
    public int DamageDecay;
    public int DamageInterval;
    public int DamageIteration;
}
